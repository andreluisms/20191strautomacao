using Model;
using Model.ViewModel;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class MonitoramentoService : IMonitoramentoService
    {
        private readonly str_dbContext _context;
        public MonitoramentoService(str_dbContext context)
        {
            _context = context;
        }

        public List<MonitoramentoModel> GetAll() => _context.Monitoramento.Select(m => new MonitoramentoModel { Id = m.Id, EquipamentoId = m.Equipamento, Estado = Convert.ToBoolean(m.Estado) }).ToList();

        public MonitoramentoModel GetById(int id) => _context.Monitoramento.Where(m => m.Id == id).Select(m => new MonitoramentoModel { Id = m.Id, EquipamentoId = m.Equipamento, Estado = Convert.ToBoolean(m.Estado) }).FirstOrDefault();

        public MonitoramentoModel GetByIdEquipamento(int idEquipamento) => _context.Monitoramento.Where(m => m.Equipamento == idEquipamento).Select(m => new MonitoramentoModel { Id = m.Id, EquipamentoId = m.Equipamento, Estado = Convert.ToBoolean(m.Estado) }).FirstOrDefault();

        public List<MonitoramentoModel> GetByIdSala(int idSala)
        {

            var monitoramentos = new List<MonitoramentoModel>(from m in _context.Monitoramento
                                                              join e in _context.Equipamento on m.Equipamento equals e.Id
                                                              where e.Sala == idSala
                                                              select new MonitoramentoModel
                                                              {
                                                                  Id = m.Id,
                                                                  Estado = Convert.ToBoolean(m.Estado),
                                                                  EquipamentoId = m.Equipamento,
                                                                  EquipamentoNavigation = new EquipamentoModel { Id = e.Id, TipoEquipamento = e.TipoEquipamento, Sala = e.Sala },
                                                              });


            return monitoramentos;
        }


        public bool Insert(MonitoramentoModel model)
        {
            try
            {
                if (GetByIdEquipamento(model.EquipamentoId) != null)
                    return true;

                _context.Add(SetEntity(model));
                return _context.SaveChanges() == 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool MonitorarSala(int idUsuario, MonitoramentoViewModel monitoramento)
        {
            var _equipamentoSala = new EquipamentoService(_context);
            var _monitoramentoService = new MonitoramentoService(_context);
            var equipamentos = _equipamentoSala.GetByIdSala(monitoramento.SalaId);

            foreach (var eq in equipamentos)
            {
                if (monitoramento.SalaParticular)
                {
                    var _salaParticular = new SalaParticularService(_context);
                    if (_salaParticular.GetByIdUsuarioAndIdSala(idUsuario, eq.Sala) == null)
                        throw new ServiceException("Houve um problema e o monitoramento não pode ser finalizado, por favor tente novamente mais tarde!");
                }
                else
                {
                    var _horarioSalaService = new HorarioSalaService(_context);
                    if (!_horarioSalaService.VerificaSeEstaEmHorarioAula(idUsuario, eq.Sala))
                        throw new ServiceException("Você não está no horário reservado para monitorar essa sala!");
                }

                var model = _monitoramentoService.GetByIdEquipamento(eq.Id);
                model.Estado = eq.TipoEquipamento.Equals(EquipamentoModel.TIPO_CONDICIONADOR) ? monitoramento.ArCondicionado : monitoramento.Luzes;

                if (!EnviarComandosMonitoramento(model))
                    throw new ServiceException("Não foi possível concluir seu monitoramento pois não foi possível estabelecer conexão com a sala!");

                Update(model);
            }

            return true;
        }

        public bool MonitorarEquipamento(int idUsuario, MonitoramentoModel model)
        {
            var _equipamentoSala = new EquipamentoService(_context);
            var equipamento = _equipamentoSala.GetByIdEquipamento(model.EquipamentoId);

            if (model.SalaParticular)
            {
                var _salaParticular = new SalaParticularService(_context);
                if (_salaParticular.GetByIdUsuarioAndIdSala(idUsuario, equipamento.Sala) == null)
                    throw new ServiceException("Houve um problema e o monitoramento não pode ser finalizado, por favor tente novamente mais tarde!");
            }
            else
            {
                var _horarioSalaService = new HorarioSalaService(_context);
                if (!_horarioSalaService.VerificaSeEstaEmHorarioAula(idUsuario, equipamento.Sala))
                    throw new ServiceException("Você não está no horário reservado para monitorar essa sala!");
            }

            if (!EnviarComandosMonitoramento(model))
                throw new ServiceException("Não foi possível concluir seu monitoramento pois não foi possível estabelecer conexão com a sala!");

            return Update(model);
        }

        public bool Update(MonitoramentoModel model)
        {
            try
            {
                _context.Update(SetEntity(model));
                return _context.SaveChanges() == 1;
            }
            catch (Exception)
            {
                throw new ServiceException("Houve um problema ao tentar fazer monitoramento da sala, por favor tente novamente em alguns minutos!");
            }
        }

        private bool EnviarComandosMonitoramento(MonitoramentoModel solicitacao)
        {
            try
            {
                var modelDesatualizado = GetById(solicitacao.Id);
                bool comandoEnviadoComSucesso = true;

                if (solicitacao.Estado != modelDesatualizado.Estado)
                {
                    var _hardwareDeSalaService = new HardwareDeSalaService(_context);
                    var _equipamentoServiceService = new EquipamentoService(_context);
                    var equipamento = _equipamentoServiceService.GetByIdEquipamento(solicitacao.EquipamentoId);
                    var hardwareDeSala = _hardwareDeSalaService.GetByIdSalaAndTipoHardware(equipamento.Sala, TipoHardwareModel.CONTROLADOR_DE_SALA).FirstOrDefault();
                    var clienteSocket = new ClienteSocketService(hardwareDeSala.Ip);

                    if (equipamento.TipoEquipamento.Equals(EquipamentoModel.TIPO_CONDICIONADOR))
                    {
                        var _codigosInfravermelhoService = new CodigoInfravermelhoService(_context);
                        var idOperacao = solicitacao.Estado ? OperacaoModel.OPERACAO_LIGAR : OperacaoModel.OPERACAO_DESLIGAR;
                        var codigosInfravermelho = _codigosInfravermelhoService.GetByIdOperacaoAndIdEquipamento(equipamento.Id, idOperacao);

                        if (codigosInfravermelho == null)
                            throw new ServiceException("Houve um problema e o monitoramento não pode ser finalizado, por favor tente novamente mais tarde!");

                        var mensagem = "CONDICIONADOR;" + codigosInfravermelho.Codigo + ";";

                        clienteSocket.AbrirConexao();
                        var status = clienteSocket.EnviarComando(mensagem);
                        clienteSocket.FecharConexao();

                        solicitacao.Estado = status.Contains("AC-ON");
                        comandoEnviadoComSucesso = status != null;
                    }
                    else
                    {
                        var mensagem = "LUZES;" + solicitacao.Estado + ";";

                        clienteSocket.AbrirConexao();
                        var status = clienteSocket.EnviarComando(mensagem);
                        clienteSocket.FecharConexao();

                        solicitacao.Estado = status.Contains("L-ON");
                        comandoEnviadoComSucesso = status != null;
                    }
                }

                return comandoEnviadoComSucesso;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private Monitoramento SetEntity(MonitoramentoModel model)
        {
            return new Monitoramento
            {
                Id = model.Id,
                Estado = Convert.ToByte(model.Estado),
                Equipamento = model.EquipamentoId
            };
        }
    }
}

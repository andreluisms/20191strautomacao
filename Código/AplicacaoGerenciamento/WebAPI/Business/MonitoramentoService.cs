﻿using Model;
using Model.AuxModel;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class MonitoramentoService : IMonitoramentoService
    {
        private readonly STR_DBContext _context;
        public MonitoramentoService(STR_DBContext context)
        {
            _context = context;
        }

        public List<MonitoramentoModel> GetAll() => _context.Monitoramento.Select(m => new MonitoramentoModel { Id = m.Id, SalaId = m.Sala, ArCondicionado = Convert.ToBoolean(m.ArCondicionado), Luzes = Convert.ToBoolean(m.Luzes) }).ToList();

        public MonitoramentoModel GetById(int id) => _context.Monitoramento.Where(m => m.Id == id).Select(m => new MonitoramentoModel { Id = m.Id, SalaId = m.Sala, ArCondicionado = Convert.ToBoolean(m.ArCondicionado), Luzes = Convert.ToBoolean(m.Luzes) }).FirstOrDefault();

        public MonitoramentoModel GetByIdSala(int idSala) => _context.Monitoramento.Where(m => m.Sala == idSala).Select(m => new MonitoramentoModel { Id = m.Id, SalaId = m.Sala, ArCondicionado = Convert.ToBoolean(m.ArCondicionado), Luzes = Convert.ToBoolean(m.Luzes) }).FirstOrDefault();

        public bool Insert(MonitoramentoModel model)
        {
            try
            {
                if (GetByIdSala(model.SalaId) != null)
                    return true;

                _context.Add(SetEntity(model));
                return _context.SaveChanges() == 1 ? true : false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public bool MonitorarSala(int idUsuario, MonitoramentoModel model)
        {
            var _horarioSalaService = new HorarioSalaService(_context);
            var _salaParticular = new SalaParticularService(_context);

            try
            {
                if (model.SalaParticular)
                {
                    if (_salaParticular.GetByIdUsuarioAndIdSala(idUsuario, model.SalaId) == null)
                        throw new ServiceException("Houve um problema e o monitoramento não pode ser finalizado, por favor tente novamente mais tarde!");
                }
                else
                {
                    if (!_horarioSalaService.VerificaSeEstaEmHorarioAula(idUsuario, model.SalaId))
                        throw new ServiceException("Você não está no horário reservado para monitorar essa sala!");
                }

                if(!EnviarComandosMonitoramento(model))
                    throw new ServiceException("Não foi possível concluir seu monitoramento pois não foi possível estabelecer conexão com a sala!");

                return Update(model);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Update(MonitoramentoModel model)
        {
            try
            {
                _context.Update(SetEntity(model));
                return _context.SaveChanges() == 1 ? true : false;
            }
            catch (Exception)
            {
                throw new ServiceException("Houve um problema ao tentar fazer monitoramento da sala, por favor tente novamente em alguns minutos!");
            }
        }


        private bool EnviarComandosMonitoramento(MonitoramentoModel solicitacao)
        {
            var modelDesatualizado = GetById(solicitacao.Id); 
            var _codigosInfravermelhoService = new CodigoInfravermelhoService(_context);
            var _equipamentoServiceService = new EquipamentoService(_context);
            var _hardwareDeSalaService = new HardwareDeSalaService(_context);
            bool comandoEnviadoComSucesso = false;

            /* 
             * Verifica qual o equipamento foi 'monitorado' para ligar/desligar   
             */
            if (!(solicitacao.ArCondicionado == modelDesatualizado.ArCondicionado))
            {
                var idOperacao = solicitacao.ArCondicionado ? OperacaoModel.OPERACAO_LIGAR : OperacaoModel.OPERACAO_DESLIGAR;
                var equipamento = _equipamentoServiceService.GetByIdSalaAndTipoEquipamento(solicitacao.SalaId, EquipamentoModel.TIPO_CONDICIONADOR);
                var codigosInfravermelho = _codigosInfravermelhoService.GetByIdOperacaoAndIdEquipamento(equipamento.Id, idOperacao);
                var hardwareDeSala = _hardwareDeSalaService.GetByIdSalaAndTipoHardware(solicitacao.SalaId, TipoHardwareModel.CONTROLADOR_DE_SALA).FirstOrDefault();

                if(!codigosInfravermelho.Any())
                    throw new ServiceException("Houve um problema e o monitoramento não pode ser finalizado, por favor tente novamente mais tarde!");

                var mensagem = MontarMensagemComComandosIr("condicionador;", codigosInfravermelho);
                var clienteSocket = new ClienteSocketService(hardwareDeSala.Ip);
                comandoEnviadoComSucesso = clienteSocket.EnviarComando(mensagem);
            }
            else if(!(solicitacao.Luzes == modelDesatualizado.Luzes))
            {
                int idOperacao = solicitacao.Luzes ? OperacaoModel.OPERACAO_LIGAR : OperacaoModel.OPERACAO_DESLIGAR;
                var equipamento = _equipamentoServiceService.GetByIdSalaAndTipoEquipamento(solicitacao.SalaId, EquipamentoModel.TIPO_LUZES);
                var codigosInfravermelho = _codigosInfravermelhoService.GetByIdOperacaoAndIdEquipamento(equipamento.Id, idOperacao);
                var hardwareDeSala = _hardwareDeSalaService.GetByIdSalaAndTipoHardware(solicitacao.SalaId, TipoHardwareModel.CONTROLADOR_DE_SALA).FirstOrDefault();

                if (!codigosInfravermelho.Any())
                    throw new ServiceException("Houve um problema e o monitoramento não pode ser finalizado, por favor tente novamente mais tarde!");

                var mensagem = MontarMensagemComComandosIr("luzes;", codigosInfravermelho);

                var clienteSocket = new ClienteSocketService(hardwareDeSala.Ip);
                comandoEnviadoComSucesso = clienteSocket.EnviarComando(mensagem);
            }

            return comandoEnviadoComSucesso;
        }

        private string MontarMensagemComComandosIr(string cabecalho, List<CodigoInfravermelhoModel> codigosInfravermelho)
        {
            foreach (var s in codigosInfravermelho)
                cabecalho += s.Codigo + ";";

            return cabecalho;
        }

        private Monitoramento SetEntity(MonitoramentoModel model)
        {
            return new Monitoramento
            {
                Id = model.Id,
                Luzes = Convert.ToByte(model.Luzes),
                ArCondicionado = Convert.ToByte(model.ArCondicionado),
                Sala = model.SalaId
            };
        }
    }
}

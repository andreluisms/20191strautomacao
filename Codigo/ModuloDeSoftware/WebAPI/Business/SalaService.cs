using Model;
using Model.AuxModel;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class SalaService : ISalaService
    {
        private readonly str_dbContext _context;
        public SalaService(str_dbContext context)
        {
            _context = context;
        }
        public List<SalaModel> GetAll() => _context.Sala.Select(s => new SalaModel { Id = s.Id, Titulo = s.Titulo, BlocoId = s.Bloco }).ToList();

        public SalaModel GetById(int id) => _context.Sala.Where(s => s.Id == id).Select(s => new SalaModel { Id = s.Id, Titulo = s.Titulo, BlocoId = s.Bloco }).FirstOrDefault();

        public List<SalaModel> GetByIdBloco(int id) => _context.Sala.Where(s => s.Bloco == id).Select(s => new SalaModel { Id = s.Id, Titulo = s.Titulo, BlocoId = s.Bloco }).ToList();
        public SalaModel GetByTitulo(string titulo) => _context.Sala.Where(s => s.Titulo.ToUpper().Equals(titulo.ToUpper())).Select(s => new SalaModel { Id = s.Id, Titulo = s.Titulo, BlocoId = s.Bloco }).FirstOrDefault();


        public bool InsertSalaWithHardwares(SalaAuxModel sala, int idUsuario)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var salaInserida = Insert(new SalaModel { Id = sala.Sala.Id, Titulo = sala.Sala.Titulo, BlocoId = sala.Sala.BlocoId });
                    if (salaInserida == null)
                        throw new ServiceException("Houve um problema ao cadastrar sala, tente novamente em alguns minutos!");

                    if (sala.HardwaresSala.Count > 0)
                    {
                        var _hardwareDeSalaService = new HardwareDeSalaService(_context);

                        foreach (var item in sala.HardwaresSala)
                        {
                            if (_hardwareDeSalaService.GetByMAC(item.MAC, idUsuario) != null)
                                throw new ServiceException("Já existe um dispositivos com o endereço MAC " + item.MAC + " informado, corrija e tente novamente!");

                            if (item.TipoHardwareId.Id == TipoHardwareModel.CONTROLADOR_DE_SALA && _hardwareDeSalaService.GetByIp(item.Ip, idUsuario) != null)
                                throw new ServiceException("Já existe um dispositivos com o endereço IP  " + item.Ip + "  informado, corrija e tente novamente!");

                            _hardwareDeSalaService.Insert(new HardwareDeSalaModel { MAC = item.MAC, SalaId = salaInserida.Id, TipoHardwareId = item.TipoHardwareId.Id, Ip = item.Ip }, idUsuario);
                        }

                        transaction.Commit();
                        return true;

                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return true;
        }

        public SalaModel Insert(SalaModel salaModel)
        {
            try
            {
                var sala = GetByTitulo(salaModel.Titulo);
                if (sala != null && sala.BlocoId == salaModel.BlocoId)
                    throw new ServiceException("Uma sala com o mesmo Titulo já está associada a este bloco!");

                var entity = new Sala();
                _context.Add(SetEntity(salaModel, entity));
                var save = _context.SaveChanges();

                if (save == 1)
                {
                    salaModel.Id = entity.Id; return salaModel;
                }
                else return null;
            }
            catch (Exception) { throw; }
        }

        public bool Remove(int id)
        {
            var _hardwareSalaService = new HardwareDeSalaService(_context);
            var _minhaSalaService = new SalaParticularService(_context);
            var _horarioSalaService = new HorarioSalaService(_context);
            var _planejamentoService = new PlanejamentoService(_context);

            try
            {
                if (_hardwareSalaService.GetByIdSala(id).Count == 0 && _minhaSalaService.GetByIdSala(id).Count == 0 &&
                    _horarioSalaService.GetByIdSala(id).Count == 0 && _planejamentoService.GetByIdSala(id).Count == 0)
                {

                    var x = _context.Sala.Where(s => s.Id == id).FirstOrDefault();
                    if (x != null)
                    {
                        _context.Remove(x);
                        return _context.SaveChanges() == 1;
                    }
                }
                else throw new ServiceException("Essa sala nao pode ser removida pois existem outros registros associados a ela!");
            }
            catch (Exception e) { throw e; }

            return false;
        }

        public bool Update(SalaModel entity)
        {
            try
            {
                var x = _context.Sala.Where(s => s.Id == entity.Id).FirstOrDefault();
                if (x != null)
                {
                    _context.Update(SetEntity(entity, x));
                    return _context.SaveChanges() == 1 ? true : false;
                }
            }
            catch (Exception e) { throw new ServiceException("Houve um problema ao atualizar registro, tente novamente em alguns minutos"); }

            return false;
        }

        private static Sala SetEntity(SalaModel model, Sala entity)
        {
            entity.Id = model.Id;
            entity.Titulo = model.Titulo;
            entity.Bloco = model.BlocoId;

            return entity;
        }

        public List<SalaModel> GetSelectedList()
            => _context.Sala.Select(s => new SalaModel { Id = s.Id, Titulo = string.Format("{0} - {1}", s.Id, s.Titulo) }).ToList();

        public List<SalaModel> GetAllByIdUsuarioOrganizacao(int idUsuario)
        {
            var _blocoService = new BlocoService(_context);

            var todasSalas = GetAll();
            var blocos = _blocoService.GetAllByIdUsuarioOrganizacao(idUsuario);

            var query = (from sl in todasSalas
                         join bl in blocos on sl.BlocoId equals bl.Id
                         select new SalaModel
                         {
                             Id = sl.Id,
                             BlocoId = sl.BlocoId,
                             Titulo = sl.Titulo
                         }).ToList();

            return query;
        }
    }
}

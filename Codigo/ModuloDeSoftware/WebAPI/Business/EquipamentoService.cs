using Model;
using Model.ViewModel;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class EquipamentoService : IEquipamentoService
    {
        private readonly str_dbContext _context;

        public EquipamentoService(str_dbContext context)
        {
            _context = context;
        }

        public EquipamentoModel GetByIdEquipamento(int idEquipamento)
           => _context.Equipamento
                  .Where(eq => eq.Id == idEquipamento)
                  .Select(eq => new EquipamentoModel
                  {
                      Id = eq.Id,
                      Modelo = eq.Modelo,
                      Marca = eq.Marca,
                      Descricao = eq.Descricao,
                      Sala = eq.Sala,
                      TipoEquipamento = eq.TipoEquipamento,
                      HardwareDeSala = (int)eq.HardwareDeSala
                  }).FirstOrDefault();


        public EquipamentoModel GetByIdSalaAndTipoEquipamento(int idSala, string tipo)
       => _context.Equipamento
                   .Where(eq => eq.Sala == idSala && eq.TipoEquipamento.ToUpper().Equals(tipo.ToUpper()))
                   .Select(eq => new EquipamentoModel
                   {
                       Id = eq.Id,
                       Modelo = eq.Modelo,
                       Marca = eq.Marca,
                       Descricao = eq.Descricao,
                       Sala = eq.Sala,
                       TipoEquipamento = eq.TipoEquipamento,
                       HardwareDeSala = (int)eq.HardwareDeSala
                   }).FirstOrDefault();


        public List<EquipamentoModel> GetByIdSala(int idSala)
       => _context.Equipamento
                   .Where(eq => eq.Sala == idSala)
                   .Select(eq => new EquipamentoModel
                   {
                       Id = eq.Id,
                       Modelo = eq.Modelo,
                       Marca = eq.Marca,
                       Descricao = eq.Descricao,
                       Sala = eq.Sala,
                       TipoEquipamento = eq.TipoEquipamento,
                       HardwareDeSala = (int)eq.HardwareDeSala
                   }).ToList();

        public List<EquipamentoModel> GetAll() => _context.Equipamento.Select(e => new EquipamentoModel { Id = e.Id, Modelo = e.Modelo, Descricao = e.Descricao, TipoEquipamento = e.TipoEquipamento, Marca = e.Marca, Sala = e.Sala, HardwareDeSala = e.HardwareDeSala != null ? (int)e.HardwareDeSala : 0 }).ToList();

        public bool Insert(EquipamentoViewModel entity)
        {
            try
            {
                ICodigoInfravermelhoService codigoInfravermelhoService = new CodigoInfravermelhoService(_context);
                IMonitoramentoService monitoramentoService = new MonitoramentoService(_context);

                var equip = SetEntity(entity.EquipamentoModel);

                _context.Add(equip);
                int inserted = _context.SaveChanges();
                _context.Entry(equip).Reload();
                var codigosEntity = new List<CodigoInfravermelhoModel>();
                if (inserted == 1)
                {
                    entity.Codigos.ForEach(c => codigosEntity.Add(new CodigoInfravermelhoModel { Codigo = c.Codigo, IdEquipamento = equip.Id, IdOperacao = c.IdOperacao }));
                    codigoInfravermelhoService.AddAll(codigosEntity);

                    monitoramentoService.Insert(new MonitoramentoModel { Estado = false, EquipamentoId = equip.Id });;
                }
                return Convert.ToBoolean(inserted);
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Update(EquipamentoViewModel entity)
        {
            try
            {
                ICodigoInfravermelhoService codigoInfravermelhoService = new CodigoInfravermelhoService(_context);

                var equip = SetEntity(entity.EquipamentoModel);
                _context.Equipamento.Update(equip);
                int updated = _context.SaveChanges();
                var codigosEntity = new List<CodigoInfravermelhoModel>();
                if (updated == 1)
                {
                    entity.Codigos.ForEach(c => codigosEntity.Add(new CodigoInfravermelhoModel { Codigo = c.Codigo, IdEquipamento = equip.Id, IdOperacao = c.IdOperacao }));
                    codigoInfravermelhoService.UpdateAll(codigosEntity);
                }
                return Convert.ToBoolean(updated);


            }
            catch (Exception e)
            {

                throw e;
            }

        }

        private static Equipamento SetEntity(EquipamentoModel model)
        {
            Equipamento entity = new Equipamento
            {
                Id = model.Id,
                Descricao = model.Descricao,
                Marca = model.Marca,
                Modelo = model.Modelo,
                TipoEquipamento = model.TipoEquipamento,
                Sala = model.Sala,
                HardwareDeSala = model.HardwareDeSala,
            };
            return entity;
        }

        public bool Remove(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var equipamento = _context.Equipamento.Where(e => e.Id == id).FirstOrDefault();
                    var codigoService = new CodigoInfravermelhoService(_context);
                    var codigos = codigoService.GetAllByEquipamento(id);
                    if (codigos != null)
                        codigoService.RemoveAll(codigos);
                    if (equipamento != null)
                    {
                        _context.Equipamento.Remove(equipamento);
                        var save = _context.SaveChanges() == 1 ? true : false;
                        transaction.Commit();
                        return save;
                    }
                    else
                    {
                        throw new ServiceException("Algo deu errado, tente novamente em alguns minutos.");
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;

                }
            }
        }
    }
}

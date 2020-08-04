﻿using Model;
using Model.ViewModel;
using Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Service
{
    public class PlanejamentoService : IService<PlanejamentoModel>
    {
        private readonly STR_DBContext _context;
        public PlanejamentoService(STR_DBContext context)
        {
            _context = context;
        }

        public List<PlanejamentoModel> GetAll()
            => _context.Planejamento
                .Select(pl => new PlanejamentoModel
                {
                    Id = pl.Id,
                    DataInicio = pl.DataInicio,
                    DataFim = pl.DataFim,
                    HorarioInicio = pl.HorarioInicio,
                    HorarioFim = pl.HorarioFim,
                    DiaSemana = pl.DiaSemana,
                    Objetivo = pl.Objetivo,
                    UsuarioId = pl.Usuario,
                    SalaId = pl.Sala

                }).ToList();
        public int Id { get; set; }

        public PlanejamentoModel GetById(int id)
            => _context.Planejamento
                .Where(pl => pl.Id == id)
                .Select(pl => new PlanejamentoModel
                {
                    Id = pl.Id,
                    DataInicio = pl.DataInicio,
                    DataFim = pl.DataFim,
                    HorarioInicio = pl.HorarioInicio,
                    HorarioFim = pl.HorarioFim,
                    DiaSemana = pl.DiaSemana,
                    Objetivo = pl.Objetivo,
                    UsuarioId = pl.Usuario,
                    SalaId = pl.Sala
                }).FirstOrDefault();

        public bool Insert(PlanejamentoModel entity)
        {
            List<Planejamento> horariosEntity = new List<Planejamento>();

            using (var transcaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if ((DateTime.Compare(entity.DataFim, entity.DataInicio) > 0))
                    {
                        foreach (var horario in entity.Horarios)
                        {
                            if (TimeSpan.Compare(horario.HorarioFim, horario.HorarioInicio) != 1)
                                throw new ServiceException("Os horários possuem inconsistências, corrija e tente novamente");
                            else
                            {
                                horariosEntity.Add(new Planejamento
                                {
                                    Id = entity.Id,
                                    Objetivo = entity.Objetivo,
                                    DataInicio = entity.DataInicio,
                                    DataFim = entity.DataFim,
                                    HorarioFim = horario.HorarioFim,
                                    HorarioInicio = horario.HorarioInicio,
                                    DiaSemana = horario.DiaSemana,
                                    Usuario = entity.UsuarioId,
                                    Sala = entity.SalaId
                                });
                            }
                        }

                        foreach (var item in horariosEntity)
                            _context.Add(item);

                        var save  = _context.SaveChanges() == 1? true: false;
                        transcaction.Commit();

                        return save;
                    }
                    else
                        throw new ServiceException("Sua Datas possuem inconsistências, corrija e tente novamente.");
                }
                catch (Exception e)
                {
                    transcaction.Rollback();
                    throw e;
                }
            }

        }

        public bool Remove(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var x = _context.Planejamento.Where(th => th.Id == id).FirstOrDefault();
                    if (x != null)
                    {
                        _context.Remove(x);
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

        public bool Update(PlanejamentoModel entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if ((DateTime.Compare(entity.DataFim, entity.DataInicio) > 0 && TimeSpan.Compare(entity.HorarioFim, entity.HorarioInicio) == 1))
                    {
                        _context.Update(SetEntity(entity, new Planejamento()));
                        var save = _context.SaveChanges() == 1 ? true : false;
                        transaction.Commit();
                        return save;
                    }
                    else
                        throw new ServiceException("Sua Datas/Horarios possuem inconsistências, corrija e tente novamente");
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }

        private static Planejamento SetEntity(PlanejamentoModel model, Planejamento entity)
        {
            entity.Id = model.Id;
            entity.DataInicio = model.DataInicio;
            entity.DataFim = model.DataFim;
            entity.HorarioInicio = model.HorarioInicio;
            entity.HorarioFim = model.HorarioFim;
            entity.DiaSemana = model.DiaSemana;
            entity.Objetivo = model.Objetivo;
            entity.Sala = model.SalaId;
            entity.Usuario = model.UsuarioId;


            return entity;
        }


        public List<PlanejamentoModel> GetSelectedList()
         => _context.Planejamento.Select(s => new PlanejamentoModel { Id = s.Id, Objetivo = string.Format("{0} - {1} à {2} - ", s.Id, s.DataInicio, s.DataFim, s.DiaSemana) }).ToList();

    }
}

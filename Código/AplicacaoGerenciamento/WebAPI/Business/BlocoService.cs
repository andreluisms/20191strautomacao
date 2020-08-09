﻿using Model;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class BlocoService : IService<BlocoModel>
    {
        private readonly STR_DBContext _context;
        public BlocoService(STR_DBContext context)
        {
            _context = context;
        }
        public List<BlocoModel> GetAll() => _context.Bloco.Select(b => new BlocoModel { Id = b.Id, OrganizacaoId = b.Organizacao, Titulo = b.Titulo }).ToList();

        public BlocoModel GetById(int id) => _context.Bloco.Where(b => b.Id == id).Select(b => new BlocoModel { Id = b.Id, OrganizacaoId = b.Organizacao, Titulo = b.Titulo }).FirstOrDefault();

        public List<BlocoModel> GetByIdOrganizacao(int id) => _context.Bloco.Where(b => b.Organizacao == id).Select(b => new BlocoModel { Id = b.Id, OrganizacaoId = b.Organizacao, Titulo = b.Titulo }).ToList();

        public BlocoModel GetByTitulo(string titulo, int  idOrganizacao) => _context.Bloco.Where(b => b.Titulo.Equals(titulo) && b.Organizacao == idOrganizacao).Select(b => new BlocoModel { Id = b.Id, OrganizacaoId = b.Organizacao, Titulo = b.Titulo }).FirstOrDefault();

        public bool Insert(BlocoModel entity)
        {
            try
            {
                if (GetByTitulo(entity.Titulo,entity.OrganizacaoId) == null)
                    throw new ServiceException("Essa organização já possui um bloco com esse nome");

                _context.Add(SetEntity(entity, new Bloco()));
                return _context.SaveChanges() == 1 ? true : false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Remove(int id)
        {
            var _hardwareDeBlocoService = new HardwareDeBlocoService(_context);
            var _salaService = new SalaService(_context);
            try
            {
                if (_salaService.GetByIdBloco(id).Count == 0 && _hardwareDeBlocoService.GetByIdBloco(id).Count == 0)
                {
                    var x = _context.Bloco.Where(b => b.Id == id).FirstOrDefault();
                    if (x != null)
                    {
                        _context.Remove(x);
                        return _context.SaveChanges() == 1 ? true : false;
                    }
                }
                else throw new ServiceException("Esse Bloco não pode ser removido pois possui hardwares e salas associados a ele!");

            }
            catch (Exception e)
            {
                throw e;
            }
            

            return false;
        }

        public bool Update(BlocoModel entity)
        {
            try
            {
                var bloco = GetByTitulo(entity.Titulo,entity.OrganizacaoId);
                if (bloco != null && bloco.Id != entity.Id)
                    throw new ServiceException("Essa organização já possui um bloco com esse nome");

                var x = _context.Bloco.Where(b => b.Id == entity.Id).FirstOrDefault();
                if (x != null)
                {
                    _context.Update(SetEntity(entity, x));
                    return _context.SaveChanges() == 1 ? true : false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return false;
        }

        private static Bloco SetEntity(BlocoModel model, Bloco entity)
        {
            entity.Id = model.Id;
            entity.Organizacao = model.OrganizacaoId;
            entity.Titulo = model.Titulo;

            return entity;
        }

        public List<BlocoModel> GetSelectedList()
           => _context.Bloco.Select(s => new BlocoModel { Id = s.Id, Titulo = string.Format("{0} - {1}", s.Id, s.Titulo) }).ToList();
    }
}

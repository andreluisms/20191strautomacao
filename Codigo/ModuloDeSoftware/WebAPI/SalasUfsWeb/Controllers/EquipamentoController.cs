using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.ViewModel;
using Service;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SalasUfsWeb.Controllers
{
    public class EquipamentoController : Controller
    {
        private readonly IEquipamentoService _equipamentoService;
        private readonly ICodigoInfravermelhoService _codigoInfravermelhoService;
        private readonly ISalaService _salaService;
        private readonly IOperacaoCodigoService _operacaoService;
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioOrganizacaoService _usuarioOrganizacaoService;
        private readonly IBlocoService _blocoService;
        private readonly IOrganizacaoService _organizacaoService;
        private readonly IHardwareDeSalaService _hardwareDeSalaService;
        public EquipamentoController(
                                        IEquipamentoService equipamentoService,
                                        ICodigoInfravermelhoService codigoInfravermelhoService,
                                        ISalaService salaService,
                                        IOperacaoCodigoService operacaoService,
                                        IUsuarioService usuarioService,
                                        IUsuarioOrganizacaoService usuarioOrganizacaoService,
                                        IBlocoService blocoService,
                                        IOrganizacaoService organizacaoService,
                                        IHardwareDeSalaService hardwareDeSalaService

                                    )
        {
            _equipamentoService = equipamentoService;
            _codigoInfravermelhoService = codigoInfravermelhoService;
            _salaService = salaService;
            _operacaoService = operacaoService;
            _usuarioService = usuarioService;
            _usuarioOrganizacaoService = usuarioOrganizacaoService;
            _blocoService = blocoService;
            _organizacaoService = organizacaoService;
            _hardwareDeSalaService = hardwareDeSalaService;
        }

        // GET: EquipamentoController
        public ActionResult Index()
        {
            var equipamentosModel = _equipamentoService.GetAll();
            List<EquipamentoViewModel> equipamentos = new List<EquipamentoViewModel>();
            equipamentosModel.ForEach(e => equipamentos.Add(new EquipamentoViewModel { EquipamentoModel = e, SalaModel = _salaService.GetById(e.Sala), HardwareDeSalaModel = _hardwareDeSalaService.GetById(e.HardwareDeSala.Value) }));

            return View(equipamentos);
        }

        // GET: EquipamentoController/Details/5
        public ActionResult Details(int id)
        {
            var equipamentoModel = _equipamentoService.GetByIdEquipamento(id);
            var codigos = _codigoInfravermelhoService.GetAllByEquipamento(equipamentoModel.Id);
            var equipamentoViewModel = new EquipamentoViewModel
            {
                EquipamentoModel = equipamentoModel,
                SalaModel = _salaService.GetById(equipamentoModel.Sala)
            };


            equipamentoViewModel.BlocoModel = _blocoService.GetById(equipamentoViewModel.SalaModel.BlocoId);
            List<CodigoInfravermelhoViewModel> codigosView = new List<CodigoInfravermelhoViewModel>();
            codigos.ForEach(c => codigosView.Add(new CodigoInfravermelhoViewModel { Codigo = c.Codigo, Id = c.Id, IdEquipamento = c.IdEquipamento, IdOperacao = c.IdOperacao, Operacao = _operacaoService.GetById(c.IdOperacao).Titulo }));
            equipamentoViewModel.Codigos = codigosView;

            return View(equipamentoViewModel);
        }

        // GET: EquipamentoController/Create
        public ActionResult Create()
        {
            string[] tiposEquipamento = { EquipamentoModel.TIPO_CONDICIONADOR, EquipamentoModel.TIPO_LUZES };

            var organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);
            var blocos = _blocoService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            var salas = _salaService.GetByIdBloco(blocos.FirstOrDefault().Id);
            var hardwares = _hardwareDeSalaService.GetAtuadorNotUsed();
            var operacoes = _operacaoService.GetAll().ToList();
            ViewBag.Operacoes = operacoes;
            ViewBag.Organizacoes = organizacoes;
            ViewBag.Usuarios = _usuarioService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            ViewBag.Blocos = blocos;
            ViewBag.Salas = salas;
            ViewBag.Hardwares = hardwares;
            ViewBag.Tipos = tiposEquipamento;
            return View();
        }

        // POST: EquipamentoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EquipamentoViewModel equipamentoViewModel)
        {
            string[] tiposEquipamento = { EquipamentoModel.TIPO_CONDICIONADOR, EquipamentoModel.TIPO_LUZES };

            var organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);
            var blocos = _blocoService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            var salas = _salaService.GetByIdBloco(blocos.FirstOrDefault().Id);
            var hardwares = _hardwareDeSalaService.GetAtuadorNotUsed();
            var operacoes = _operacaoService.GetAll().ToList();
            ViewBag.Operacoes = operacoes;
            ViewBag.Organizacoes = organizacoes;
            ViewBag.Usuarios = _usuarioService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            ViewBag.Salas = salas;
            ViewBag.Blocos = blocos;
            ViewBag.Hardwares = hardwares;
            ViewBag.Tipos = tiposEquipamento;

            try
            {
                if (ModelState.IsValid)
                {

                    if (_equipamentoService.Insert(equipamentoViewModel))
                    {
                        TempData["mensagemSucesso"] = "Equipamento cadastrado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    else TempData["mensagemErro"] = "Houve um problema ao inserir novo equipamento, tente novamente em alguns minutos.";
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return View(equipamentoViewModel);
        }

        // GET: EquipamentoController/Edit/5
        public ActionResult Edit(int id)
        {

            var equipamento = _equipamentoService.GetByIdEquipamento(id);
            var sala = _salaService.GetById(equipamento.Sala);
            var bloco = _blocoService.GetById(sala.BlocoId);
            var codigos = _codigoInfravermelhoService.GetAllByEquipamento(equipamento.Id);

            List<CodigoInfravermelhoViewModel> codigosView = new List<CodigoInfravermelhoViewModel>();
            codigos.ForEach(c => codigosView.Add(new CodigoInfravermelhoViewModel { Codigo = c.Codigo, Id = c.Id, IdEquipamento = c.IdEquipamento, IdOperacao = c.IdOperacao, Operacao = _operacaoService.GetById(c.IdOperacao).Titulo }));

            EquipamentoViewModel equipamentoViewModel = new EquipamentoViewModel
            {
                EquipamentoModel = equipamento,
                SalaModel = _salaService.GetById(equipamento.Sala),
                BlocoModel = bloco,
                OrganizacaoModel = _organizacaoService.GetById(bloco.OrganizacaoId),
                Codigos = codigosView,

                HardwareDeSalaModel = _hardwareDeSalaService.GetById(equipamento.HardwareDeSala.Value)
            };

            string[] tiposEquipamento = { EquipamentoModel.TIPO_CONDICIONADOR, EquipamentoModel.TIPO_LUZES };

            var organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);
            var blocos = _blocoService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            var operacoes = _operacaoService.GetAll().ToList();
            var hardwares = _hardwareDeSalaService.GetAtuadorNotUsed();

            ViewBag.Operacoes = operacoes;
            ViewBag.Organizacoes = organizacoes;
            ViewBag.Usuarios = _usuarioService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            ViewBag.Salas = _salaService.GetByIdBloco(blocos.FirstOrDefault().Id);
            ViewBag.Blocos = blocos;
            ViewBag.Tipos = tiposEquipamento;
            ViewBag.Hardwares = hardwares;

            return View(equipamentoViewModel);
        }

        // POST: EquipamentoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EquipamentoViewModel equipamentoViewModel)
        {
            string[] tiposEquipamento = { EquipamentoModel.TIPO_CONDICIONADOR, EquipamentoModel.TIPO_LUZES };

            var organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);
            var blocos = _blocoService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            var operacoes = _operacaoService.GetAll().ToList();
            var hardwares = _hardwareDeSalaService.GetAtuadorNotUsed();
            ViewBag.Operacoes = operacoes;
            ViewBag.Organizacoes = organizacoes;
            ViewBag.Usuarios = _usuarioService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            ViewBag.Salas = _salaService.GetByIdBloco(blocos.FirstOrDefault().Id);
            ViewBag.Blocos = blocos;
            ViewBag.Tipos = tiposEquipamento;
            ViewBag.Hardwares = hardwares;

            try
            {
                if (ModelState.IsValid)
                {

                    if (_equipamentoService.Update(equipamentoViewModel))
                    {
                        TempData["mensagemSucesso"] = "Equipamento atualizado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    else TempData["mensagemErro"] = "Houve um problema ao atualizar o equipamento, tente novamente em alguns minutos.";
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return View(equipamentoViewModel);
        }

        // GET: EquipamentoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EquipamentoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                if (_equipamentoService.Remove(id))
                    TempData["mensagemSucesso"] = "Equipamento removido com sucesso!";
                else
                    TempData["mensagemErro"] = "Houve um problema ou remover o Equipamento, tente novamente em alguns minutos";

            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

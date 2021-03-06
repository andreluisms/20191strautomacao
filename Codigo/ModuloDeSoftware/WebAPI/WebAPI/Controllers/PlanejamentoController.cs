﻿using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Service.Interface;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanejamentoController : ControllerBase
    {
        private readonly IPlanejamentoService _service;
        public PlanejamentoController(IPlanejamentoService service)
        {
            _service = service;
        }
        // GET: api/Planejamento
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                var planejamentos = _service.GetAll();
                if (planejamentos.Count == 0)
                    return NoContent();

                return Ok(planejamentos);
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // GET: api/Planejamento/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            try
            {
                var planejamento = _service.GetById(id);
                if (planejamento == null)
                    return NotFound("Planejamento não encontrado na base de dados");

                return Ok(planejamento);
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // POST: api/Planejamento
        [HttpPost]
        public ActionResult Post([FromBody] PlanejamentoModel planejamentoModel)
        {
            try
            {
                if (_service.Insert(planejamentoModel))
                    return Ok();

                return BadRequest();
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // PUT: api/Planejamento/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] PlanejamentoModel planejamentoModel)
        {
            try
            {
                if (_service.Update(planejamentoModel))
                    return Ok();

                return BadRequest();
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id, bool excluirReservas)
        {
            try
            {
                if (_service.Remove(id, excluirReservas))
                    return Ok();

                return BadRequest();
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
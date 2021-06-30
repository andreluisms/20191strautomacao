using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Service.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitoramentoController : ControllerBase
    {

        private readonly IMonitoramentoService _service;
        public MonitoramentoController(IMonitoramentoService service)
        {
            _service = service;
        }

        // GET: api/Monitoramento/5
        [HttpGet("{idSala}")]
        [Route("ObterPorSala")]
        public ActionResult Get(int idEquipamento)
        {
            try
            {
                var monitoramento = _service.GetByIdEquipamento(idEquipamento);
                if (monitoramento == null)
                    return NoContent();

                return Ok(monitoramento);
            }
            catch (ServiceException e)
            {
                return StatusCode(500, e.Message);
            }

        }

        // PUT: api/Monitoramento/5
        [HttpPut]
        public ActionResult Atualizar([FromBody] MonitoramentoModel monitoramento)
        {
            try
            {
                if (_service.Update(monitoramento))
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

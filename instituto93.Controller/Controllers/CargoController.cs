using instituto93.Application;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Domain.Models;

namespace instituto93.Controller.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CargosController : ControllerBase
    {
        private readonly ICargosService _service; // <- usar la interfaz

        public CargosController(ICargosService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<CargosModelo>>> Get(CancellationToken cancellationToken)
        {
            try
            {
                var cargos = await _service.GetCargos(cancellationToken);
                return Ok(cargos);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CargosModelo>> GetById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var cargo = await _service.GetByIdCargos(id, cancellationToken);
                return Ok(cargo);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<int>> CreateCargo(CargosModelo cargo, CancellationToken cancellationToken)
        {
            try
            {
                var cargoId = await _service.CreateCargo(cargo, cancellationToken);
                return Ok(cargoId);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<int>> UpdateCargo(CargosModelo cargo, CancellationToken cancellationToken)
        {
            try
            {
                var cargoId = await _service.UpdateCargo(cargo, cancellationToken);
                return Ok(cargoId);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DelateCargo(int id, CancellationToken cancellationToken)
        {
            try
            {
                var cargoId = await _service.DeleteCargo(id, cancellationToken);
                return Ok(cargoId);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
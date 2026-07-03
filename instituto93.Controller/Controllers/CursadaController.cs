using Microsoft.AspNetCore.Mvc;
using instituto93.Domain.Models;
using instituto93.Application.Interfaces;
using System.Threading.Tasks;

namespace instituto93.Controller.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursadaController : ControllerBase
    {
        private readonly ICursadaService _service;
        public CursadaController(ICursadaService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _service.GetByIdAsync(id);
            return res == null ? NotFound() : Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cursada model)
        {
            await _service.AddAsync(model);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Cursada model)
        {
            await _service.UpdateAsync(model);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
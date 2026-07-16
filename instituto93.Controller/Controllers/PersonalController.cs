using instituto93.Application;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Domain.Models;
using instituto93.Application.Interfaces;

namespace instituto93.Controller.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonalController : ControllerBase
    {
        private readonly IPersonalService _service;

        public PersonalController(IPersonalService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Personal>>> Get(CancellationToken cancellationToken)
        {
            try
            {
                var personal = await _service.GetPersonalAsync(cancellationToken);
                return Ok(personal);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

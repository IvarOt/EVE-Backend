using eve_backend.logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eve_backend.api.Controllers
{
    [Route("api/excel/{Excelid}/object/")]
    [ApiController]
    public class ObjectController : ControllerBase
    {
        private readonly IObjectService _objectService ;
        public ObjectController(IObjectService objectService) 
        {
            _objectService = objectService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Put()
        {
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            return Ok();
        }

    }
}

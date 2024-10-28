using eve_backend.logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eve_backend.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObjectController : ControllerBase
    {
        private readonly IObjectService _objectService ;
        public ObjectController(IObjectService objectService) 
        {
            _objectService = objectService;
        }
        [HttpGet("{ExcelId}")]
        public async Task<IActionResult> Get(int ExcelId)
        {
            var objects = await _objectService.GetObjects(ExcelId);
            return Ok(objects);
        }
        [HttpPost("{ExcelId}")]
        public async Task<IActionResult> Post(int ExcelId)
        {
            await _objectService.CreateObject(ExcelId);
            return Ok();
        }
        [HttpDelete("{ObjectId}")]
        public async Task<IActionResult> Delete(int ObjectId)
        {
            await _objectService.DeleteObject(ObjectId);
            return Ok();
        }
    }
}

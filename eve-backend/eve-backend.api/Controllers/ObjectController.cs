using eve_backend.logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eve_backend.api.Controllers
{
    [Route("api/Excel/{ExcelId}/Object/")]
    [ApiController]
    public class ObjectController : ControllerBase
    {
        private readonly IObjectService _objectService ;
        public ObjectController(IObjectService objectService) 
        {
            _objectService = objectService;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromRoute] int ExcelId)
        {
            var objects = await _objectService.GetObjects(ExcelId);
            return Ok(objects);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromRoute] int ExcelId)
        {
            await _objectService.CreateObject(ExcelId);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int objectId)
        {
            await _objectService.DeleteObject(objectId);
            return Ok();
        }
    }
}

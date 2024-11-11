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
        public async Task<IActionResult> GetAllObjects(int page, int pagesize, bool isDescending, int ExcelId)
        {
            var objects = await _objectService.GetObjects(page, pagesize, isDescending, ExcelId);
            return Ok(objects);
        }

        [HttpGet("{ExcelId}/Single")]
        public async Task<IActionResult> Get(int page, int ExcelId)
        {
            var objects = await _objectService.GetObject(page, ExcelId);
            return Ok(objects);
        }

        [HttpGet("{ExcelId}/Count")]
        public async Task<IActionResult> Get(int ExcelId)
        {
            var count = await _objectService.GetCount(ExcelId);
            return Ok(count);
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

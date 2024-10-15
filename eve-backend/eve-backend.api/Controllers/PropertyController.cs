using eve_backend.logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace eve_backend.api.Controllers
{
    [Route("api/Excel/{ExcelId}/Object/{ObjectId}/Property/")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromRoute] int ObjectId)
        {
            var properties = await _propertyService.GetProperties(ObjectId);
            return Ok(properties);
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromRoute] int ObjectId, int PropertyId, string Value)
        {
            await _propertyService.UpdateProperty(ObjectId, PropertyId, Value);
            return Ok();
        }
    }
}

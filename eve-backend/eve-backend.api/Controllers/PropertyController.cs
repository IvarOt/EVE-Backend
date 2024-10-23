using eve_backend.logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace eve_backend.api.Controllers
{
    [Route("api/Property")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }
        [HttpGet("{ObjectId}")]
        public async Task<IActionResult> Get(int ObjectId)
        {
            var properties = await _propertyService.GetProperties(ObjectId);
            return Ok(properties);
        }
        [HttpPut]
        public async Task<IActionResult> Put(int ObjectId, int PropertyId, string Value)
        {
            await _propertyService.UpdateProperty(ObjectId, PropertyId, Value);
            return Ok();
        }
    }
}

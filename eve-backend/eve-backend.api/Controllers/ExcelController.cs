using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using eve_backend.logic.Interfaces;
using System.Text.Json;
using Newtonsoft.Json.Linq;
namespace eve_backend.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private readonly IExcelService _excelService;

        public ExcelController(IExcelService excelService)
        {
            _excelService = excelService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            await _excelService.UploadExcel(file);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _excelService.DeleteExcel(id);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put(int id, string fileName)
        {
            await _excelService.UpdateExcel(id, fileName);
            return Ok();
        }

    }
}

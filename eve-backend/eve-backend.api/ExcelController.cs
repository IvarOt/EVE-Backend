using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using eve_backend.logic.Interfaces;
namespace eve_backend.api
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
            string strin2g = await _excelService.UploadExcel(file);
            

            return Ok(strin2g);
        }
    }
}

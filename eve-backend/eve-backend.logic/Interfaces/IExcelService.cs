using eve_backend.logic.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eve_backend.logic.Interfaces
{
    public interface IExcelService
    {
        Task<String> UploadExcel(IFormFile excelFile);
    }
}

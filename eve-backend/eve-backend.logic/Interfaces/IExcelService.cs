using eve_backend.logic.DTO;
using eve_backend.logic.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eve_backend.logic.Interfaces
{
    public interface IExcelService
    {
        Task UploadExcel(IFormFile excelFile);

        Task DeleteExcel(int id);

        Task UpdateExcel(int id, string fileName);
        Task<List<ExcelFile>> GetExcelFiles(int page, int pageSize, bool sortByDate, bool isDescending, string searchTerm);
        Task<int> GetCount();
    }
}

using eve_backend.logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eve_backend.logic.Interfaces
{
    public interface IExcelRepository
    {
        Task SaveExcelFile(ExcelFile file);

        Task DeleteExcelFile(int id);

        Task UpdateExcelFile(int id, string fileName, DateTime LastUpdated);
        Task<List<ExcelFile>> GetExcelFiles(int page, int pagesize, bool isDescending, string searchTerm);
        Task<List<ExcelFile>> GetExcelFilesAZ(int page, int pagesize, bool isDescending, string searchTerm);
        Task<ObjectStructure> GetFileObjectStructure(int fileId);
        Task<ExcelFile> GetExcelFile(int id);
        Task<int> GetCount();

    }
}

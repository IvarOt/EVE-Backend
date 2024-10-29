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

        Task<List<ExcelFile>> GetExcelFiles();
        Task<ObjectStructure> GetFileObjectStructure(int fileId);
    }
}

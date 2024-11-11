using eve_backend.logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eve_backend.logic.Interfaces
{
    public interface IObjectRepository
    {
        Task<List<ExcelObject>> GetObjects(int page, int pagesize, bool isDescending, int excelId);
        Task<ExcelObject> GetObject(int page, int excelId);
        Task<int> GetCount(int excelId);
        Task CreateObject(ExcelObject excelObject);
        Task UpdateObject(int objectId, DateTime dateTime);
        Task DeleteObject(int objectId);
    }
}

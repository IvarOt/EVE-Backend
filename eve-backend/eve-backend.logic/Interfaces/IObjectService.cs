using eve_backend.logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eve_backend.logic.Interfaces
{
    public interface IObjectService
    {
        Task<List<ExcelObject>> GetObjects(int page, int pagesize, bool isDescending, int excelId);
        Task CreateObject(int fileId);
        Task DeleteObject(int objectId);
        Task UpdateObject(int objectId);
    }
}

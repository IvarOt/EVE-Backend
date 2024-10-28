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
        Task<List<ExcelObject>> GetObjects(int id);
        Task CreateObject(ExcelObject excelObject);
        Task UpdateObject(int objectId, DateTime dateTime);
        Task DeleteObject(int objectId);
    }
}

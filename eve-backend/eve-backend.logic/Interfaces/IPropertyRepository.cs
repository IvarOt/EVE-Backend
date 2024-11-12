using eve_backend.logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eve_backend.logic.Interfaces
{
    public interface IPropertyRepository
    {
        Task<List<ExcelProperty>> GetProperties(int objectId);

        Task UpdateProperty(Guid propertyId, string value);
    }
}

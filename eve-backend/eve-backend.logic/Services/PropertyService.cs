using eve_backend.logic.Interfaces;
using eve_backend.logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eve_backend.logic.Services
{
    public class PropertyService: IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        public PropertyService (IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<List<ExcelProperty>> GetProperties(int objectId)
        {
            var properties = await _propertyRepository.GetProperties(objectId);
            return properties;
        }

        public async Task UpdateProperty(int propertyId, string value)
        {
            await _propertyRepository.UpdateProperty(propertyId, value);
        }
    }
}

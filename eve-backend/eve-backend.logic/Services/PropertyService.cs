using eve_backend.logic.Interfaces;
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
    }
}

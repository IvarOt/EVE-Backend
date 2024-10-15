using eve_backend.logic.Interfaces;
using eve_backend.logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eve_backend.logic.Services
{
    public class ObjectService : IObjectService
    {
        readonly IObjectRepository _objectRepository;
        public ObjectService(IObjectRepository objectRepository)
        {
            _objectRepository = objectRepository;
        }
        public async Task<List<ExcelObject>> GetObjects(int id)
        {
            var objects = await _objectRepository.GetObjects(id);
            return objects;
        }

    }
}

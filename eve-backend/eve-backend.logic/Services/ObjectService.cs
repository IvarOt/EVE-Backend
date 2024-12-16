using eve_backend.logic.DTO;
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
        private readonly IPropertyRepository _propertyRepository;
        private readonly IObjectRepository _objectRepository;
        private readonly IExcelRepository _excelRepository;
        public ObjectService(IObjectRepository objectRepository, IExcelRepository excelRepository, IPropertyRepository propertyRepository)
        {
            _objectRepository = objectRepository;
            _excelRepository = excelRepository;
            _propertyRepository = propertyRepository;
        }

        public async Task<ResponseGetAllObjects> GetObjects(int page, int pagesize, bool isDescending, int excelId)
        {
            var objects = await _objectRepository.GetObjects(page, pagesize, isDescending, excelId);
            ExcelFile excelfile = await _excelRepository.GetExcelFile(excelId);
            ResponseGetAllObjects response = new ResponseGetAllObjects();
            response.objects = objects;
            response.ObjectIdentifier = excelfile.ObjectIdentifier;

            return response;
        }

        public async Task<ExcelObject> GetObject(int page, int excelId)
        {
            var result = await _objectRepository.GetObject(page, excelId);
            result.ExcelProperties = await _propertyRepository.GetProperties(result.Id);
            return result;
        }

        public async Task<int> GetCount(int excelId)
        {
            var result = await _objectRepository.GetCount(excelId);
            return result;
        }

        public async Task UpdateObject(int objectId)
        {
            await _objectRepository.UpdateObject(objectId, DateTime.Now);
        }

        public async Task DeleteObject(int objectId)
        {
            await _objectRepository.DeleteObject(objectId);
        }
        public async Task CreateObject(int fileId)
        {
            List<string> headers = await _excelRepository.GetFileObjectStructure(fileId);
            ExcelObject newObject = new ExcelObject();
            foreach (var item in headers)
            {
                newObject.ExcelProperties.Add(new ExcelProperty { Name = item, Value = "" });
            }
            newObject.LastUpdated = DateTime.Now;
            newObject.ExcelFileId = fileId;
            await _objectRepository.CreateObject(newObject);
        }

    }
}

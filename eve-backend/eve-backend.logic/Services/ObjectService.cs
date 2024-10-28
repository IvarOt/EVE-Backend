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
        private readonly IObjectRepository _objectRepository;
        private readonly IExcelRepository _excelRepository;
        public ObjectService(IObjectRepository objectRepository, IExcelRepository excelRepository)
        {
            _objectRepository = objectRepository;
            _excelRepository = excelRepository;
        }

        public async Task<List<ExcelObject>> GetObjects(int excelId)
        {
            var objects = await _objectRepository.GetObjects(excelId);
            return objects;
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
            ObjectStructure structure = await _excelRepository.GetFileObjectStructure(fileId);
            ExcelObject newObject = new ExcelObject();
            foreach (var item in structure.Headers)
            {
                newObject.ExcelProperties.Add(new ExcelProperty { Name = item, Value = "" });
            }
            newObject.LastUpdated = DateTime.Now;
            newObject.ExcelFileId = fileId;
            await _objectRepository.CreateObject(newObject);
        }

    }
}

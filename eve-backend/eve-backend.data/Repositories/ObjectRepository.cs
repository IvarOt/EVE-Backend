using eve_backend.logic.Interfaces;
using eve_backend.logic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eve_backend.data.Repositories
{
    public class ObjectRepository : IObjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ObjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<ExcelObject>> GetObjects(int excelId)
        {
            var objects = await _context.ExcelObjects.Include(x => x.ExcelProperties).Where(x => x.ExcelFileId == excelId).ToListAsync();
            return objects;
        }

        public async Task<ExcelObject> GetFirstObject(int fileId)
        {
            var firstObject = await _context.ExcelObjects.Include(x => x.ExcelProperties).Where(x => x.ExcelFileId == fileId).FirstOrDefaultAsync();
            if (firstObject == null)
            {
                throw new FileNotFoundException();
            }
            else
            {
                return firstObject;
            }
        }

        public async Task UpdateObject(int objectId, DateTime dateTime)
        {
            var objectToUpdate = await _context.ExcelObjects.Where(x => x.Id == objectId).FirstOrDefaultAsync();
            if (objectToUpdate == null)
            {
                throw new FileNotFoundException();
            }
            else
            {
                objectToUpdate.LastUpdated = dateTime;
                await _context.SaveChangesAsync();
            }
        }
        public async Task CreateObject(ExcelObject excelObject)
        {
            await _context.ExcelObjects.AddAsync(excelObject);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteObject(int objectId)
        {
            var objectToDelete = await _context.ExcelObjects.Where(x => x.Id == objectId).FirstOrDefaultAsync();
            if (objectToDelete == null)
            {
                throw new FileNotFoundException();
            }
            else
            {
                _context.Remove(objectToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}

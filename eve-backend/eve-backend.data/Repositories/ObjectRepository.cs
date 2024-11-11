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
        public async Task<List<ExcelObject>> GetObjects(int page, int pagesize, bool isDescending, int excelId)
        {
            page = pagesize * page;
            var result =  isDescending
                ? await _context.ExcelObjects.Where(x => x.ExcelFileId == excelId).OrderByDescending(x => x.LastUpdated).Skip(page).Take(pagesize).ToListAsync()
                : await _context.ExcelObjects.Where(x => x.ExcelFileId == excelId).OrderBy(x => x.LastUpdated).Skip(page).Take(pagesize).ToListAsync();


            if (result == null)
            {
                throw new FileNotFoundException();
            }
            return result;

        }

        public async Task<ExcelObject> GetObject(int page, int excelId)
        {
            page = page--;
            var result = await _context.ExcelObjects.Where(x => x.ExcelFileId == excelId).Skip(page).Take(1).FirstOrDefaultAsync();
            if (result == null)
            {
                throw new FileNotFoundException();
            }
            return result;
        }
        public async Task<int> GetCount(int excelId)
        {
            var result = await _context.ExcelObjects.Where(x => x.ExcelFileId == excelId).CountAsync();
            return result;
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

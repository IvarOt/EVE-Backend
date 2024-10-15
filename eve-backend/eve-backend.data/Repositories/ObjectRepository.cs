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
    public class ObjectRepository: IObjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ObjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<ExcelObject>> GetObjects(int id)
        {
            var objects = await _context.ExcelObjects.Include(x => x.ExcelProperties).Where(x => x.Id == id).ToListAsync();
            return objects;
        }

    }
}

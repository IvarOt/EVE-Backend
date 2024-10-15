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
    public class PropertyRepository : IPropertyRepository
    {
        private readonly ApplicationDbContext _context;
        public PropertyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExcelProperty>> GetProperties(int objectId)
        {
            var properties = await _context.ExcelProperties.Where(x => x.ExcelObjectId == objectId).ToListAsync();
            return properties;
        }
    }
}

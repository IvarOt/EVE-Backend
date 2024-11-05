using Azure;
using eve_backend.logic.Interfaces;
using eve_backend.logic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Drawing.Printing;

namespace eve_backend.data.Repositories
{
    public class ExcelRepository : IExcelRepository
    {
        private readonly ApplicationDbContext _context;
        public ExcelRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<List<ExcelFile>> GetExcelFiles(int page, int pagesize, bool isDescending, string searchTerm)
        {
            if (searchTerm.IsNullOrEmpty())
            {
                searchTerm = "";
            }
            page = page * pagesize;
            return isDescending
            ? await _context.ExcelFiles.OrderByDescending(b => b.LastUpdated).Where(b => b.Name.Contains(searchTerm)).Skip(page).Take(pagesize).ToListAsync()
            : await _context.ExcelFiles.OrderBy(b => b.LastUpdated).Where(b => b.Name.Contains(searchTerm)).Skip(page).Take(pagesize).ToListAsync();
        }

        public async Task<List<ExcelFile>> GetExcelFilesAZ(int page, int pagesize, bool isDescending, string searchTerm)
        {
            if (searchTerm.IsNullOrEmpty())
            {
                searchTerm = "";
            }
            page = page * pagesize;
            return isDescending
            ? await _context.ExcelFiles.OrderByDescending(b => b.Name).Where(b => b.Name.Contains(searchTerm)).Skip(page).Take(pagesize).ToListAsync()
            : await _context.ExcelFiles.OrderBy(b => b.Name).Where(b => b.Name.Contains(searchTerm)).Skip(page).Take(pagesize).ToListAsync();
        }

        public async Task SaveExcelFile(ExcelFile file)
        {
            await _context.ExcelFiles.AddAsync(file);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteExcelFile(int id)
        {
            var file = await _context.ExcelFiles.FindAsync(id);
            if (file == null)
            {
                throw new FileNotFoundException();
            }
            else
            {
                _context.ExcelFiles.Remove(file);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateExcelFile(int id, string fileName, DateTime lastUpdated)
        {
            var file = await _context.ExcelFiles.FindAsync(id);
            if (file == null)
            {
                throw new FileNotFoundException();
            }
            else
            {
                file.Name = fileName;
                file.LastUpdated = lastUpdated;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ObjectStructure> GetFileObjectStructure(int fileId)
        {
            ObjectStructure structure = await _context.ExcelFiles.Where(x => x.Id == fileId).Select(x => x.Structure).FirstOrDefaultAsync();
            if (structure == null)
            {
                throw new FileNotFoundException();
            }
            return structure;
        }
    }
}

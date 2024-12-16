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
            var result =  isDescending
            ? await _context.ExcelFiles.OrderByDescending(b => b.LastUpdated).Where(b => b.Name.Contains(searchTerm)).Skip(page).Take(pagesize).ToListAsync()
            : await _context.ExcelFiles.OrderBy(b => b.LastUpdated).Where(b => b.Name.Contains(searchTerm)).Skip(page).Take(pagesize).ToListAsync();

            if (result == null)
            {
                throw new FileNotFoundException();
            }
            return result;
        }

        public async Task<List<ExcelFile>> GetExcelFilesAZ(int page, int pagesize, bool isDescending, string searchTerm)
        {
            if (searchTerm.IsNullOrEmpty())
            {
                searchTerm = "";
            }
            page = page * pagesize;
            var result = isDescending
            ? await _context.ExcelFiles.OrderByDescending(b => b.Name).Where(b => b.Name.Contains(searchTerm)).Skip(page).Take(pagesize).ToListAsync()
            : await _context.ExcelFiles.OrderBy(b => b.Name).Where(b => b.Name.Contains(searchTerm)).Skip(page).Take(pagesize).ToListAsync();

            if (result == null)
            {
                throw new FileNotFoundException();
            }
            return result;
        }

        public async Task<int> GetCount()
        {
            var result = await _context.ExcelFiles.CountAsync();
            return result;
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

        public async Task<List<string>> GetFileObjectStructure(int fileId)
        {
            var headers = await _context.ExcelFiles.Where(x => x.Id == fileId).Select(x => x.Headers).FirstOrDefaultAsync();
            if (headers == null)
            {
                throw new FileNotFoundException();
            }
            return headers;
        }

        public async Task<ExcelFile> GetExcelFile(int id)
        {
            var file = await _context.ExcelFiles.Include(x => x.excelObjects).ThenInclude(x => x.ExcelProperties).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null)
            {
                throw new FileNotFoundException();
            }
            return file;
        }

        public async Task UpdateObjectIdentifier(int id, string objectIdentifier)
        {
            var file = await _context.ExcelFiles.Where(x => x.Id == id).Include(x => x.excelObjects).ThenInclude(x => x.ExcelProperties).FirstOrDefaultAsync();
            if (file == null)
            {
                throw new FileNotFoundException();
            }
            else
            {
                file.ObjectIdentifier = objectIdentifier;
                foreach (var obj in file.excelObjects)
                {
                    var prop = obj.ExcelProperties.Where(x => x.Name == objectIdentifier).FirstOrDefault();
                    if (prop != null)
                    {
                        obj.Identifier = prop.Value;
                    }
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}

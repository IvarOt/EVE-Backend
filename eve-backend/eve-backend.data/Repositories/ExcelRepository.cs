using eve_backend.logic.Interfaces;
using eve_backend.logic.Models;

namespace eve_backend.data.Repositories
{
    public class ExcelRepository : IExcelRepository
    {
        private readonly ApplicationDbContext _context;
        public ExcelRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<List<ExcelFile>> GetExcelFiles()
        {
            var files = _context.ExcelFiles.ToList();
            return files;
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
    }
}

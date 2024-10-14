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

        public async Task SaveExcelFile(ExcelFile file)
        {
            await _context.ExcelFiles.AddAsync(file);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteExcelFile(int id)
        {
            var file = await _context.ExcelFiles.FindAsync(id);
            _context.ExcelFiles.Remove(file);
            await _context.SaveChangesAsync();
        }
    }
}

using eve_backend.logic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace eve_backend.data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ExcelObject> ExcelObjects { get; set; }
        public DbSet<ExcelFile> ExcelFiles { get; set; }
        public DbSet<ExcelProperty> ExcelProperties { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExcelFile>()
                .HasMany(ef => ef.excelObjects)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExcelObject>()
                .HasMany(eo => eo.ExcelProperties)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

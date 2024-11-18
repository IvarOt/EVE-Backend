using eve_backend.data;
using eve_backend.data.Repositories;
using eve_backend.logic.Interfaces;
using eve_backend.logic.Models;
using eve_backend.logic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVE_Backend.test
{
    [TestClass]
    public class Test_UploadExcel
    {
        private ServiceProvider _serviceProvider;
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;

        [TestInitialize]
        public void Setup()
        {
            var serviceCollection = new ServiceCollection();
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
          .UseInMemoryDatabase("EVE-DB")
            .Options;
            serviceCollection.AddScoped<IExcelRepository, ExcelRepository>();
            serviceCollection.AddScoped<IExcelService, ExcelService>();
            serviceCollection.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("EVE-DB"));

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
        public IFormFile ConvertExcelToIFormFile(string filePath)
        {
            var fileExtension = Path.GetExtension(filePath).ToLower();
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            var formFile = new FormFile(fileStream, 0, fileStream.Length, "excelFile", Path.GetFileName(filePath))
            {
                Headers = new HeaderDictionary(),
                ContentType = fileExtension == ".xlsx"
                    ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    : "application/vnd.ms-excel"
            };

            return formFile;
        }
        [TestCleanup]
        public void Cleanup()
        {
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
            }
        }
        [TestMethod]
        public async void Test_UploadExcel_HappyFlow()
        {
            var excelService = _serviceProvider.GetService<IExcelService>();
            IFormFile file = ConvertExcelToIFormFile("testExileFolder\\basic.xlsx");
            excelService.UploadExcel(file);
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                ExcelFile excelFile = await context.ExcelFiles.Include(x => x.excelObjects).ThenInclude(x => x.ExcelProperties).FirstOrDefaultAsync();
                Assert.AreEqual("Nome prodotto", excelFile.Headers.First());
            }
        }
    }
}

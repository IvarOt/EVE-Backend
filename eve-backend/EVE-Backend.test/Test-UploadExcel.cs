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
using System.ComponentModel.DataAnnotations;
using System.Drawing;
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
        public async Task Test_UploadExcel_HappyFlow()
        {
            var excelService = _serviceProvider.GetService<IExcelService>();
            IFormFile file = ConvertExcelToIFormFile(".\\testExileFolder\\basic.xlsx");
            await excelService.UploadExcel(file);
            List<string> Headers = new List<string>()
            {
                "Nome prodotto",
                "Brand",
                "EAN13",
                "Minsan",
                "Descrizione",
                "Keyword (x, y, z, …)"
            };
            List<string> Values = new List<string>()
            {
                "gert",
                "pepsi",
                "12345",
                "hello",
                "dit is mooi",
                "pepsi"

            };

            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                ExcelFile excelFile = context.ExcelFiles.Include(x => x.excelObjects).ThenInclude(x => x.ExcelProperties).FirstOrDefault();
                for(var i = 0; excelFile.excelObjects[0].ExcelProperties.Count > i; i++)
                {
                    Assert.AreEqual(Values[i] ,excelFile.excelObjects[0].ExcelProperties[i].Value);
                    Assert.AreEqual(Headers[i], excelFile.excelObjects[0].ExcelProperties[i].Name);
                }
            }
        }
        [TestMethod]
        public async Task Test_UploadExcel_OnlyHeaders()
        {
            var excelService = _serviceProvider.GetService<IExcelService>();
            IFormFile file = ConvertExcelToIFormFile(".\\testExileFolder\\basic(only header).xlsx");
            await excelService.UploadExcel(file);
            List<string> Headers = new List<string>()
            {
                "Nome prodotto",
                "Brand",
                "EAN13",
                "Minsan",
                "Descrizione",
                "Keyword (x, y, z, …)"
            };
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                ExcelFile excelFile = context.ExcelFiles.FirstOrDefault();
                CollectionAssert.AreEqual(Headers, excelFile.Headers);
            }
        }
        [TestMethod]
        public async Task Test_UploadExcel_MoreInputThenHeaders()
        {
            var excelService = _serviceProvider.GetService<IExcelService>();
            IFormFile file = ConvertExcelToIFormFile(".\\testExileFolder\\basic(meer tekst dan headers).xlsx");
            await Assert.ThrowsExceptionAsync<ApplicationException>( () =>  excelService.UploadExcel(file));
        }
        [TestMethod]
        public async Task Test_UploadExcel_Intermediate()
        {
            var excelService = _serviceProvider.GetService<IExcelService>();
            IFormFile file = ConvertExcelToIFormFile(".\\testExileFolder\\intermediate.xlsx");
            await Assert.ThrowsExceptionAsync<ApplicationException>(() => excelService.UploadExcel(file));
        }
    }
}
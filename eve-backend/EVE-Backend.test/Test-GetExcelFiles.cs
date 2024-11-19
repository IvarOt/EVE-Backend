using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eve_backend.data.Repositories;
using eve_backend.data;
using eve_backend.logic.Interfaces;
using eve_backend.logic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using eve_backend.logic.Models;

namespace EVE_Backend.test
{
    [TestClass]
    public class Test_GetExcelFiles
    {
        private ServiceProvider _serviceProvider;
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;

        [TestInitialize]
        public async Task Setup()
        {
            var serviceCollection = new ServiceCollection();
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
          .UseInMemoryDatabase("EVE-DB")
            .Options;
            serviceCollection.AddScoped<IExcelRepository, ExcelRepository>();
            serviceCollection.AddScoped<IExcelService, ExcelService>();
            serviceCollection.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("EVE-DB"));

            _serviceProvider = serviceCollection.BuildServiceProvider();
            var excelService = _serviceProvider.GetService<IExcelService>();
            IFormFile file = ConvertExcelToIFormFile(".\\testExileFolder\\zzzz.xlsx");
            IFormFile file2 = ConvertExcelToIFormFile(".\\testExileFolder\\aaaa.xlsx");
            await excelService.UploadExcel(file);
            await excelService.UploadExcel(file2);

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
        public async Task Test_GetExcelFiles_HappyFlow()
        {
            var excelService = _serviceProvider.GetService<IExcelService>();

            List<ExcelFile> excelFiles = await excelService.GetExcelFiles(0, 2, false, true, "");
            Assert.AreEqual("zzzz.xlsx", excelFiles[0].Name);
            Assert.AreEqual("aaaa.xlsx", excelFiles[1].Name);
            Assert.AreEqual(2, excelFiles.Count);
        }
        [TestMethod]
        public async Task Test_GetExcelFiles_HappyFlowAscending()
        {
            var excelService = _serviceProvider.GetService<IExcelService>();

            List<ExcelFile> excelFiles = await excelService.GetExcelFiles(0, 2, false, false, "");
            Assert.AreEqual("aaaa.xlsx", excelFiles[0].Name);
            Assert.AreEqual("zzzz.xlsx", excelFiles[1].Name);
            Assert.AreEqual(2, excelFiles.Count);
        }
        [TestMethod]
        public async Task Test_GetExcelFiles_HappyFlowAscendingTimeUploaded()
        {
            var excelService = _serviceProvider.GetService<IExcelService>();

            List<ExcelFile> excelFiles = await excelService.GetExcelFiles(0, 2, true, false, "");
            Assert.AreEqual("zzzz.xlsx", excelFiles[0].Name);
            Assert.AreEqual("aaaa.xlsx", excelFiles[1].Name);
            Assert.AreEqual(2, excelFiles.Count);
        }
        [TestMethod]
        public async Task Test_GetExcelFiles_HappyFlowTimeUploaded()
        {
            var excelService = _serviceProvider.GetService<IExcelService>();

            List<ExcelFile> excelFiles = await excelService.GetExcelFiles(0, 2, true, true, "");
            Assert.AreEqual("aaaa.xlsx", excelFiles[0].Name);
            Assert.AreEqual("zzzz.xlsx", excelFiles[1].Name);
            Assert.AreEqual(2, excelFiles.Count);
        }
        [TestMethod]
        public async Task Test_GetExcelFiles_HappyFlowSearch()
        {
            var excelService = _serviceProvider.GetService<IExcelService>();

            List<ExcelFile> excelFiles = await excelService.GetExcelFiles(0, 2, true, false, "a");
            Assert.AreEqual("aaaa.xlsx", excelFiles[0].Name);
        }
        [TestMethod]
        public async Task Test_GetExcelFiles_HappyFlowPageSize()
        {
            var excelService = _serviceProvider.GetService<IExcelService>();

            List<ExcelFile> excelFiles = await excelService.GetExcelFiles(0, 1, true, false, "a");
            Assert.AreEqual(1, excelFiles.Count);
        }
        //[TestMethod]
        //public async Task Test_GetExcelFiles_HappyFlowPageSizeNull()
        //{
        //    var excelService = _serviceProvider.GetService<IExcelService>();

        //    List<ExcelFile> excelFiles = await excelService.GetExcelFiles(100000, 10000000, true, false, "a");
        //    Assert.AreEqual(0, excelFiles.Count);
        //}
        [TestMethod]
        public async Task Test_GetExcelFiles_PageSizeError()
        {
            var excelService = _serviceProvider.GetService<IExcelService>();
            await Assert.ThrowsExceptionAsync<Exception>(() => excelService.GetExcelFiles(0, -1, true, false, "a"));
        }
        [TestMethod]
        public async Task Test_GetExcelFiles_PageError()
        {
            var excelService = _serviceProvider.GetService<IExcelService>();
            await Assert.ThrowsExceptionAsync<Exception>(() => excelService.GetExcelFiles(-1, 1, true, false, "a"));
        }
    }
}

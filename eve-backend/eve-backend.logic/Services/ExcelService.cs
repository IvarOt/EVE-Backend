using eve_backend.logic.Interfaces;
using OfficeOpenXml;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using LicenseContext = OfficeOpenXml.LicenseContext;
using Formatting = Newtonsoft.Json.Formatting;
using Newtonsoft.Json.Linq;
using eve_backend.logic.Models;

namespace eve_backend.logic.Services
{
    public class ExcelService : IExcelService
    {
        private readonly IExcelRepository _excelRepository;
        public ExcelService(IExcelRepository excelRepository)
        {
            _excelRepository = excelRepository;
        }

        private async Task<string> GetJsonFromExcelBasic(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var jsonData = string.Empty;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    var rows = new List<Dictionary<string, object>>();

                    var headers = new List<string>();
                    for (int col = 1; col <= colCount; col++)
                    {
                        headers.Add(worksheet.Cells[1, col].Text);
                    }

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var rowData = new Dictionary<string, object>();
                        for (int col = 1; col <= colCount; col++)
                        {
                            rowData[headers[col - 1]] = worksheet.Cells[row, col].Text;
                        }
                        rows.Add(rowData);
                    }

                    jsonData = JsonConvert.SerializeObject(rows, Formatting.Indented);
                }
            }

            return jsonData;
        }

        public async Task UploadExcel(IFormFile excelFile)
        {
            string json = await GetJsonFromExcelBasic(excelFile);
            var jsonArray = JArray.Parse(json);

            ExcelFile file = new ExcelFile();

            foreach ( var item in jsonArray )
            {
                ExcelObject excelObject = new ExcelObject();

                foreach ( var property in item.Children<JProperty>() )
                {
                    ExcelProperty excelProperty = new ExcelProperty();
                    excelProperty.Name = property.Name;
                    excelProperty.Value = property.Value.ToString();
                    excelObject.ExcelProperties.Add(excelProperty);
                }
                file.excelObjects.Add(excelObject);
            }
            file.Name = excelFile.FileName;
            file.LastUpdated = DateTime.Now;
            await _excelRepository.SaveExcelFile(file);
        }

        public async Task DeleteExcel(int id)
        {
            await _excelRepository.DeleteExcelFile(id);
        }

        public async Task UpdateExcel(int id, string fileName)
        {
            var LastUpdated = DateTime.Now;
            await _excelRepository.UpdateExcelFile(id, fileName, LastUpdated);
        }

        public async Task<List<ExcelFile>> GetExcelFiles()
        {
            var files = await _excelRepository.GetExcelFiles();
            return files;
        }
    }
}

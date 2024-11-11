using eve_backend.logic.Interfaces;
using OfficeOpenXml;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using LicenseContext = OfficeOpenXml.LicenseContext;
using Formatting = Newtonsoft.Json.Formatting;
using Newtonsoft.Json.Linq;
using eve_backend.logic.Models;
using eve_backend.logic.DTO;

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

                    if (rowCount == 1)
                    {
                        var rowData = new Dictionary<string, object>();
                        for (int col = 1; col <= colCount; col++)
                        {
                            rowData[headers[col - 1]] = "";
                            rows.Add(rowData);
                        }
                    }
                    else
                    {
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var rowData = new Dictionary<string, object>();
                            for (int col = 1; col <= colCount; col++)
                            {
                                rowData[headers[col - 1]] = worksheet.Cells[row, col].Text;
                            }
                            rows.Add(rowData);
                        }
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
            var jsonItem = jsonArray.FirstOrDefault();

            foreach (var property in jsonItem.Children<JProperty>())
            {
                file.Structure.Headers.Add(property.Name);
            }

            foreach (var item in jsonArray)
            {
                ExcelObject excelObject = new ExcelObject();

                foreach (var property in item.Children<JProperty>())
                {
                    ExcelProperty excelProperty = new ExcelProperty();
                    excelProperty.Name = property.Name;
                    excelProperty.Value = property.Value.ToString();
                    excelObject.ExcelProperties.Add(excelProperty);
                    excelObject.LastUpdated = DateTime.Now;
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

        public async Task<List<ExcelFile>> GetExcelFiles(int page, int pageSize, bool sortByDate, bool isDescending, string searchTerm)
        {
            if (sortByDate)
            {
                return isDescending
                ? await _excelRepository.GetExcelFiles(page, pageSize, true, searchTerm)
                : await _excelRepository.GetExcelFiles(page, pageSize, false, searchTerm);
            }
            else
            {
                return isDescending
                ? await _excelRepository.GetExcelFilesAZ(page, pageSize, true, searchTerm)
                : await _excelRepository.GetExcelFilesAZ(page, pageSize, false, searchTerm);
            }
        }

        public async Task<ResponseExcelDownload> DownloadExcel(int id)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var file = await _excelRepository.GetExcelFile(id);
            var excelPackage = new ExcelPackage();
            var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

            var headers = file.Structure.Headers;
            for (int i = 0; i < headers.Count; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            var objects = file.excelObjects;
            for (int i = 0; i < objects.Count; i++)
            {
                var properties = objects[i].ExcelProperties;
                for (int j = 0; j < properties.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1].Value = properties[j].Value;
                }
            }

            var stream = new MemoryStream();
            excelPackage.SaveAs(stream);
            stream.Position = 0;
            return new ResponseExcelDownload { Stream = stream, FileName = file.Name, type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };
            
        }   
    }
}

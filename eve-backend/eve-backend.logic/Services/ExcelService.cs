using eve_backend.logic.Interfaces;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http;
using LicenseContext = OfficeOpenXml.LicenseContext;
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

        public async Task UpdateObjectIdentifier(int id, string objectIdentifier)
        {
            await _excelRepository.UpdateObjectIdentifier(id, objectIdentifier);
        }


        public async Task UploadExcel(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelFile excelFile = new ExcelFile();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    var rows = new List<Dictionary<string, object>>();
                    for (int col = 1; col <= colCount; col++)
                    {
                        excelFile.Headers.Add(worksheet.Cells[1, col].Text);
                    }

                    if (rowCount > 1)
                    {
                        for (int row = 2; row <= rowCount; row++)
                        {
                            ExcelObject excelObject = new ExcelObject();
                            for (int col = 1; col <= colCount; col++)
                            {
                                
                                ExcelProperty excelProperty = new ExcelProperty();
                                excelProperty.Name = worksheet.Cells[1, col].Text;
                                excelProperty.Value = worksheet.Cells[row, col].Text;
                                if (excelProperty.Name == "" && excelProperty.Value != "")
                                {
                                    throw new ApplicationException("File has values without headers");
                                }
                                excelObject.ExcelProperties.Add(excelProperty);
                            }
                            excelObject.LastUpdated = DateTime.Now;
                            excelFile.excelObjects.Add(excelObject);
                        }
                    }
                }
            }
            excelFile.Name = file.FileName;
            excelFile.ObjectIdentifier = excelFile.Headers.FirstOrDefault().ToString();
            excelFile.LastUpdated = DateTime.Now;
            await _excelRepository.SaveExcelFile(excelFile);
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
            if( pageSize <= 0)
            {
                throw new  Exception("pagesize must be bigger then 0");
            }
            if ( page < 0 ) 
            {
                throw new Exception("page must be bigger then -1");
            }
                
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

            var headers = file.Headers;
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
        public async Task<int> GetCount()
        {
            var result = await _excelRepository.GetCount();
            return result;
        }
    }
}

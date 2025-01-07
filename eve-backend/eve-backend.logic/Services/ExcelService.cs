using eve_backend.logic.Interfaces;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http;
using LicenseContext = OfficeOpenXml.LicenseContext;
using eve_backend.logic.Models;
using eve_backend.logic.DTO;
using System.Text.Json;
using System.Data.Common;
using OfficeOpenXml.Style;
using OfficeOpenXml.DataValidation;
using System.Numerics;
using System.Runtime.CompilerServices;

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

        public async Task HandleUploadExcel(IFormFile file)
        {
            if (await CheckForConfig(file))
            {
                await UploadIntermediateExcel(file);
            }
            else
            {
                await UploadBasicExcel(file);
            }
        }

        public async Task<bool> CheckForConfig(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelFile excelFile = new ExcelFile();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    foreach (var worksheet in package.Workbook.Worksheets)
                    {
                        if (worksheet.Name.ToLower() == "config")
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
        }

        public async Task UploadIntermediateExcel(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelFile excelFile = new ExcelFile();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var sheets = package.Workbook.Worksheets.Where(x => x.Name.ToLower() != "config").ToList();
                    var ConfigSheet = package.Workbook.Worksheets.Where(x => x.Name.ToLower() == "config").FirstOrDefault();

                    if (ConfigSheet == null)
                    {
                        throw new Exception("Config sheet not found.");
                    }

                    var Header = ConfigSheet.Cells.FirstOrDefault(x => x.Value != null && x.Value.ToString().ToLower() == "header");
                    var Attribute = ConfigSheet.Cells.FirstOrDefault(x => x.Value != null && x.Value.ToString().ToLower() == "attribute");

                    var HeaderStyle = Header.Style;
                    var AttributeStyle = Attribute.Style;

                    var headerLocation = new { Row = Header.Start.Row, Column = Header.Start.Column };
                    var attributeLocation = new { Row = Attribute.Start.Row, Column = Attribute.Start.Column };
                    foreach (var sheet in sheets)
                    {
                        int rowCount = sheet.Dimension.Rows;
                        int colCount = sheet.Dimension.Columns;

                        if (headerLocation.Row == attributeLocation.Row)
                        {
                            var headers = ReadOutColumnHeaders(headerLocation.Column, rowCount, sheet, HeaderStyle);
                            excelFile.Headers = headers.Values.ToList();
                            List<ExcelObject> objects = ReadOutColumnAttributes(attributeLocation.Column, colCount, sheet, AttributeStyle, headers);
                            foreach (var obj in objects.ToList())
                            {
                                bool hasValues = false;
                                foreach (var prop in obj.ExcelProperties)
                                {
                                    if (!string.IsNullOrEmpty(prop.Value))
                                    {
                                        hasValues = true;
                                    }
                                }
                                if (hasValues)
                                {
                                    excelFile.excelObjects.Add(obj);
                                }
                            }
                        }
                        else
                        {
                            var headers = ReadOutRowHeaders(headerLocation.Row, colCount, sheet, HeaderStyle, headerLocation.Column);
                            excelFile.Headers = headers.Values.ToList();
                            List<ExcelObject> objects = ReadOutRowAttributes(attributeLocation.Row, rowCount, sheet, AttributeStyle, headers);
                            foreach (var obj in objects.ToList())
                            {
                                bool hasValues = false;
                                foreach (var prop in obj.ExcelProperties)
                                {
                                    if (!string.IsNullOrEmpty(prop.Value))
                                    {
                                        hasValues = true;
                                    }
                                    break;
                                }
                                if (hasValues)
                                {
                                    excelFile.excelObjects.Add(obj);
                                }
                            }
                        }
                    }
                }
            }
            excelFile.Name = file.FileName;
            excelFile.LastUpdated = DateTime.Now;
            excelFile.ObjectIdentifier = excelFile.Headers.FirstOrDefault();
            foreach (var obj in excelFile.excelObjects)
            {
                obj.Identifier = obj.ExcelProperties.Where(x => x.Name == excelFile.ObjectIdentifier).FirstOrDefault().Value.ToString();

            }
            await _excelRepository.SaveExcelFile(excelFile);
        }

        private Dictionary<int, string> ReadOutColumnHeaders(int headerLocationColumn, int rowCount, ExcelWorksheet sheet, ExcelStyle HeaderStyle)
        {
            //column headers
            var headerStart = headerLocationColumn;
            Dictionary<int, string> headers = new Dictionary<int, string>();
            for (int i = headerStart; i <= rowCount; i++)
            {
                var cell = sheet.Cells[i, headerLocationColumn].Style;
                var cellValue = sheet.Cells[i, headerLocationColumn].Text;

                if (HeaderStyle.Font.Name == cell.Font.Name
                                                && HeaderStyle.Font.Bold == cell.Font.Bold
                                                && !string.IsNullOrEmpty(cellValue)
                                                && HeaderStyle.Font.Color.Indexed == cell.Font.Color.Indexed
                                                && HeaderStyle.Fill.BackgroundColor.Rgb == cell.Fill.BackgroundColor.Rgb
                                                )
                {
                    headers.Add(i, cellValue.Trim());
                }
            }
            return headers;
        }

        private List<ExcelObject> ReadOutColumnAttributes(int AttributeLocationColumn, int collCount, ExcelWorksheet sheet, ExcelStyle HeaderStyle, Dictionary<int, string> headers)
        {
            //column attributes
            List<ExcelObject> attributes = new List<ExcelObject>();
            for (int colIndex = AttributeLocationColumn; colIndex <= collCount; colIndex++)
            {
                ExcelObject excelObject = new ExcelObject();
                foreach (var row in headers)
                {
                    var cell = sheet.Cells[row.Key, colIndex].Style;
                    var cellValue = sheet.Cells[row.Key, colIndex].Text;

                    if (HeaderStyle.Font.Name == cell.Font.Name
                                                    && HeaderStyle.Font.Bold == cell.Font.Bold
                                                    && HeaderStyle.Font.Color.Indexed == cell.Font.Color.Indexed
                                                    && HeaderStyle.Fill.BackgroundColor.Rgb == cell.Fill.BackgroundColor.Rgb
                                                    )
                    {
                        if (cellValue == "" || cellValue == null)
                        {
                            excelObject.ExcelProperties.Add(new ExcelProperty { Name = row.Value, Value = "" });
                        }
                        else
                        {
                            string trimmed = cellValue.Trim();
                            excelObject.ExcelProperties.Add(new ExcelProperty { Name = row.Value, Value = trimmed });
                        }
                    }
                }
                excelObject.Identifier = "";
                excelObject.LastUpdated = DateTime.Now;
                attributes.Add(excelObject);

            }
            return attributes;
        }

        private Dictionary<int, string> ReadOutRowHeaders(int headerLocationRow, int rowCount, ExcelWorksheet sheet, ExcelStyle HeaderStyle, int headerLocationColumn)
        {
            Dictionary<int, string> headers = new Dictionary<int, string>();

            for (int i = headerLocationColumn; i <= rowCount; i++)
            {
                var cell = sheet.Cells[headerLocationRow, i].Style;
                var cellValue = sheet.Cells[headerLocationRow, i].Text;

                if (HeaderStyle.Font.Name == cell.Font.Name
                        && HeaderStyle.Font.Bold == cell.Font.Bold
                        && !string.IsNullOrEmpty(cellValue)
                        && HeaderStyle.Font.Color.Indexed == cell.Font.Color.Indexed
                        && HeaderStyle.Fill.BackgroundColor.Rgb == cell.Fill.BackgroundColor.Rgb
                        )
                {
                    headers.Add(i, cellValue.Trim());
                }
            }
            return headers;
        }


        private List<ExcelObject> ReadOutRowAttributes(int AttributeLocationRow, int RowCount, ExcelWorksheet sheet, ExcelStyle HeaderStyle, Dictionary<int, string> headers)
        {
            //column attributes
            List<ExcelObject> attributes = new List<ExcelObject>();
            for (int rowIndex = AttributeLocationRow; rowIndex <= RowCount; rowIndex++)
            {
                ExcelObject excelObject = new ExcelObject();
                foreach (var colls in headers)
                {
                    var cell = sheet.Cells[rowIndex, colls.Key].Style;
                    var cellValue = sheet.Cells[rowIndex, colls.Key].Text;

                    if (HeaderStyle.Font.Name == cell.Font.Name
                                                    && HeaderStyle.Font.Bold == cell.Font.Bold
                                                    && HeaderStyle.Font.Color.Indexed == cell.Font.Color.Indexed
                                                    && HeaderStyle.Fill.BackgroundColor.Rgb == cell.Fill.BackgroundColor.Rgb
                                                    )
                    {
                        if (cellValue == "" || cellValue == null)
                        {
                            excelObject.ExcelProperties.Add(new ExcelProperty { Name = colls.Value, Value = "" });
                        }
                        else
                        {
                            string trimmed = cellValue.Trim();
                            excelObject.ExcelProperties.Add(new ExcelProperty { Name = colls.Value, Value = trimmed });
                        }
                    }
                }
                excelObject.Identifier = "";
                excelObject.LastUpdated = DateTime.Now;
                attributes.Add(excelObject);

            }
            return attributes;
        }

        public async Task UploadBasicExcel(IFormFile file)
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
                    excelFile.ObjectIdentifier = excelFile.Headers.FirstOrDefault().ToString();

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
                            excelObject.Identifier = excelObject.ExcelProperties.Where(x => x.Name == excelFile.ObjectIdentifier).FirstOrDefault().Value.ToString();
                            excelObject.LastUpdated = DateTime.Now;
                            excelFile.excelObjects.Add(excelObject);
                        }
                    }
                }
            }
            excelFile.Name = file.FileName;
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
            if (pageSize <= 0)
            {
                throw new Exception("pagesize must be bigger then 0");
            }
            if (page < 0)
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

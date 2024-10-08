using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eve_backend.logic.Interfaces;
using eve_backend.logic.DTO;
using eve_backend.logic.Models;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Xml;
using System.ComponentModel;
using OfficeOpenXml;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using LicenseContext = OfficeOpenXml.LicenseContext;
using Formatting = Newtonsoft.Json.Formatting;

namespace eve_backend.logic
{
    public class ExcelService : IExcelService
    {
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

        public async Task<String> UploadExcel(IFormFile excelFile)
        {
            string json = await GetJsonFromExcelBasic(excelFile);

            dynamic model = new DynamicModel();

            model.rewerwerwer = "werwerwer";

            return json;
         
        }
    }
}

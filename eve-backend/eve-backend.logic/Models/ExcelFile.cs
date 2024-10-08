using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eve_backend.logic.Models
{
    public class ExcelFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ExcelObject> excelObjects { get; set; } = new List<ExcelObject>();
        public ExcelFile() { }
    }
}

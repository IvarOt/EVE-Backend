using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eve_backend.logic.Models
{
    public class ExcelObject
    {
        public int Id { get; set; }
        public List<ExcelProperty> ExcelProperties { get; set; } = new List<ExcelProperty>();
        public ExcelObject() { }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace eve_backend.logic.Models
{
    public class ExcelFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ObjectIdentifier { get; set; } 
        public List<string> Headers { get; set; } = new List<string>();
        public List<ExcelObject> excelObjects { get; set; } = new List<ExcelObject>();
        public DateTime LastUpdated { get; set; }
        public ExcelFile() { }
    }
}

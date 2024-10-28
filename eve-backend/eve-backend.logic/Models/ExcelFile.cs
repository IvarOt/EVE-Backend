using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public DateTime LastUpdated { get; set; }

        [ForeignKey("ObjectStructure")]
        public int StructureId { get; set; }
        public ObjectStructure Structure { get; set; } = new ObjectStructure();
        public ExcelFile() { }
    }
}

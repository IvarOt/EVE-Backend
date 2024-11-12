using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eve_backend.logic.Models
{
    public class ExcelProperty
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int ExcelObjectId { get; set; }

        public ExcelProperty() { }
    }
}

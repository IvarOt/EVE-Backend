using eve_backend.logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eve_backend.logic.DTO
{
    public class ResponseGetAllObjects
    {
        public List<ExcelObject> objects { get; set; }
        public string ObjectIdentifier { get; set; }

    }
}

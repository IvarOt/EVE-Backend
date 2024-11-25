using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eve_backend.logic.DTO
{
    public class ResponseExcelDownload
    {
        public Stream Stream { get; set; }
        public string FileName { get; set; }
        public string type { get; set; }
    }
}

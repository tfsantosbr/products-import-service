using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsImport.Consumer.Domain.Imports.Models
{
    public class ProcessSpreadsheetResult
    {
        public int Processed { get; set; }
        public int Errors { get; set; }
        public string ProcessTime { get; set; }
        public int TotalLines { get; set; }
    }
}

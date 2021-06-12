using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsImport.Api.Domain.Imports.Models
{
    public class ImportProductItem
    {
        public Guid ImportId { get; set; }
        public string ProductCode { get; set; }
        public bool IsProcessed { get; set; }
        public string Observation { get; set; }
        public DateTime? ProcessedAt { get; set; }
    }
}

using System;

namespace ProductsImport.Api.Domain.Imports.Models
{
    public class ProductsImportResult
    {
        public Guid ImportId { get; set; }
        public string FilePath { get; set; }
    }
}
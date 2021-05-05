using System;

namespace ProductsImport.Api.Domain.Imports
{
    public class Import
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string SpreadsheetFileUrl { get; set; }
    }
}
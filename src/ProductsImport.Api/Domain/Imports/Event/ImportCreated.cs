using System;

namespace ProductsImport.Api.Domain.Imports.Event
{
    public class ImportCreated
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string SpreadsheetFileUrl { get; set; }
    }
}
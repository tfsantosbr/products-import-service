using System;

namespace ProductsImport.Consumer.Domain.Imports.Events
{
    public class ImportCreated
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string SpreadsheetFileUrl { get; set; }
    }
}
using System;

namespace ProductsImport.Api.Domain.Imports
{
    public class Import
    {
        public Import(string spreadsheetFileUrl)
        {
            Id = Guid.NewGuid();
            SpreadsheetFileUrl = spreadsheetFileUrl;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public string SpreadsheetFileUrl { get; private set; }

        public void Complete()
        {
            CompletedAt = DateTime.UtcNow;
        }
    }
}
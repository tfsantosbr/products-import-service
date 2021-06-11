using System;

namespace ProductsImport.Api.Domain.Imports.Models
{
    public class ImportItem
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int TotalItems { get; set; }
        public int TotalItemsProcessed { get; set; }

        public decimal PercentageProcessed => 
            TotalItems == 0 ? 0 : Math.Round((decimal)((TotalItemsProcessed * 100) / TotalItems), 2);

        public string Duration => 
            CompletedAt.HasValue ? (CompletedAt.Value - CreatedAt).ToString() : null;
    }
}

using System;

namespace Products.Api.Models
{
    public class ProductDetails
    {
        public Guid Id { get; set; }
        public string StoreId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Stock { get; set; }
        public DateTime? ProcessedAt { get; set; }
    }
}

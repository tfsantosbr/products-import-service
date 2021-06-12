using System;

namespace Products.Api.Models
{
    public class ProductItem
    {
        public Guid Id { get; set; }
        public string StoreId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}

using System;

namespace ProductsImport.ProductUpdater.Consumer.Domain.Products
{
    public class Product
    {
        public Product(long storeId, string code, string name, decimal price, decimal stock)
        {
            Id = Guid.NewGuid();
            StoreId = storeId;
            Code = code;
            Name = name;
            Price = price;
            Stock = stock;
            ProcessedAt = DateTime.UtcNow;
        }

        private Product()
        {
        }

        public Guid Id { get; private set; }
        public long StoreId { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public decimal Stock { get; private set; }
        public DateTime ProcessedAt { get; private set; }

        public void Update(string name, decimal price, decimal stock)
        {
            Name = name;
            Price = price;
            Stock = stock;
            ProcessedAt = DateTime.UtcNow;
        }
    }
}

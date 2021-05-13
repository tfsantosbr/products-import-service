using System;

namespace ProductsImport.Consumer.Domain.Imports
{
    public class ImportProduct
    {
        public ImportProduct(Guid importId, long storeId, string productCode, string name, decimal price, decimal stock)
        {
            ImportId = importId;
            ProductCode = productCode;
            StoreId = storeId;
            Name = name;
            Price = price;
            Stock = stock;
        }

        public Guid ImportId { get; private set; }
        public long StoreId { get; private set; }
        public string ProductCode { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public decimal Stock { get; private set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsImport.ProductUpdater.Consumer.Domain.Products.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetProduct(long storeId, string code);
        Task Create(Product product);
        Task Update(Product product);
    }
}

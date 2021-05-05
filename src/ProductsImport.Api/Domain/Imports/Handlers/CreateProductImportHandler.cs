using System.Threading.Tasks;
using ProductsImport.Api.Domain.Imports.Commands;
using ProductsImport.Api.Domain.Imports.Models;

namespace ProductsImport.Api.Domain.Imports.Handlers
{
    public class CreateProductImportHandler : ICreateProductImportHandler
    {
        public async Task<ProductsImportResult> Handle(CreateProductsImport request)
        {
            await Task.Yield();

            return new ProductsImportResult();
        }
    }
}
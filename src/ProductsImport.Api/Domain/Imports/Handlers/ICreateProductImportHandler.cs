using System.Threading.Tasks;
using ProductsImport.Api.Domain.Imports.Commands;
using ProductsImport.Api.Domain.Imports.Models;

namespace ProductsImport.Api.Domain.Imports.Handlers
{
    public interface ICreateProductImportHandler
    {
        Task<ProductsImportResult> Handle(CreateProductsImport request);
    }
}
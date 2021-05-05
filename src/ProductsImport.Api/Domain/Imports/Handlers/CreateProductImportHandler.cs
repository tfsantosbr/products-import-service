using System.Threading.Tasks;
using ProductsImport.Api.Domain.Core.Services.Files;
using ProductsImport.Api.Domain.Imports.Commands;
using ProductsImport.Api.Domain.Imports.Models;

namespace ProductsImport.Api.Domain.Imports.Handlers
{
    public class CreateProductImportHandler : ICreateProductImportHandler
    {
        private readonly IFileService fileService;

        public CreateProductImportHandler(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public async Task<ProductsImportResult> Handle(CreateProductsImport request)
        {
            var result = await fileService.Upload(request.FileName, request.Data);

            return new ProductsImportResult{
                FilePath = result.FilePath
            };
        }
    }
}
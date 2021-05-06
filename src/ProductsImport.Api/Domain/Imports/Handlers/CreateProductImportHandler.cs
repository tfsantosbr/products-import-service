using System.Threading.Tasks;
using ProductsImport.Api.Domain.Core.Services.Files;
using ProductsImport.Api.Domain.Imports.Commands;
using ProductsImport.Api.Domain.Imports.Models;
using ProductsImport.Api.Domain.Imports.Repository;

namespace ProductsImport.Api.Domain.Imports.Handlers
{
    public class CreateProductImportHandler : ICreateProductImportHandler
    {
        private readonly IFileService fileService;
        private readonly IImportRepository importRepository;

        public CreateProductImportHandler(IFileService fileService, IImportRepository importRepository)
        {
            this.fileService = fileService;
            this.importRepository = importRepository;
        }

        public async Task<ProductsImportResult> Handle(CreateProductsImport request)
        {
            // salva planilha no storage

            var result = await fileService.Upload(request.FileName, request.Data);

            // registra importação no banco de dados

            var import = new Import(result.FilePath);

            await importRepository.Create(import);

            return new ProductsImportResult{
                ImportId = import.Id,
                FilePath = result.FilePath
            };
        }
    }
}
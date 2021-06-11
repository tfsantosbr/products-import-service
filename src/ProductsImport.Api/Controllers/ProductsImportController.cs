using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsImport.Api.Domain.Imports.Commands;
using ProductsImport.Api.Domain.Imports.Handlers;
using ProductsImport.Api.Domain.Imports.Repository;
using System.IO;
using System.Threading.Tasks;

namespace ProductsImport.Api.Controllers
{
    [ApiController]
    [Route("products/imports")]
    public class ProductsImportController : ControllerBase
    {
        private readonly ICreateProductImportHandler _productImportHandler;
        private readonly IImportRepository _importRepository;

        public ProductsImportController(ICreateProductImportHandler productImportHandler, IImportRepository importRepository)
        {
            _productImportHandler = productImportHandler;
            _importRepository = importRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateImport(IFormFile spreadsheetFile)
        {
            if (spreadsheetFile is null)
                return BadRequest("Spreadsheet file is required");

            using var memoryStream = new MemoryStream();
            await spreadsheetFile.CopyToAsync(memoryStream);

            var request = new CreateProductsImport(spreadsheetFile.FileName, memoryStream.ToArray());
            await memoryStream.DisposeAsync(); 

            var result = await _productImportHandler.Handle(request);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> ListImports()
        {
            return Ok(await _importRepository.ListImports());
        }
    }
}

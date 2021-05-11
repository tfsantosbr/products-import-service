using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsImport.Api.Domain.Imports.Commands;
using ProductsImport.Api.Domain.Imports.Handlers;
using System.IO;
using System.Threading.Tasks;

namespace ProductsImport.Api.Controllers
{
    [ApiController]
    [Route("products/import")]
    public class ProductsImportController : ControllerBase
    {
        private readonly ICreateProductImportHandler productImportHandler;

        public ProductsImportController(ICreateProductImportHandler productImportHandler)
        {
            this.productImportHandler = productImportHandler;
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

            var result = await productImportHandler.Handle(request);

            return Ok(result);
        }
    }
}

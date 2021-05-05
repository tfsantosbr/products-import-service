using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductsImport.Api.Domain.Imports.Commands;
using ProductsImport.Api.Domain.Imports.Handlers;

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
        public async Task<IActionResult> CreateImport(IFormFile spreadsheet)
        {
            if (spreadsheet is null)
                return BadRequest("Spreadsheet file is required");

            var spreadsheetStream = spreadsheet.OpenReadStream();

            var request = new CreateProductsImport(spreadsheetStream);

            var result = await productImportHandler.Handle(request);

            spreadsheetStream.Close();

            return Ok(result);
        }
    }
}

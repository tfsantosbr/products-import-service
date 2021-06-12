using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsImport.Api.Domain.Imports.Commands;
using ProductsImport.Api.Domain.Imports.Handlers;
using ProductsImport.Api.Domain.Imports.Repository;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ProductsImport.Api.Controllers
{
    [ApiController]
    [Route("imports")]
    public class ImportController : ControllerBase
    {
        private readonly ICreateProductImportHandler _productImportHandler;
        private readonly IImportRepository _importRepository;

        public ImportController(ICreateProductImportHandler productImportHandler, IImportRepository importRepository)
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

        [HttpGet("{importId}")]
        public async Task<IActionResult> ListImports(Guid importId)
        {
            var result = await _importRepository.GetImportDetails(importId);

            if (result is null)
                return NotFound(new { Message = "Import not found" });

            return Ok(result);
        }
    }
}

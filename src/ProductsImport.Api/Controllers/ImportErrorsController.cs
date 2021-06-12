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
    [Route("imports/{importId}/errors")]
    public class ImportErrorsController : ControllerBase
    {
        private readonly IImportRepository _importRepository;

        public ImportErrorsController(IImportRepository importRepository)
        {
            _importRepository = importRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ListImportsErrors(Guid importId)
        {
            return Ok(await _importRepository.ListImportErrors(importId));
        }
    }
}

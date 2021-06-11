using ProductsImport.Api.Domain.Imports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsImport.Api.Domain.Imports.Repository
{
    public interface IImportRepository
    {
        Task Create(Import import);
        Task<IEnumerable<ImportItem>> ListImports();
    }
}

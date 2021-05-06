using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsImport.Api.Domain.Imports.Repository
{
    public interface IImportRepository
    {
        Task Create(Import import);
    }
}

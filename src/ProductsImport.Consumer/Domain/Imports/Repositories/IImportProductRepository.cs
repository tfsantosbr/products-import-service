using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsImport.Consumer.Domain.Imports.Repositories
{
    public interface IImportProductRepository
    {
        Task Create(ImportProduct importProduct);
    }
}

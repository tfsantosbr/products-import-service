using System;
using System.Threading.Tasks;

namespace ProductsImport.ProductUpdater.Consumer.Domain.Imports.Repositories
{
    public interface IImportRepository
    {
        Task MarkProductAsProcessed(Guid importId, string productCode, string observation = null);
        Task<int> TotalProductsProcessing(Guid importId);
    }
}

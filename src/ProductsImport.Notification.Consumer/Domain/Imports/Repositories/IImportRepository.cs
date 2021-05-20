using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsImport.Notification.Consumer.Domain.Imports.Repositories
{
    public interface IImportRepository
    {
        Task MarkImportCompleted(Guid importId, DateTime completedAt);
    }
}

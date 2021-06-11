using ProductsImport.Notification.Consumer.Domain.Imports.Events;
using System.Threading.Tasks;

namespace ProductsImport.Notification.Consumer.Domain.Imports.Handlers
{
    public interface IImportCompletedHandler
    {
        Task Handle(ImportCompleted notification);
    }
}

using System.Threading.Tasks;
using ProductsImport.Consumer.Domain.Imports.Events;

namespace ProductsImport.Consumer.Domain.Imports.Handlers
{
    public interface IImportCreatedHandler
    {
        Task Handle(ImportCreated notification);
    }
}
using ProductsImport.ProductUpdater.Consumer.Domain.Products.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsImport.ProductUpdater.Consumer.Domain.Products.Handlers
{
    public interface  IProcessProductRequestedHandler
    {
        Task Handle(ProcessProductRequested notification);
    }
}

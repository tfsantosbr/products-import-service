using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsImport.ProductUpdater.Consumer.Domain.Imports.Events
{
    public class ImportCompleted
    {
        public ImportCompleted(Guid importId)
        {
            ImportId = importId;
        }

        public Guid ImportId { get; private set; }
    }
}

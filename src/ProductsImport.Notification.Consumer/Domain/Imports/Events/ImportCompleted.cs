using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsImport.Notification.Consumer.Domain.Imports.Events
{
    public class ImportCompleted
    {
        public Guid ImportId { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}

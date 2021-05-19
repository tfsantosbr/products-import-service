using ProductsImport.Consumer.Domain.Imports.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsImport.Consumer.Domain.Imports.Services
{
    public interface ISpreadsheetService
    {
        Task<ProcessSpreadsheetResult> Process(Guid importId, string spreadsheetPath);
    }
}

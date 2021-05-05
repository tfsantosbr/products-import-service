using System.IO;

namespace ProductsImport.Api.Domain.Imports.Commands
{
    public class CreateProductsImport
    {
        public CreateProductsImport(Stream data)
        {
            Data = data;
        }

        public Stream Data { get; }
    }
}
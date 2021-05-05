using System.IO;

namespace ProductsImport.Api.Domain.Imports.Commands
{
    public class CreateProductsImport
    {
        public CreateProductsImport(string fileName, byte[] data)
        {
            Data = data;
            FileName = fileName;
        }

        public string FileName { get; set; }
        public byte[] Data { get; }
    }
}
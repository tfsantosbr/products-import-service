using System.IO;
using System.Threading.Tasks;
using ProductsImport.Api.Domain.Core.Services.Files;

namespace ProductsImport.Api.Infrastructure.Services
{
    public class FileService : IFileService
    {
        public Task<FileServiceResponse> Upload(string fileName, byte[] data)
        {
            var path = Path.Combine("/", fileName);
            
            File.Create(path);

            var result = new FileServiceResponse
            {
                FilePath = path
            };

            return Task.FromResult(result);
        }
    }
}
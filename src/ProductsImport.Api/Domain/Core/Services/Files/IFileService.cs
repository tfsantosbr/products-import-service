using System.Threading.Tasks;

namespace ProductsImport.Api.Domain.Core.Services.Files
{
    public interface IFileService
    {
         Task<FileServiceResponse> Upload(string fileName, byte[] data);
    }
}
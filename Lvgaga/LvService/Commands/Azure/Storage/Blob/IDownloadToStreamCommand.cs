using System.IO;
using System.Threading.Tasks;

namespace LvService.Commands.Azure.Storage.Blob
{
    public interface IDownloadToStreamCommand
    {
        bool CanExecute(dynamic p);
        Task<Stream> ExecuteAsync(dynamic p);
    }
}
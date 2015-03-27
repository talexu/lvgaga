using System.IO;
using System.Threading.Tasks;

namespace LvService.Commands.Azure.Storage.Blob
{
    public class DownloadToStreamCommandChain : IDownloadToStreamCommand
    {
        public IDownloadToStreamCommand PreviousCommand { get; set; }

        public DownloadToStreamCommandChain(IDownloadToStreamCommand command = null)
        {
            PreviousCommand = command;
        }

        public bool CanExecute(dynamic p)
        {
            return PreviousCommand != null;
        }

        public virtual async Task<Stream> ExecuteAsync(dynamic p)
        {
            if (CanExecute(p)) return await PreviousCommand.ExecuteAsync(p);
            return null;
        }
    }
}
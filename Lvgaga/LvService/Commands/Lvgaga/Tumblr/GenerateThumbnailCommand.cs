using System.IO;
using System.Threading.Tasks;
using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.Scaling;
using LvService.Commands.Common;

namespace LvService.Commands.Lvgaga.Tumblr
{
    public class GenerateThumbnailCommand : ICommand
    {
        private Stream _stream;

        public bool CanExecute(dynamic p)
        {
            _stream = p.Stream;
            return _stream != null;
        }

        public Task ExecuteAsync(dynamic p)
        {
            var image = new KalikoImage(_stream);
            var thumb = image.Scale(new CropScaling(128, 128));
            var ms = new MemoryStream();
            thumb.SaveJpg(ms, 99);
            ms.Position = 0;

            p.Stream = ms;

            return Task.FromResult(true);
        }
    }
}
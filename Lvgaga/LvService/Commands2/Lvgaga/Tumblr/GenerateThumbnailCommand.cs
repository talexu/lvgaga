using System.IO;
using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.Scaling;
using LvService.Commands.Common;
using System.Threading.Tasks;

namespace LvService.Commands2.Lvgaga.Tumblr
{
    public class GenerateThumbnailCommand : ICommand
    {
        protected Stream StreamOfOriginal;

        public bool CanExecute(dynamic p)
        {
            StreamOfOriginal = p.StreamOfOriginal;
            return StreamOfOriginal != null;
        }

        public Task ExecuteAsync(dynamic p)
        {
            var image = new KalikoImage(StreamOfOriginal);
            var thumb = image.Scale(new CropScaling(128, 128));
            var ms = new MemoryStream();
            thumb.SaveJpg(ms, 99);
            ms.Position = 0;

            p.StreamOfThumbnail = ms;

            return Task.FromResult(true);
        }
    }
}
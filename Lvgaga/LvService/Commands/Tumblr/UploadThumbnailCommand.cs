using System;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.Scaling;
using LvService.Commands.Common;

namespace LvService.Commands.Tumblr
{
    public class UploadThumbnailCommand : UploadTumblrCommand
    {
        public UploadThumbnailCommand()
        {

        }

        public UploadThumbnailCommand(ICommand command)
            : base(command)
        {

        }

        public new bool CanExecute(dynamic p)
        {
            try
            {
                Container = p.ContainerOfThumbnail;
                BlobName = p.BlobNameOfMedia;
                Stream = p.StreamOfMedia;
                return Container != null && !String.IsNullOrEmpty(BlobName) && Stream != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task ExecuteAsync(dynamic p)
        {
            await base.ExecuteAsync(p as ExpandoObject);
            p.MediaUri = p.BlobUri;

            if (!CanExecute(p)) return;

            p.Container = Container;
            p.BlobName = BlobName;

            var image = new KalikoImage(Stream);
            var thumb = image.Scale(new CropScaling(128, 128));
            var ms = new MemoryStream();
            thumb.SaveJpg(ms, 99);
            ms.Position = 0;

            p.Stream = ms;
        }
    }
}
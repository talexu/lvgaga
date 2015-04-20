using System;
using System.Dynamic;
using System.Threading.Tasks;
using LvService.Commands.Common;

namespace LvService.Commands.Tumblr
{
    public class UploadMediaCommand : UploadTumblrCommand
    {
        public UploadMediaCommand()
        {

        }

        public UploadMediaCommand(ICommand command)
            : base(command)
        {

        }

        public new bool CanExecute(dynamic p)
        {
            Container = p.ContainerOfMedia;
            BlobName = p.BlobNameOfMedia;
            Stream = p.StreamOfMedia;
            return Container != null && !String.IsNullOrEmpty(BlobName) && Stream != null;
        }

        public override async Task ExecuteAsync(dynamic p)
        {
            await base.ExecuteAsync(p as ExpandoObject);

            if (!CanExecute(p)) return;

            p.Container = Container;
            p.BlobName = BlobName;
            p.Stream = Stream;
        }
    }
}
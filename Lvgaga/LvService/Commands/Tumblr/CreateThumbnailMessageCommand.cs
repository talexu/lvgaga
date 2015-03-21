using System;
using System.Dynamic;
using LvService.Commands.Common;
using LvService.Factories;

namespace LvService.Commands.Tumblr
{
    public class CreateThumbnailMessageCommand : AzureStorageCommand
    {
        public IQueueMessageFactory QueueMessageFactory { get; set; }

        public CreateThumbnailMessageCommand()
        {

        }

        public CreateThumbnailMessageCommand(ICommand command)
            : base(command)
        {

        }

        public override bool CanExecute(dynamic p)
        {
            return !String.IsNullOrEmpty(p.FileAbsoluteUri);
        }

        public override async void Execute(dynamic p)
        {
            if (CanExecute(p))
            {
                dynamic message = QueueMessageFactory.CreateCreateThumbnailMessage(p.FileAbsoluteUri);
                if (message != null)
                {
                    await ThumbnailRequestQueue.AddMessageAsync(message);
                }
            }

            base.Execute(p as ExpandoObject);
        }
    }
}
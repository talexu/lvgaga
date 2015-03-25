using System;
using System.Dynamic;
using System.Threading.Tasks;
using LvModel.View.Tumblr;
using LvService.Commands.Common;
using LvService.Factories.Azure.Storage;

namespace LvService.Commands.Tumblr
{
    public class CreateTumblrCommand : CommandChain
    {
        public ITableEntityFactory TableEntityFactory { get; set; }

        private string _partitionKey;
        private string _mediaUri;
        private TumblrText _tumblrText;

        public CreateTumblrCommand()
        {
            
        }

        public CreateTumblrCommand(ICommand command)
            : base(command)
        {

        }

        public new bool CanExecute(dynamic p)
        {
            try
            {
                _partitionKey = p.PartitionKey;
                _mediaUri = p.MediaUri;
                _tumblrText = p.TumblrText;
                return !String.IsNullOrEmpty(_partitionKey) &&
                       !String.IsNullOrEmpty(_mediaUri) &&
                       _tumblrText != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return;

            var tumblrEntity = TableEntityFactory.CreateTumblrEntity(p);
            if (tumblrEntity == null) return;

            p.Entity = tumblrEntity;
            await base.ExecuteAsync(p as ExpandoObject);
        }
    }
}
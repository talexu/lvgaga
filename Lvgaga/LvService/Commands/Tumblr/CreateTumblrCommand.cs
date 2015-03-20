using LvModel.Azure.StorageTable;
using LvModel.View.Tumblr;
using LvService.Commands.Common;
using LvService.Factories;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Tumblr
{
    public class CreateTumblrCommand : CommandChain
    {
        public ITableEntityFactory TableEntityFactory { get; set; }

        public CreateTumblrCommand()
        {

        }

        public CreateTumblrCommand(ICommand command)
            : base(command)
        {

        }

        public override bool CanExecute(dynamic p)
        {
            return p.Table is CloudTable && p.ViewModel is TumblrViewModel;
        }

        public override async void Execute(dynamic p)
        {
            if (!CanExecute(p)) return;

            CloudTable table = p.Table;
            TumblrEntity tumblrEntity = TableEntityFactory.CreateTumblrEntity(p.ViewModel);
            if (table == null || tumblrEntity == null) return;

            p.Entity = tumblrEntity;
            p.RelativeUri = tumblrEntity.Uri;
            await table.ExecuteAsync(TableOperation.Insert(tumblrEntity));

            base.Execute(p as object);
        }
    }
}
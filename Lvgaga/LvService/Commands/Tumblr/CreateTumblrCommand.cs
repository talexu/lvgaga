using System;
using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.View.Tumblr;
using LvService.Commands.Common;
using LvService.Factories;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Tumblr
{
    public class CreateTumblrCommand : AzureStorageCommand
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
            if (!base.CanExecute(p as ExpandoObject)) return false;

            try
            {
                return p.TumblrText is TumblrText;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return;

            CloudTable table = p.Table;
            TumblrEntity tumblrEntity = TableEntityFactory.CreateTumblrEntity(p.TumblrText);
            if (table == null || tumblrEntity == null) return;

            p.TableEntity = tumblrEntity;
            await table.ExecuteAsync(TableOperation.Insert(tumblrEntity));

            await base.ExecuteAsync(p as ExpandoObject);
        }
    }
}
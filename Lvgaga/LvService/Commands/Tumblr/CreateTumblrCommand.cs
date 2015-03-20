using System.Dynamic;
using System.Windows.Input;
using LvService.Commands.Common;
using Microsoft.WindowsAzure.Storage.Table;

namespace LvService.Commands.Tumblr
{
    public class CreateTumblrCommand : AzureStorageCommand
    {
        public CreateTumblrCommand()
        {

        }

        public CreateTumblrCommand(ICommand command)
            : base(command)
        {

        }

        public override void Execute(object parameter)
        {
            base.Execute(parameter);

            dynamic p = parameter as ExpandoObject;
            if (p == null) return;

            var table = p.table;
            var tumblrEntity = TableEntityFactory.CreateTumblrEntity(p.ViewModel);
            if (table == null || tumblrEntity == null) return;

            var insertOperation = TableOperation.Insert(tumblrEntity);
            table.Execute(insertOperation);
        }
    }
}
using System;
using System.Dynamic;
using System.Threading.Tasks;
using LvModel.View.Tumblr;
using LvService.Commands.Azure.Storage.Table;
using LvService.Commands.Common;
using LvService.Factories;

namespace LvService.Commands.Tumblr
{
    public class CreateTumblrCommand : CommandChain
    {
        public ITableEntityFactory TableEntityFactory { get; set; }
        private TumblrText _tumblrText;

        public CreateTumblrCommand()
        {
            NextCommand = new CreateTableEntityCommand();
        }

        public CreateTumblrCommand(ICommand command)
            : base(command)
        {

        }

        public new bool CanExecute(dynamic p)
        {
            try
            {
                _tumblrText = p.TumblrText;
                return _tumblrText != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return;

            var tumblrEntity = TableEntityFactory.CreateTumblrEntity(_tumblrText);
            if (tumblrEntity == null) return;

            p.Entity = tumblrEntity;
            await base.ExecuteAsync(p as ExpandoObject);
        }
    }
}
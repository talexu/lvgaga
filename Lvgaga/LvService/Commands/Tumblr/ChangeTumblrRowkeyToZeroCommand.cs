using System;
using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvModel.View.Tumblr;
using LvService.Commands.Common;

namespace LvService.Commands.Tumblr
{
    public class ChangeTumblrRowkeyToZeroCommand : CommandChain
    {
        private readonly string _allCategory = TumblrCategory.All.ToString("D");
        private TumblrEntity _entity;

        public ChangeTumblrRowkeyToZeroCommand()
        {

        }

        public ChangeTumblrRowkeyToZeroCommand(ICommand command)
            : base(command)
        {

        }

        public new bool CanExecute(dynamic p)
        {
            try
            {
                _entity = p.Entity;
                return _entity != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return;

            _entity.RowKey = _allCategory + _entity.RowKey.Substring(1);

            await base.ExecuteAsync(p as ExpandoObject);
        }
    }
}
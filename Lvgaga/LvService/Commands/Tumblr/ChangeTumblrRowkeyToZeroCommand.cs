using System;
using System.Dynamic;
using System.Threading.Tasks;
using LvModel.Azure.StorageTable;
using LvService.Commands.Common;

namespace LvService.Commands.Tumblr
{
    public class ChangeTumblrRowkeyToZeroCommand : CommandChain
    {
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

            _entity.RowKey = '0' + _entity.RowKey.Substring(1);
            p.Entity = _entity;

            await base.ExecuteAsync(p as ExpandoObject);
        }
    }
}
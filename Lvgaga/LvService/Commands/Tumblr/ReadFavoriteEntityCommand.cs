using System.Dynamic;
using System.Threading.Tasks;
using LvService.Commands.Azure.Storage.Table;

namespace LvService.Commands.Tumblr
{
    public class ReadFavoriteEntityCommand : ReadTableEntityCommand
    {
        public ReadFavoriteEntityCommand()
        {

        }

        public ReadFavoriteEntityCommand(ITableEntityCommand command)
            : base(command)
        {

        }

        public override async Task<T> ExecuteAsync<T>(dynamic p)
        {

            return await base.ExecuteAsync<T>(p as ExpandoObject);
        }
    }
}
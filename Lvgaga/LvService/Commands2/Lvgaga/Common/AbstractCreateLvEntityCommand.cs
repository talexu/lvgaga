using System.Threading.Tasks;
using LvService.Commands.Common;
using LvService.Factories.Azure.Storage;
using LvService.Factories.Uri;

namespace LvService.Commands2.Lvgaga.Common
{
    public abstract class AbstractCreateLvEntityCommand : ICommand
    {
        public IUriFactory UriFactory { get; set; }
        public ITableEntityFactory TableEntityFactory { get; set; }

        public abstract bool CanExecute(dynamic p);

        public abstract Task ExecuteAsync(dynamic p);
    }
}
using System.Threading.Tasks;
using LvService.Commands.Common;
using LvService.Factories.Azure.Storage;
using LvService.Factories.Uri;
using Microsoft.Practices.Unity;

namespace LvService.Commands.Lvgaga.Common
{
    public abstract class AbstractCreateLvEntityCommand : ICommand
    {
        [Dependency]
        public IUriFactory UriFactory { get; set; }
        [Dependency]
        public ITableEntityFactory TableEntityFactory { get; set; }

        public abstract bool CanExecute(dynamic p);

        public abstract Task ExecuteAsync(dynamic p);
    }
}
using LvService.Commands.Common;
using LvService.Factories.Azure.Storage;
using LvService.Factories.Uri;

namespace LvService.Commands.Tumblr
{
    public class CreateLvEntityCommand : CommandChain
    {
        public IUriFactory UriFactory { get; set; }
        public ITableEntityFactory TableEntityFactory { get; set; }

        public CreateLvEntityCommand()
        {

        }

        public CreateLvEntityCommand(ICommand command)
            : base(command)
        {

        }
    }
}
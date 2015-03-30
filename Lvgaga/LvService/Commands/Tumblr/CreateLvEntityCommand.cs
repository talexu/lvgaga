using LvService.Commands.Common;
using LvService.Factories.Azure.Storage;

namespace LvService.Commands.Tumblr
{
    public class CreateLvEntityCommand : CommandChain
    {
        public ITableEntityFactory TableEntityFactory { get; set; }

        public CreateLvEntityCommand()
        {

        }

        public CreateLvEntityCommand(ICommand command) : base(command)
        {

        }
    }
}
using System.Threading.Tasks;

namespace LvService.Commands.Common
{
    public class CommandChain : ICommand
    {
        public ICommand NextCommand { get; set; }

        public CommandChain()
        {

        }

        public CommandChain(ICommand command)
        {
            NextCommand = command;
        }

        public bool CanExecute(dynamic p)
        {
            return NextCommand != null;
        }

        public virtual async Task ExecuteAsync(dynamic p)
        {
            if (CanExecute(p)) await NextCommand.ExecuteAsync(p);
        }
    }
}

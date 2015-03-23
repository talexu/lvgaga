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

        public virtual bool CanExecute(dynamic p)
        {
            return NextCommand != null && NextCommand.CanExecute(p);
        }

        public virtual async Task ExecuteAsync(dynamic p)
        {
            if (NextCommand != null && NextCommand.CanExecute(p)) await NextCommand.ExecuteAsync(p);
        }
    }
}

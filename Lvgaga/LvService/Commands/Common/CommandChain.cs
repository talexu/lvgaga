using System.Threading.Tasks;

namespace LvService.Commands.Common
{
    public class CommandChain : ICommand
    {
        public ICommand PreviousCommand { get; set; }

        public CommandChain(ICommand command = null)
        {
            PreviousCommand = command;
        }

        public bool CanExecute(dynamic p)
        {
            return PreviousCommand != null;
        }

        public virtual async Task ExecuteAsync(dynamic p)
        {
            if (CanExecute(p)) await PreviousCommand.ExecuteAsync(p);
        }
    }
}

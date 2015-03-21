using System.Threading.Tasks;

namespace LvService.Commands.Common
{
    public class CommandChain : ICommand
    {
        private readonly ICommand _command;

        public CommandChain()
        {

        }

        public CommandChain(ICommand command)
        {
            _command = command;
        }

        public virtual bool CanExecute(dynamic p)
        {
            return _command != null && _command.CanExecute(p);
        }

        public virtual async Task ExecuteAsync(dynamic p)
        {
            if (_command != null && _command.CanExecute(p)) await _command.ExecuteAsync(p);
        }
    }
}

using System.Linq;
using System.Threading.Tasks;

namespace LvService.Commands.Common
{
    public class CompositeCommand : ICommand
    {
        private readonly ICommand[] _commands;

        public CompositeCommand(params ICommand[] commands)
        {
            _commands = commands;
        }

        public bool CanExecute(dynamic p)
        {
            return _commands.All(command => command != null);
        }

        public async Task ExecuteAsync(dynamic p)
        {
            if (!CanExecute(p)) return;

            foreach (var command in _commands)
            {
                await command.ExecuteAsync(p);
            }
        }
    }
}
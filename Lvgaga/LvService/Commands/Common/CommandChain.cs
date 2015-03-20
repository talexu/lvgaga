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

        public virtual void Execute(dynamic p)
        {
            if (_command != null && _command.CanExecute(p)) _command.Execute(p);
        }
    }
}

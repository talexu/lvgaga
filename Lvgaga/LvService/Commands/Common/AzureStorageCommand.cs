using System;
using System.Windows.Input;
using LvService.Factories;

namespace LvService.Commands.Common
{
    public class AzureStorageCommand : ICommand
    {
        private readonly ICommand _command;
        public ITableEntityFactory TableEntityFactory;

        public AzureStorageCommand()
        {

        }

        public AzureStorageCommand(ICommand command)
        {
            _command = command;
        }

        public virtual bool CanExecute(object parameter)
        {
            return _command != null && _command.CanExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public virtual void Execute(object parameter)
        {
            if (_command != null)
            {
                _command.Execute(parameter);
            }
        }
    }
}

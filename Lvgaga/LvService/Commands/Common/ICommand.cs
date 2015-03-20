namespace LvService.Commands.Common
{
    public interface ICommand
    {
        bool CanExecute(dynamic p);
        void Execute(dynamic p);
    }
}
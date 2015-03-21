using System.Threading.Tasks;

namespace LvService.Commands.Common
{
    public interface ICommand
    {
        bool CanExecute(dynamic p);
        Task ExecuteAsync(dynamic p);
    }
}
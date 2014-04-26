using System.Windows.Input;

namespace NabbR.Commands
{
    public interface IDelegateCommand<T> : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}

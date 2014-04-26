
using System;
namespace NabbR.Commands
{
    public class DelegateCommand : IDelegateCommand
    {
        private readonly Action execute;
        private readonly Func<Boolean> canExecute;

        public DelegateCommand(Action execute, Func<Boolean> canExecute = null)
        {
            if (execute == null) throw new ArgumentNullException("execute");
            this.execute = execute;
            this.canExecute = canExecute;
        }

        #region IDelegateCommand<T> Members
        public void RaiseCanExecuteChanged()
        {
            var eh = this.CanExecuteChanged;
            if (eh != null)
            {
                eh(this, EventArgs.Empty);
            }
        }
        #endregion

        #region ICommand Members
        public bool CanExecute(Object parameter)
        {
            return canExecute == null || canExecute();
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(Object parameter)
        {
            this.execute();
        }
        #endregion
    }
}

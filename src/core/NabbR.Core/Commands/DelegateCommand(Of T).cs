using System;

namespace NabbR.Commands
{
    public class DelegateCommand<T> : IDelegateCommand<T>
    {
        private readonly Action<T> execute;
        private readonly Func<T, Boolean> canExecute;

        public DelegateCommand(Action<T> execute, Func<T, Boolean> canExecute = null)
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
            return canExecute == null || canExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(Object parameter)
        {
            this.execute((T)parameter);
        }
        #endregion
    }
}

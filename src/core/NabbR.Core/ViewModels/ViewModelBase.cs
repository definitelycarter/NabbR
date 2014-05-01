using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NabbR.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected Boolean Set<T>(ref T storage, T value, [CallerMemberName] String propertyName = "")
        {
            Boolean result = false;
            if (!EqualityComparer<T>.Default.Equals(storage, value))
            {
                storage = value;
                this.RaisePropertyChanged(propertyName);

                result = true;
            }
            return result;
        }
        protected void RaisePropertyChanged(String propertyName)
        {
            var eh = this.PropertyChanged;
            if (eh != null)
            {
                eh(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public virtual IEnumerable<CommandViewModel> GetAvailableActions(ViewType viewType)
        {
            yield break;
        }

        public enum ViewType { Any, Prompt };
    }
}
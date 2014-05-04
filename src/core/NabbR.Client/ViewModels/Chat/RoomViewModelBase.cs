using System;

namespace NabbR.ViewModels.Chat
{
    public class RoomViewModelBase : ViewModelBase
    {
        private String name;

        public String Name
        {
            get { return name; }
            set { this.Set(ref name, value); }
        }
    }
}

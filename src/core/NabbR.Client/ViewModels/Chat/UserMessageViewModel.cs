using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NabbR.ViewModels.Chat
{
    public class UserMessageViewModel : MessageViewModel
    {
        private Boolean isSelf;
        private UserViewModel user;
        private readonly ObservableCollection<MessageViewModel> messages;

        public UserMessageViewModel()
        {
            this.messages = new ObservableCollection<MessageViewModel>();
        }

        public UserViewModel User
        {
            get { return this.user; }
            set { this.Set(ref this.user, value); }
        }
        public Boolean IsSelf
        {
            get { return this.isSelf; }
            set { this.Set(ref this.isSelf, value); }
        }

        public ICollection<MessageViewModel> Messages
        {
            get { return this.messages; }
        }
    }
}

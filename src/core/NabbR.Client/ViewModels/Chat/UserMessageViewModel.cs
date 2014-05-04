using JabbR.Client.Models;
using System;

namespace NabbR.ViewModels.Chat
{
    public class UserMessageViewModel : MessageViewModel
    {
        private Boolean isSelf;
        private UserViewModel user;

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
    }
}

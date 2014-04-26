using JabbR.Client.Models;
using System;

namespace NabbR.ViewModels.Chat
{
    public class UserViewModel : ViewModelBase
    {
        private String name;
        private String hash;
        private UserStatus userStatus;

        public String Name
        {
            get { return this.name; }
            set { this.Set(ref this.name, value); }
        }

        public String Hash
        {
            get { return this.hash; }
            set { this.Set(ref this.hash, value); }
        }
        public UserStatus Status
        {
            get { return this.userStatus; }
            set { this.Set(ref this.userStatus, value); }
        }
    }
}

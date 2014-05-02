using JabbR.Client.Models;

namespace NabbR.ViewModels.Chat
{
    public class UserMessageViewModel : MessageViewModel
    {
        private UserViewModel user;

        public UserViewModel User
        {
            get { return this.user; }
            set { this.Set(ref this.user, value); }
        }
    }
}

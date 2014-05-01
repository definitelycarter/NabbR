using JabbR.Client.Models;

namespace NabbR.ViewModels.Chat
{
    public class UserMessageViewModel : MessageViewModel
    {
        private UserViewModel user;

        public UserMessageViewModel(Message message, UserViewModel user)
            : base(message)
        {
            this.user = user;
        }
        public UserMessageViewModel()
        {

        }

        public UserViewModel User
        {
            get { return this.user; }
            set { this.Set(ref this.user, value); }
        }
    }
}

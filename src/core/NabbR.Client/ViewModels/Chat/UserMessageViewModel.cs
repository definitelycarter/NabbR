using JabbR.Client.Models;

namespace NabbR.ViewModels.Chat
{
    public class UserMessageViewModel : MessageViewModel
    {
        private readonly UserViewModel user;

        public UserMessageViewModel(Message message, UserViewModel user)
            : base(message)
        {
            this.user = user;
        }

        public UserViewModel User
        {
            get { return this.user; }
        }
    }
}

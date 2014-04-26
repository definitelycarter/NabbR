using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NabbR.ViewModels.Chat
{
    public class ChatUserViewModel : ViewModelBase
    {
        private Boolean isTyping;
        private readonly UserViewModel user;
        
        public ChatUserViewModel(UserViewModel user)
        {
            this.user = user;
        }

        public UserViewModel User
        {
            get { return this.user; }
        }

        public Boolean IsTyping
        {
            get { return this.isTyping; }
            set { this.Set(ref this.isTyping, value); }
        }
    }
}

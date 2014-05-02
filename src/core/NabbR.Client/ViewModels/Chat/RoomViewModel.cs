using JabbR.Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace NabbR.ViewModels.Chat
{
    public class RoomViewModel : RoomViewModelBase
    {
        private String welcome;
        private readonly ObservableCollection<UserViewModel> users;
        private readonly ObservableCollection<MessageViewModel> messages;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomViewModel" /> class.
        /// </summary>
        public RoomViewModel()
        {
            this.users = new ObservableCollection<UserViewModel>();
            this.messages = new ObservableCollection<MessageViewModel>();
        }

        /// <summary>
        /// Gets the room welcome message.
        /// </summary>
        /// <value>
        /// The room welcome message.
        /// </value>
        public String Welcome
        {
            get { return this.welcome; }
            set { this.Set(ref welcome, value); }
        }

        public ObservableCollection<MessageViewModel> Messages
        {
            get { return this.messages; }
        }
        public ObservableCollection<UserViewModel> Users
        {
            get { return this.users; }
        }

        public void AddNotification(String notificationMessage)
        {
            Message message = new Message { Content = notificationMessage, When = DateTime.Now };
            this.Messages.Add(message.AsViewModel());
        }

        public void Add(Message message)
        {
            UserViewModel userVm = this.Users.FirstOrDefault(u => u.Name == message.User.Name);
            
            if (userVm == null)
            {
                userVm = message.User.AsViewModel();
                userVm.Status = UserStatus.Offline;
            }

            MessageViewModel messageVm = message.AsUserMessageViewModel(userVm);
            this.Messages.Add(messageVm);
            userVm.IsTyping = false;
        }
    }
}

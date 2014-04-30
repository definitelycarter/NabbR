using JabbR.Client.Models;
using System;
using System.Collections.ObjectModel;

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
            this.Messages.Add(new MessageViewModel(message));
        }
    }
}

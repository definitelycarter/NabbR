using JabbR.Client.Models;
using NabbR.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace NabbR.ViewModels.Chat
{
    public class RoomViewModel : PublicRoomViewModel, IRoom
    {
        private String welcome;

        private readonly IJabbRContext jabbrContext;
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
        public RoomViewModel(IJabbRContext jabbrContext)
            : this()
        {
            this.jabbrContext = jabbrContext;
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
            UserViewModel user = this.EnsureUser(message.User);

            UserMessageViewModel lastUserMessage = this.Messages.LastOrDefault() as UserMessageViewModel;
            if (lastUserMessage != null && lastUserMessage.User.Name == user.Name)
            {
                lastUserMessage.Messages.Add(message.AsViewModel());
            }
            else
            {
                UserMessageViewModel messageVm = message.AsUserMessageViewModel(user);
                messageVm.IsSelf = user.Name == this.jabbrContext.Username;
                this.Messages.Add(messageVm);
            }
            user.IsTyping = false;
        }

        private UserViewModel EnsureUser(User user)
        {
            UserViewModel userVm = this.Users.FirstOrDefault(u => u.Name == user.Name);

            if (userVm == null)
            {
                userVm = user.AsViewModel();
                userVm.Status = UserStatus.Offline;
            }

            return userVm;
        }


        public Task<Boolean> SendMessage(String message)
        {
            return this.jabbrContext.SendMessage(message, this.Name);
        }
    }
}

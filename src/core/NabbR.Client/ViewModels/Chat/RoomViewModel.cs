using JabbR.Client.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NabbR.ViewModels.Chat
{
    public class RoomViewModel : ViewModelBase
    {
        private String name;
        private String topic;
        private readonly ObservableCollection<ChatUserViewModel> users;
        private readonly ObservableCollection<MessageViewModel> messages;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomViewModel" /> class.
        /// </summary>
        public RoomViewModel()
        {
            this.users = new ObservableCollection<ChatUserViewModel>();
            this.messages = new ObservableCollection<MessageViewModel>();
        }
        /// <summary>
        /// Gets the name of the room.
        /// </summary>
        /// <value>
        /// The name of the room.
        /// </value>
        public String Name
        {
            get { return name; }
            set { this.Set(ref name, value); }
        }
        /// <summary>
        /// Gets the room welcome.
        /// </summary>
        /// <value>
        /// The room welcome.
        /// </value>
        public String Welcome
        {
            get { return null; }
        }
        /// <summary>
        /// Gets the room topic.
        /// </summary>
        /// <value>
        /// The room topic.
        /// </value>
        public String Topic
        {
            get { return this.topic; }
            set { this.Set(ref topic, value); }
        }
        public ObservableCollection<MessageViewModel> Messages
        {
            get { return this.messages; }
        }
        public ObservableCollection<ChatUserViewModel> Users
        {
            get { return this.users; }
        }
    }
}

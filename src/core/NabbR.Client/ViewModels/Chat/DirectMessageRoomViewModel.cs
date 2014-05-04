using NabbR.Events;
using NabbR.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace NabbR.ViewModels.Chat
{
    public class DirectMessageRoomViewModel : RoomViewModelBase, IRoom, IHandle<DirectMessageReceived>
    {
        private UserViewModel to;
        private UserViewModel from;

        private readonly IJabbRContext jabbrContext;
        private readonly IEventAggregator eventAggregator;
        private readonly ObservableCollection<MessageViewModel> messages;

        public DirectMessageRoomViewModel(IJabbRContext jabbrContext,
                                          IEventAggregator eventAggregator)
        {
            this.jabbrContext = jabbrContext;
            this.eventAggregator = eventAggregator;
            this.messages = new ObservableCollection<MessageViewModel>();

            this.eventAggregator.Subscribe(this);
        }
        public DirectMessageRoomViewModel()
        {
            this.messages = new ObservableCollection<MessageViewModel>();
        }

        public UserViewModel To
        {
            get { return this.to; }
            set { this.Set(ref this.to, value); }
        }
        public UserViewModel From
        {
            get { return this.from; }
            set { this.Set(ref this.from, value); }
        }
        public ObservableCollection<MessageViewModel> Messages
        {
            get { return this.messages; }
        }
        public String RoomName { get; set; }

        void IHandle<DirectMessageReceived>.Handle(DirectMessageReceived message)
        {
            if (this.from.Name == message.From)
            {
                this.messages.Add(new MessageViewModel { Content = message.Message, MessageDateTime = DateTime.Now });
            }
        }

        String IRoom.Name
        {
            get { return from != null ? from.Name : null; }
        }

        Task<Boolean> IRoom.SendMessage(String message)
        {
            String command = String.Format("/msg @{0} {1}", from.Name, message);
            return this.jabbrContext.SendMessage(command, this.RoomName);
        }
    }
}

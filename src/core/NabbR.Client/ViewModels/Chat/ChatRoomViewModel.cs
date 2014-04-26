using NabbR.Commands;
using NabbR.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace NabbR.ViewModels.Chat
{
    public class ChatRoomViewModel : ViewModelBase, INavigationAware
    {
        private RoomViewModel room;
        private String messageToSend;
        private readonly IJabbRContext jabbrContext;
        private IDelegateCommand sendMessageCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatRoomViewModel"/> class.
        /// </summary>
        /// <param name="jabbrContext">The jabbr context.</param>
        public ChatRoomViewModel(IJabbRContext jabbrContext)
        {
            this.jabbrContext = jabbrContext;
        }

        /// <summary>
        /// Gets the room.
        /// </summary>
        /// <value>
        /// The room.
        /// </value>
        public RoomViewModel Room
        {
            get { return this.room; }
            private set { this.Set(ref this.room, value); }
        }

        public IDelegateCommand SendMessageCommand
        {
            get { return this.sendMessageCommand ?? (this.sendMessageCommand = new DelegateCommand(() => this.HandleSendMessage(), () => this.CanSendMessage())); }
        }

        public String MessageToSend
        {
            get { return this.messageToSend; }
            set
            {
                if (this.Set(ref this.messageToSend, value))
                {
                    this.SendMessageCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private void HandleSendMessage()
        {
            this.jabbrContext.SendMessage(messageToSend, this.room.Name);
            this.MessageToSend = null;
        }
        private Boolean CanSendMessage()
        {
            return !String.IsNullOrWhiteSpace(this.MessageToSend);
        }
        /// <summary>
        /// Called when navigated.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        void INavigationAware.Navigated(IDictionary<String, String> parameters)
        {
            String roomName;
            if (parameters.TryGetValue("room", out roomName))
            {
                this.Room = this.jabbrContext.Rooms.FirstOrDefault(r => r.Name == roomName);
            }
        }
    }
}

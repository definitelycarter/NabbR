using NabbR.Commands;
using NabbR.Services;
using System;

namespace NabbR.ViewModels.Chat
{
    public class ChatRoomViewModelBase<T> : ViewModelBase where T : IRoom
    {
        private T room;
        private String composedMessage;
        private IDelegateCommand sendMessageCommand;

        protected readonly IJabbRContext jabbrContext;

        public ChatRoomViewModelBase(IJabbRContext jabbrContext)
        {
            this.jabbrContext = jabbrContext;
        }

        public T Room
        {
            get { return this.room; }
            set { this.Set(ref this.room, value); }
        }

        public IDelegateCommand SendMessageCommand
        {
            get { return this.sendMessageCommand ?? (this.sendMessageCommand = new DelegateCommand(this.HandleSendMessage, this.CanSendMessage)); }
        }

        public virtual String ComposedMessage
        {
            get { return this.composedMessage; }
            set
            {
                if (this.Set(ref this.composedMessage, value))
                {
                    this.SendMessageCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private void HandleSendMessage()
        {
            this.Room.SendMessage(this.composedMessage);
            this.ComposedMessage = null;
        }
        private Boolean CanSendMessage()
        {
            return !String.IsNullOrWhiteSpace(this.ComposedMessage);
        }
    }
}

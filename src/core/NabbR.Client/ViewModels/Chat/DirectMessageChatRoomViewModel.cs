using NabbR.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NabbR.ViewModels.Chat
{
    public class DirectMessageChatRoomViewModel : ChatRoomViewModelBase<DirectMessageRoomViewModel>, INavigationAware
    {
        public DirectMessageChatRoomViewModel(IJabbRContext jabbrContext)
            : base(jabbrContext)
        {
        }

#if DEBUG
        public DirectMessageChatRoomViewModel()
            : base(null)
        {
            this.Room = new DirectMessageRoomViewModel();
            this.Room.To = new UserViewModel { Name = "davepermen.net", IsTyping = false, Hash = "6ad5f8c742f1e8ec61000e2b0900fc76", Status = JabbR.Client.Models.UserStatus.Active };
            this.Room.From = new UserViewModel { Name = "definitelycarter", IsTyping = true, Hash = "0ca89936b4a526bc1ee7ad4c5eb6fcbe", Status = JabbR.Client.Models.UserStatus.Inactive };

            this.Room.Messages.Add(new UserMessageViewModel { Content = "Hey dave, how are you doing today?!?!", IsSelf = true, MessageDateTime = DateTime.Now, User = this.Room.To });
            this.Room.Messages.Add(new UserMessageViewModel { Content = "Hey adam, i am doing quite fine. Thanks for asking!", IsSelf = false, MessageDateTime = DateTime.Now, User = this.Room.From });
        }
#endif
        void INavigationAware.Navigated(IDictionary<String, String> parameters)
        {
            String username;
            if (parameters.TryGetValue("username", out username))
            {
                this.Room = this.jabbrContext.DirectMessageRooms.First(dm => dm.RoomName == username);
            }
        }
    }
}

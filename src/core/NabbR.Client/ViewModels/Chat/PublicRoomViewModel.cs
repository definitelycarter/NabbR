using System;

namespace NabbR.ViewModels.Chat
{
    public class PublicRoomViewModel : RoomViewModelBase
    {
        private String topic;

        public String Topic
        {
            get { return this.topic; }
            set { this.Set(ref topic, value); }
        }
    }
}

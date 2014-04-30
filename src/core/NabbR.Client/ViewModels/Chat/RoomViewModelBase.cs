
using System;
namespace NabbR.ViewModels.Chat
{
    public class RoomViewModelBase : ViewModelBase
    {
        private String name;
        private String topic;

        public String Name
        {
            get { return name; }
            set { this.Set(ref name, value); }
        }
        public String Topic
        {
            get { return this.topic; }
            set { this.Set(ref topic, value); }
        }
    }
}

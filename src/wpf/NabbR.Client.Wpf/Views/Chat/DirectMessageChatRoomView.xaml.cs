
using NabbR.ViewModels;
using NabbR.ViewModels.Chat;

namespace NabbR.Views.Chat
{
    [View("/directmessage")]
    [ViewModel(typeof(DirectMessageChatRoomViewModel))]
    public partial class DirectMessageChatRoomView
    {
        public DirectMessageChatRoomView()
        {
            InitializeComponent();
        }
    }
}

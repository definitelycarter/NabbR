
using NabbR.ViewModels;
using NabbR.ViewModels.Chat;
using System;
namespace NabbR.Views.Chat
{
    [View(LobbyView.Uri)]
    [ViewModel(typeof(LobbyViewModel))]
    public partial class LobbyView
    {
        public const String Uri = "/chat/lobby";

        public LobbyView()
        {
            InitializeComponent();
        }
    }
}

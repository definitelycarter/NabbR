using NabbR.ViewModels;
using NabbR.ViewModels.Chat;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace NabbR.Views.Chat
{
    [View("/ChatRoom")]
    [ViewModel(typeof(ChatRoomViewModel))]
    partial class ChatRoomView
    {
        private ChatRoomViewModel chatRoom;
        public ChatRoomView()
        {
            InitializeComponent();
            this.DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.chatRoom = (ChatRoomViewModel)e.NewValue;
        }

        private void TextBox_KeyUp(Object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                ICommand command = this.chatRoom.SendMessageCommand;

                if (command.CanExecute(null))
                {
                    command.Execute(null);
                }
            }
        }
    }
}

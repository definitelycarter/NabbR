using JabbR.Client.Models;
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
        private LoadingStates loadingState;
        public ChatRoomView()
        {
            InitializeComponent();
            this.DataContextChanged += OnDataContextChanged;
        }


        public ChatRoomViewModel ChatRoom
        {
            get { return this.chatRoom; }
            private set
            {
                if (this.chatRoom != value)
                {
                    if (this.chatRoom != null) this.chatRoom.PropertyChanged -= OnChatRoomPropertyChanged;
                    this.chatRoom = value;
                    if (this.chatRoom != null) this.chatRoom.PropertyChanged += OnChatRoomPropertyChanged;
                }
            }
        }

        private void UserFilter(object sender, FilterEventArgs e)
        {
            UserViewModel userViewModel = (UserViewModel)e.Item;
            
            if (userViewModel.Status == UserStatus.Offline)
            {
                e.Accepted = false;
            }
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.ChatRoom = (ChatRoomViewModel)e.NewValue;
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


        private void OnChatRoomPropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LoadingState")
            {
                VisualStateManager.GoToState(this, this.ChatRoom.LoadingState.ToString(), true);
            }
        }
    }
}

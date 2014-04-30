
using NabbR.ViewModels;
using NabbR.ViewModels.Chat;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace NabbR.Views.Chat
{
    [View(LobbyView.Uri)]
    [ViewModel(typeof(LobbyViewModel))]
    public partial class LobbyView
    {
        public const String Uri = "/chat/lobby";
        private LobbyViewModel lobbyViewModel;

        public LobbyView()
        {
            InitializeComponent();
            this.DataContextChanged += OnDataContextChanged;
        }

        public LobbyViewModel Lobby
        {
            get { return this.lobbyViewModel; }
            private set
            {
                if (this.lobbyViewModel != null) this.lobbyViewModel.PropertyChanged -= OnLobbyViewModelPropertyChanged;
                this.lobbyViewModel = value;
                if (this.lobbyViewModel != null) this.lobbyViewModel.PropertyChanged += OnLobbyViewModelPropertyChanged;
            }
        }

        private void OnFilter(Object sender, FilterEventArgs e)
        {
            LobbyRoomViewModel lvm = e.Item as LobbyRoomViewModel;

            if (lvm != null)
            {
                e.Accepted = !lvm.Closed || IncludeClosedRoomsCheckBox.IsChecked.GetValueOrDefault();
            }
        }
        private void OnIncludeClosedRoomsCheckChanged(Object sender, RoutedEventArgs e)
        {
        }
        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.Lobby = (LobbyViewModel)e.NewValue;
        }
        private void OnLobbyViewModelPropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LoadingState")
            {
                String stateName = String.Concat("Lobby", this.Lobby.LoadingState);
                VisualStateManager.GoToState(this, stateName, true);
            }
        }
    }
}

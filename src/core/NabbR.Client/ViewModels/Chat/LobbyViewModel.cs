using NabbR.Commands;
using NabbR.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace NabbR.ViewModels.Chat
{
    public class LobbyViewModel : ViewModelBase, INavigationAware
    {
        private LoadingStates loadingState;
        private readonly IJabbRContext jabbrContext;
        private IEnumerable<LobbyRoomViewModel> rooms;
        private IDelegateCommand<LobbyRoomViewModel> navigateToRoomCommand;

        public LobbyViewModel(IJabbRContext jabbrContext)
        {
            this.jabbrContext = jabbrContext;
        }

#if DEBUG
        public LobbyViewModel()
        {
            this.Rooms = new LobbyRoomViewModel[] {
                new LobbyRoomViewModel { Name = "AngularJS", Closed = false, Count = 0, Topic = "Angular Stuff" },
                new LobbyRoomViewModel { Name = "Some Other Room", Closed = false, Count = 0, Topic = "Basic room" },
                new LobbyRoomViewModel { Name = "general-chat", Closed = false, Count = 15, Topic = "A really really really long topic description that is just way too long" },
            };
        }
#endif
        public LoadingStates LoadingState
        {
            get { return this.loadingState; }
            set { this.Set(ref this.loadingState, value); }
        }
        public IEnumerable<RoomViewModel> MyRooms
        {
            get { return this.jabbrContext.Rooms; }
        }
        public IEnumerable<LobbyRoomViewModel> Rooms
        {
            get { return this.rooms; }
            set { this.Set(ref this.rooms, value); }
        }

        public IDelegateCommand<LobbyRoomViewModel> NavigateToRoomCommand
        {
            get { return this.navigateToRoomCommand ?? (this.navigateToRoomCommand = new DelegateCommand<LobbyRoomViewModel>(r => this.HandleNavigateToRoom(r))); }
        }

        private void HandleNavigateToRoom(LobbyRoomViewModel room)
        {
        }

        async void INavigationAware.Navigated(IDictionary<String, String> parameters)
        {
            await this.GetLobbyRooms();
        }

        private async Task GetLobbyRooms()
        {
            this.LoadingState = LoadingStates.Loading;
            this.Rooms = await this.jabbrContext.GetLobbyRooms();
            this.LoadingState = LoadingStates.Loaded;
        }
    }
}

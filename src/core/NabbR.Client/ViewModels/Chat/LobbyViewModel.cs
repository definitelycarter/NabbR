using NabbR.Services;
using System;
using System.Collections.Generic;
using System.Linq;
namespace NabbR.ViewModels.Chat
{
    public class LobbyViewModel : ViewModelBase, INavigationAware
    {
        private readonly IJabbRContext jabbrContext;
        private IEnumerable<LobbyRoomViewModel> rooms;

        public LobbyViewModel(IJabbRContext jabbrContext)
        {
            this.jabbrContext = jabbrContext;
        }

#if DEBUG
        public LobbyViewModel()
        {
            this.Rooms = new LobbyRoomViewModel[] {
                new LobbyRoomViewModel { Name = "AngularJS", Closed = false, Count = 0, Topic = "Angular Stuff" },
                new LobbyRoomViewModel { Name = "general-chat", Closed = false, Count = 15, Topic = "A really really really long topic description that is just way too long" },
            };
        }
#endif

        public IEnumerable<RoomViewModel> MyRooms
        {
            get { return this.jabbrContext.Rooms; }
        }

        public IEnumerable<LobbyRoomViewModel> Rooms
        {
            get { return this.rooms; }
            set { this.Set(ref this.rooms, value); }
        }

        void INavigationAware.Navigated(IDictionary<String, String> parameters)
        {
            this.GetLobbyRooms();
        }

        private void GetLobbyRooms()
        {
            this.jabbrContext.GetLobbyRooms()
                .ContinueWith(t =>
                {
                    this.Rooms = t.Result.OrderByDescending(r => r.Count);
                });
        }
    }
}

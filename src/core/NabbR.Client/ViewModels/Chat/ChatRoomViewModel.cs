using NabbR.Commands;
using NabbR.Events;
using NabbR.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NabbR.ViewModels.Chat
{
    public class ChatRoomViewModel : ChatRoomViewModelBase<RoomViewModel>, INavigationAware
    {
        private LoadingStates loadingState;

        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatRoomViewModel"/> class.
        /// </summary>
        /// <param name="jabbrContext">The jabbr context.</param>
        public ChatRoomViewModel(IJabbRContext jabbrContext,
                                 IEventAggregator eventAggregator)
            : base(jabbrContext)
        {
            this.eventAggregator = eventAggregator;
        }

#if DEBUG
        public ChatRoomViewModel()
            : base(null)
        {
            this.Room = new RoomViewModel
            {
                Name = "This is a design-time room name.",
                Topic = "This is a design-time room topic.",
                Welcome = "This is a design-time room welcome message.",
            };

            this.Room.Users.Add(new UserViewModel { Name = "definitelycarter", IsTyping = true, Hash = "0ca89936b4a526bc1ee7ad4c5eb6fcbe", Status = JabbR.Client.Models.UserStatus.Inactive });
            this.Room.Users.Add(new UserViewModel { Name = "davepermen.net", IsTyping = false, Hash = "6ad5f8c742f1e8ec61000e2b0900fc76", Status = JabbR.Client.Models.UserStatus.Active });

            this.Room.Messages.Add(new Chat.UserMessageViewModel { User = this.Room.Users[0], Content = "This is a design-time message!", MessageDateTime = DateTimeOffset.Now });
            this.Room.Messages.Add(new Chat.UserMessageViewModel { User = this.Room.Users[1], Content = "That message is really cool!", MessageDateTime = DateTimeOffset.Now });
            this.Room.Messages.Add(new Chat.MessageViewModel { Content = "bryan_the_bot as become inactive.", MessageDateTime = DateTime.Now });
        }
#endif

        public LoadingStates LoadingState
        {
            get { return this.loadingState; }
            set { this.Set(ref this.loadingState, value); }
        }

        public override String ComposedMessage
        {
            get { return base.ComposedMessage; }
            set
            {
                base.ComposedMessage = value;

                if (value != null)
                {
                    this.NotifyTyping();
                }
            }
        }

        /// <summary>
        /// Called when navigated.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        async void INavigationAware.Navigated(IDictionary<String, String> parameters)
        {
            String roomName;
            if (parameters.TryGetValue("room", out roomName))
            {
                this.LoadingState = LoadingStates.Loading;
                RoomViewModel roomViewModel = this.jabbrContext.Rooms.FirstOrDefault(r => r.Name == roomName);

                if (roomViewModel == null)
                {
                    roomViewModel = await this.jabbrContext.JoinRoom(roomName);
                }

                this.LoadingState = LoadingStates.Loaded;
                this.Room = roomViewModel;
                this.eventAggregator.Publish(new RoomActivated { Room = roomViewModel });
            }
        }

        private DateTime lastNotificationSent;

        private void NotifyTyping()
        {
            TimeSpan timeLastSent = DateTime.Now.Subtract(lastNotificationSent);
            if (this.Room != null && timeLastSent.TotalSeconds > 3)
            {
                lastNotificationSent = DateTime.Now;

                String roomName = this.Room.Name;
                this.jabbrContext.SetTyping(roomName);
            }
        }
    }
}

using JabbR.Client;
using JabbR.Client.Models;
using JabbR.Models;
using NabbR.Events;
using NabbR.ViewModels.Chat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NabbR.Services
{
    public class JabbRContext : IJabbRContext, IApplicationContext
    {
        private readonly IJabbRClient jabbrClient;
        private readonly IEventAggregator eventAggregator;
        private readonly SynchronizationContext _synchronizationContext = SynchronizationContext.Current;
        private readonly ObservableCollection<UserViewModel> users = new ObservableCollection<UserViewModel>();
        private readonly ObservableCollection<RoomViewModel> rooms = new ObservableCollection<RoomViewModel>();
        /// <summary>
        /// Initializes a new instance of the <see cref="JabbRContext"/> class.
        /// </summary>
        /// <param name="jabbrClient">The jabbr client.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <exception cref="System.ArgumentNullException">
        /// jabbrClient
        /// or
        /// eventAggregator
        /// </exception>
        public JabbRContext(IJabbRClient jabbrClient,
                            IEventAggregator eventAggregator)
        {
            if (jabbrClient == null) throw new ArgumentNullException("jabbrClient");
            if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");

            this.jabbrClient = jabbrClient;
            this.eventAggregator = eventAggregator;

            this.jabbrClient.JoinedRoom += OnRoomJoined;
            this.jabbrClient.MessageReceived += OnMessageReceived;
            this.jabbrClient.UserActivityChanged += OnUserActivityChanged;
            this.jabbrClient.UsersInactive += OnUsersInactive;
            this.jabbrClient.UserTyping += OnUserTypingChanged;
            this.jabbrClient.UserLeft += OnUserLeftRoom;
            this.jabbrClient.UserJoined += OnUserJoinedRoom;
        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <value>
        /// The user id.
        /// </value>
        public String UserId { get; private set; }
        /// <summary>
        /// Gets the rooms.
        /// </summary>
        /// <value>
        /// The rooms.
        /// </value>
        IEnumerable<RoomViewModel> IJabbRContext.Rooms
        {
            get { return this.rooms; }
        }
        /// <summary>
        /// Logs in the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        async Task<Boolean> IJabbRContext.LoginAsync(String username, String password)
        {
            try
            {
                LogOnInfo info = await this.jabbrClient.Connect(username, password);
                this.UserId = info.UserId;

                var roomInfoTasks = info.Rooms.Select(r => this.HandleRoomJoined(r));
                await Task.WhenAll(roomInfoTasks);
                return true;
            }
            catch (Exception)
            {
                // todo
            }
            return false;
        }
        Task<Boolean> IJabbRContext.SendMessage(String message, String roomName)
        {
            return this.jabbrClient.Send(new ClientMessage { Content = message, Room = roomName });
        }

        private async void OnRoomJoined(Room roomInfo)
        {
            await this.HandleRoomJoined(roomInfo);
        }

        private void OnUsersInactive(IEnumerable<User> users)
        {
            foreach (var user in users)
            {
                this.OnUserActivityChanged(user);
            }
        }
        private void OnUserActivityChanged(User user)
        {
            _synchronizationContext.InvokeIfRequired(() =>
                {
                    UserViewModel userVm = this.users.FirstOrDefault(u => u.Name == user.Name);

                    if (userVm == null)
                    {
                        userVm = user.AsViewModel();
                        this.users.Add(userVm);
                    }
                    else
                    {
                        userVm.Status = user.Status;
                    }
                });
        }
        private void OnUserJoinedRoom(User user, String roomName, Boolean arg3)
        {
            _synchronizationContext.InvokeIfRequired(() =>
                {
                    RoomViewModel roomVm = this.rooms.FirstOrDefault(r => r.Name == roomName);

                    if (roomVm != null)
                    {
                        UserViewModel userVm = this.users.FirstOrDefault(u => u.Name == user.Name);

                        if (userVm == null)
                        {
                            userVm = user.AsViewModel();
                            this.users.Add(userVm);
                        }

                        ChatUserViewModel chatUser = new ChatUserViewModel(userVm);
                        roomVm.Users.Add(chatUser);
                    }
                });
        }
        private void OnUserLeftRoom(User user, String roomName)
        {
            _synchronizationContext.InvokeIfRequired(() =>
            {
                RoomViewModel roomVm = this.rooms.FirstOrDefault(r => r.Name == roomName);

                if (roomVm != null)
                {
                    ChatUserViewModel userLeaving = roomVm.Users.FirstOrDefault(u => u.User.Name == user.Name);

                    if (userLeaving != null)
                    {
                        roomVm.Users.Remove(userLeaving);
                    }
                }
            });
        }
        private void OnUserTypingChanged(User user, string roomName)
        {
            _synchronizationContext.InvokeIfRequired(() =>
            {
                RoomViewModel roomVm = this.rooms.FirstOrDefault(r => r.Name == roomName);
                if (roomVm != null)
                {
                    ChatUserViewModel chatUser = roomVm.Users.FirstOrDefault(u => u.User.Name == user.Name);
                    chatUser.IsTyping = true;
                }
            });
        }
        private void OnMessageReceived(Message message, String roomName)
        {
            _synchronizationContext.InvokeIfRequired(() =>
                {
                    RoomViewModel room = this.rooms.FirstOrDefault(r => r.Name == roomName);

                    if (room != null)
                    {
                        this.HandleMessageReceived(message, room);
                    }
                });
        }

        private async Task HandleRoomJoined(Room roomInfo)
        {
            var room = await this.jabbrClient.GetRoomInfo(roomInfo.Name);
            RoomViewModel roomVm = room.AsViewModel();

            foreach (var user in room.Users)
            {
                UserViewModel userVm = this.users.FirstOrDefault(u => u.Name == user.Name);

                if (userVm == null)
                {
                    userVm = user.AsViewModel();
                    this.users.Add(userVm);
                }

                ChatUserViewModel chatUser = new ChatUserViewModel(userVm);
                roomVm.Users.Add(chatUser);
            }

            foreach (var message in room.RecentMessages)
            {
                this.HandleMessageReceived(message, roomVm);
            }

            this.rooms.Add(roomVm);
            this.eventAggregator.Publish(new JoinedRoom { Room = roomVm });

        }
        private void HandleMessageReceived(Message message, RoomViewModel room)
        {
            UserViewModel userVm = this.users.FirstOrDefault(u => u.Name == message.User.Name);
            if (userVm == null)
            {
                userVm = message.User.AsViewModel();
                this.users.Add(userVm);
            }

            MessageViewModel messageVm = new MessageViewModel(userVm, message);
            room.Messages.Add(messageVm);
        }
    }

    static class Extensions
    {
        public static void InvokeIfRequired(this SynchronizationContext context, Action action)
        {
            if (SynchronizationContext.Current == context)
            {
                action();
            }
            else
            {
                context.Send(o => action(), null);
            }
        }
    }
}

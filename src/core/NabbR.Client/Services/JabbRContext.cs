using JabbR.Client;
using JabbR.Client.Models;
using JabbR.Models;
using NabbR.Events;
using NabbR.ViewModels.Chat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NabbR.Services
{
    public class JabbRContext : IJabbRContext
    {
        private String userId;
        private String username;
        private readonly IJabbRClient jabbrClient;
        private readonly IEventAggregator eventAggregator;
        private readonly SynchronizationContext _synchronizationContext = SynchronizationContext.Current;
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
        }

        String IJabbRContext.UserId
        {
            get { return this.userId; }
        }
        String IJabbRContext.Username
        {
            get { return this.username; }
        }
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
                this.LogoutUser();
                LogOnInfo info = await this.jabbrClient.Connect(username, password);
                await this.LoginUser(info);
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

        Task<RoomViewModel> IJabbRContext.JoinRoom(String roomName)
        {
            TaskCompletionSource<RoomViewModel> tcs = new TaskCompletionSource<RoomViewModel>();

            NotifyCollectionChangedEventHandler handler = null;
            handler = (o, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
                {
                    foreach (RoomViewModel roomViewModel in e.NewItems)
                    {
                        if (String.Equals(roomViewModel.Name, roomName, StringComparison.OrdinalIgnoreCase))
                        {
                            this.rooms.CollectionChanged -= handler;
                            tcs.SetResult(roomViewModel);
                        }
                    }
                }
            };

            this.rooms.CollectionChanged -= handler;
            this.jabbrClient.JoinRoom(roomName);
            return tcs.Task;
        }

        async Task<IEnumerable<LobbyRoomViewModel>> IJabbRContext.GetLobbyRooms()
        {
            var scheduler = TaskScheduler.Default;
            var roomInfos = await this.jabbrClient.GetRooms();
            IList<LobbyRoomViewModel> rooms = new List<LobbyRoomViewModel>();

            foreach (Room roomInfo in roomInfos)
            {
                rooms.Add(roomInfo.AsLobbyRoomViewModel());
            }

            return rooms;
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
                    foreach (var room in this.rooms)
                    {
                        UserViewModel userVm = room.Users.FirstOrDefault(u => u.Name == user.Name);

                        if (userVm != null)
                        {
                            if (userVm.Status != user.Status)
                            {
                                userVm.Status = user.Status;
                                room.AddNotification(String.Format("{0} has become {1}.", user.Name, userVm.Status));
                            }
                        }
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
                        this.HandleUserJoiningRoom(roomVm, user);
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
                    UserViewModel userLeaving = roomVm.Users.FirstOrDefault(u => u.Name == user.Name);

                    if (userLeaving != null)
                    {
                        if (user.Name == this.username)
                        {
                            this.rooms.Remove(roomVm);
                        }
                        else
                        {
                            userLeaving.Status = UserStatus.Offline;
                            roomVm.AddNotification(String.Format("{0} has left {1}.", userLeaving.Name, roomVm.Name));
                        }
                    }
                }
            });
        }
        private void OnUserTypingChanged(User user, string roomName)
        {
            _synchronizationContext.InvokeIfRequired(() =>
            {
                RoomViewModel roomVm = this.rooms.FirstOrDefault(r => r.Name == roomName);
                UserViewModel userVm = roomVm.Users.FirstOrDefault(u => u.Name == user.Name);

                if (userVm != null)
                {
                    userVm.IsTyping = true;
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

        private Task HandleRoomJoined(Room roomInfo)
        {
            return this.jabbrClient.GetRoomInfo(roomInfo.Name)
                .ContinueWith(t =>
                {
                    _synchronizationContext.InvokeIfRequired(() =>
                        {
                            var room = t.Result;
                            RoomViewModel roomVm = room.AsViewModel();

                            foreach (var user in room.Users)
                            {
                                this.HandleUserJoiningRoom(roomVm, user);
                            }

                            foreach (var message in room.RecentMessages)
                            {
                                this.HandleMessageReceived(message, roomVm);
                            }

                            this.rooms.Add(roomVm);
                        });
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }
        private void HandleMessageReceived(Message message, RoomViewModel room)
        {
            UserViewModel userVm = room.Users.FirstOrDefault(u => u.Name == message.User.Name);
            if (userVm == null)
            {
                userVm = message.User.AsViewModel();
                userVm.Status = UserStatus.Offline;
                room.Users.Add(userVm);
            }

            MessageViewModel messageVm = new UserMessageViewModel(message, userVm);
            room.Messages.Add(messageVm);
        }
        private void HandleUserJoiningRoom(RoomViewModel room, User user)
        {
            UserViewModel userVm = room.Users.FirstOrDefault(u => u.Name == user.Name);
            if (userVm == null)
            {
                userVm = user.AsViewModel();
                room.Users.Add(userVm);
            }
            else
            {
                userVm.Status = user.Status;
            }
        }

        private void LogoutUser()
        {
            this.jabbrClient.JoinedRoom -= OnRoomJoined;
            this.jabbrClient.MessageReceived -= OnMessageReceived;
            this.jabbrClient.UserActivityChanged -= OnUserActivityChanged;
            this.jabbrClient.UsersInactive -= OnUsersInactive;
            this.jabbrClient.UserTyping -= OnUserTypingChanged;
            this.jabbrClient.UserLeft -= OnUserLeftRoom;
            this.jabbrClient.UserJoined -= OnUserJoinedRoom;

            this.rooms.Clear();
        }
        private async Task LoginUser(LogOnInfo logonInfo)
        {
            var myUserInfo = await this.jabbrClient.GetUserInfo();
            this.username = myUserInfo.Name;
            this.userId = logonInfo.UserId;

            this.jabbrClient.JoinedRoom += OnRoomJoined;
            this.jabbrClient.MessageReceived += OnMessageReceived;
            this.jabbrClient.UserActivityChanged += OnUserActivityChanged;
            this.jabbrClient.UsersInactive += OnUsersInactive;
            this.jabbrClient.UserTyping += OnUserTypingChanged;
            this.jabbrClient.UserLeft += OnUserLeftRoom;
            this.jabbrClient.UserJoined += OnUserJoinedRoom;

            foreach (var roomName in logonInfo.Rooms.Select(r => r.Name).OrderBy(r => r))
            {
                RoomViewModel room = new RoomViewModel { Name = roomName };
                this.RefreshRoomInfoAsync(room);
                this.rooms.Add(room);
                this.eventAggregator.Publish(new JoinedRoom { Room = room });
            }
        }
        private void RefreshRoomInfoAsync(RoomViewModel roomViewModel)
        {
            this.jabbrClient.GetRoomInfo(roomViewModel.Name)
                .ContinueWith(t =>
                {
                    Room room = t.Result;
                    _synchronizationContext.InvokeIfRequired(() =>
                        {
                            roomViewModel.Name = room.Name;
                            roomViewModel.Topic = room.Topic;
                            roomViewModel.Welcome = room.Welcome;

                            foreach (var user in room.Users)
                            {
                                this.HandleUserJoiningRoom(roomViewModel, user);
                            }

                            foreach (var message in room.RecentMessages)
                            {
                                this.HandleMessageReceived(message, roomViewModel);
                            }
                        });
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
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
                context.Post(o => action(), null);
            }
        }
    }
}

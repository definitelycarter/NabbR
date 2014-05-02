using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;
using NabbR.Controls;
using NabbR.Events;
using NabbR.ViewModels;
using NabbR.ViewModels.Chat;
using NabbR.Views;
using NabbR.Views.Chat;
using System;
using System.Linq;
using System.Windows;

namespace NabbR.Client
{
    [View(Shell.Uri)]
    [ViewModel(typeof(ShellViewModel))]
    partial class Shell : ModernWindow, IHandle<JoinedRoom>, IHandle<LeftRoom>, IHandle<NavigateToRoom>, IHandle<MessageReceived>
    {
        const String Uri = "/";
        private ShellViewModel viewModel;

        public Shell(IEventAggregator eventAggregator)
            : this()
        {
            eventAggregator.Subscribe(this);
        }
        public Shell()
        {
            InitializeComponent();
            this.DataContextChanged += OnDataContextChanged;
        }

        public ShellViewModel ViewModel
        {
            get { return this.viewModel; }
            private set { this.viewModel = value; }
        }

        private void OnDataContextChanged(Object sender, DependencyPropertyChangedEventArgs e)
        {
            this.ViewModel = (ShellViewModel)e.NewValue;
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call System.Windows.FrameworkElement.ApplyTemplate().
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ModernFrame frame = (ModernFrame)this.Template.FindName("ContentFrame", this);

            frame.Navigated += OnNavigated;
            // critical to make navigation work
            frame.KeepContentAlive = false;
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            var link = this.chatGroup.Links.OfType<RoomLink>().FirstOrDefault(r => r.Source == e.Source);

            if (link != null)
            {
                link.UnreadCount = 0;
            }
        }

        public void Handle(LeftRoom message)
        {
            var leftRoom = message.Room;
            var link = this.chatGroup.Links.OfType<RoomLink>().FirstOrDefault(r => r.RoomName == leftRoom.Name);
            this.chatGroup.Links.Remove(link);

            var navigationLink = this.chatGroup.Links.FirstOrDefault();
            this.ContentSource = navigationLink.Source;
        }
        public void Handle(JoinedRoom message)
        {
            var room = message.Room;
            var roomUri = String.Format("/chatroom?room={0}", room.Name);
            var link = new RoomLink { RoomName = room.Name, DisplayName = room.Name, Source = new Uri(roomUri, UriKind.RelativeOrAbsolute) };
            this.chatGroup.Links.Add(link);
        }

        public void Handle(NavigateToRoom message)
        {
            var roomUri = String.Format("/chatroom?room={0}", message.RoomName);
            this.ContentSource = new Uri(roomUri, UriKind.RelativeOrAbsolute);
        }

        public void Handle(MessageReceived message)
        {
            String roomName = message.RoomName;
            RoomViewModel activeRoom = this.ViewModel.ActiveRoom;

            if (activeRoom == null ||
                activeRoom.Name != message.RoomName)
            {
                var roomLink = this.chatGroup.Links.OfType<RoomLink>().FirstOrDefault(r => r.RoomName == roomName);
                if (roomLink != null)
                {
                    roomLink.UnreadCount++;
                }
            }
            
            mediaElement.Position = TimeSpan.Zero;
            mediaElement.Play();
        }
    }
}

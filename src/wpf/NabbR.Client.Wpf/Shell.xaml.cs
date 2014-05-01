using FirstFloor.ModernUI.Windows.Controls;
using NabbR.Controls;
using NabbR.Events;
using NabbR.ViewModels;
using NabbR.Views;
using System;
using System.Linq;
using System.Windows.Controls;

namespace NabbR.Client
{
    [View(Shell.Uri)]
    [ViewModel(typeof(ShellViewModel))]
    partial class Shell : ModernWindow, IHandle<JoinedRoom>, IHandle<LeftRoom>, IHandle<NavigateToRoom>, IHandle<MessageReceived>
    {
        const String Uri = "/";

        public Shell(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            eventAggregator.Subscribe(this);
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call System.Windows.FrameworkElement.ApplyTemplate().
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ModernFrame frame = (ModernFrame)this.Template.FindName("ContentFrame", this);

            // critical to make navigation work
            frame.KeepContentAlive = false;
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
            mediaElement.Position = TimeSpan.Zero;
            mediaElement.Play();
        }
    }
}

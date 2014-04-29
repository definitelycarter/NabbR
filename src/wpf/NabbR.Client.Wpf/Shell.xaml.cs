using FirstFloor.ModernUI.Windows.Controls;
using NabbR.Controls;
using NabbR.ViewModels;
using NabbR.ViewModels.Chat;
using NabbR.Views;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace NabbR.Client
{
    [View(Shell.Uri)]
    [ViewModel(typeof(ShellViewModel))]
    partial class Shell : ModernWindow
    {
        const String Uri = "/";

        public Shell()
        {
            InitializeComponent();
            this.DataContextChanged += OnDataContextChanged;
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call System.Windows.FrameworkElement.ApplyTemplate().
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ModernFrame frame = (ModernFrame)this.Template.FindName("ContentFrame", this);

            // critical to make navigation work
            // frame.KeepContentAlive = false;
            frame.Navigated += (o, e) =>
                {

                };
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ShellViewModel shellViewModel = e.NewValue as ShellViewModel;
            if (shellViewModel != null)
            {
                shellViewModel.Rooms.CollectionChanged += OnRoomsChanged;
            }
        }

        private void OnRoomsChanged(Object sender, NotifyCollectionChangedEventArgs e)
        {

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    foreach (RoomViewModel room in e.NewItems)
                    {
                        String uri = String.Format("/chatroom?room={0}", room.Name);
                        chatGroup.Links.Add(new RoomLink { RoomName = room.Name, DisplayName = room.Name, Source = new Uri(uri, UriKind.RelativeOrAbsolute) });
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems != null)
                {
                    foreach (RoomViewModel room in e.OldItems)
                    {
                        RoomLink link = this.chatGroup.Links.OfType<RoomLink>().FirstOrDefault(l => l.RoomName == room.Name);
                        chatGroup.Links.Remove(link);

                        var newLink = chatGroup.Links.FirstOrDefault();

                        if (newLink != null)
                        {
                            this.ContentSource = newLink.Source;
                        }
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                this.chatGroup.Links.Clear();
            }
        }

        static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            if (obj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }

                    T childItem = FindVisualChild<T>(child);
                    if (childItem != null) return childItem;
                }
            }
            return null;
        }
    }
}

using FirstFloor.ModernUI.Windows.Controls;
using JabbR.Client.Models;
using NabbR.ViewModels;
using NabbR.ViewModels.Chat;
using NabbR.Views;
using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media;

namespace NabbR.Client
{
    [View(Shell.Uri)]
    [ViewModel(typeof(ShellViewModel))]
    partial class Shell
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

            ModernFrame frame = FindVisualChild<ModernFrame>(this);
            // critical to make navigation work
            frame.KeepContentAlive = false;
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
            if (e.NewItems != null)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        foreach (RoomViewModel room in e.NewItems)
                        {
                            String uri = String.Format("/ChatRoom?room={0}", room.Name);
                            chatGroup.Links.Add(new FirstFloor.ModernUI.Presentation.Link { DisplayName = room.Name, Source = new Uri(uri, UriKind.RelativeOrAbsolute) });
                        }
                    }));
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

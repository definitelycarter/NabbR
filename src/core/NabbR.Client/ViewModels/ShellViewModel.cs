using NabbR.Events;
using NabbR.Services;
using NabbR.ViewModels.Chat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NabbR.ViewModels
{
    public class ShellViewModel : ViewModelBase, INavigationAware, IHandle<JoinedRoom>
    {
        private readonly IDialogService dialogService;
        private readonly ObservableCollection<RoomViewModel> rooms;

        public ShellViewModel(IDialogService dialogService,
                              IEventAggregator eventAggregator)
        {
            this.dialogService = dialogService;
            this.rooms = new ObservableCollection<RoomViewModel>();

            eventAggregator.Subscribe(this);
        }


        public ObservableCollection<RoomViewModel> Rooms
        {
            get { return this.rooms; }
        }

        void INavigationAware.Navigated(IDictionary<String, String> parameters)
        {
            this.dialogService.Show<LoginViewModel>("/login", 
                (vm, dialogResult) =>
                {
                    
                }
            );
        }

        void IHandle<JoinedRoom>.Handle(JoinedRoom message)
        {
            this.Rooms.Add(message.Room);
        }
    }
}

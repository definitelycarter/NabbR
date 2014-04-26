using NabbR.Events;
using NabbR.Services;
using NabbR.ViewModels.Chat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NabbR.ViewModels
{
    public class ShellViewModel : ViewModelBase, INavigationAware
    {
        private readonly IJabbRContext jabbrContext;
        private readonly IDialogService dialogService;

        public ShellViewModel(IJabbRContext jabbrContext,
                              IDialogService dialogService)
        {
            this.jabbrContext = jabbrContext;
            this.dialogService = dialogService;
        }

        public ObservableCollection<RoomViewModel> Rooms
        {
            get { return this.jabbrContext.Rooms; }
        }

        void INavigationAware.Navigated(IDictionary<String, String> parameters)
        {
            this.dialogService.Show<LoginViewModel>("/login", 
                (vm, dialogResult) =>
                {
                    
                }
            );
        }
    }
}

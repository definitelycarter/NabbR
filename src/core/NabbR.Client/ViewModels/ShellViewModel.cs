using NabbR.Events;
using NabbR.Security;
using NabbR.Services;
using NabbR.ViewModels.Chat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NabbR.ViewModels
{
    public class ShellViewModel : ViewModelBase, INavigationAware, IHandle<RoomActivated>
    {
        private RoomViewModel activeRoom;
        private readonly IJabbRContext jabbrContext;
        private readonly ICredentialManager credentialManager;

        public ShellViewModel(IJabbRContext jabbrContext,
                              IEventAggregator eventAggregator,
                              ICredentialManager credentialManager)
        {
            this.jabbrContext = jabbrContext;
            this.credentialManager = credentialManager;

            eventAggregator.Subscribe(this);
        }

        public RoomViewModel ActiveRoom
        {
            get { return this.activeRoom; }
            private set { this.Set(ref this.activeRoom, value); }
        }
        public IEnumerable<RoomViewModel> Rooms
        {
            get { return this.jabbrContext.Rooms; }
        }

        async void INavigationAware.Navigated(IDictionary<String, String> parameters)
        {
            await this.AuthenticateUserAsync();
        }


        private async Task AuthenticateUserAsync()
        {
            Int32 retry = 0;
            while (retry < 3)
            {
                var credentials = await this.credentialManager.GetCredentials("jabbr:credentials");
                if (credentials != null)
                {
                    var loggedIn = await this.jabbrContext.LoginAsync(credentials.Username, credentials.Password);

                    if (loggedIn)
                    {
                        return;
                    }
                }
                retry++;
            }
        }

        public void Handle(RoomActivated message)
        {
            this.ActiveRoom = message.Room;
        }
    }
}

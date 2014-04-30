using NabbR.Security;
using NabbR.Services;
using NabbR.ViewModels.Chat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace NabbR.ViewModels
{
    public class ShellViewModel : ViewModelBase, INavigationAware
    {
        private readonly IJabbRContext jabbrContext;
        private readonly ICredentialManager credentialManager;

        public ShellViewModel(IJabbRContext jabbrContext,
                              ICredentialManager credentialManager)
        {
            this.jabbrContext = jabbrContext;
            this.credentialManager = credentialManager;
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
    }
}

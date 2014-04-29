using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.Security.Credentials.UI;

namespace NabbR.Security
{
    public class CredentialManager : ICredentialManager
    {
        private readonly PasswordVault passwordVault;

        public CredentialManager()
        {
            this.passwordVault = new PasswordVault();
        }

        public async Task<CredentialResult> GetCredentials(String resource)
        {
            CredentialResult credentials = this.GetStoredCredentials(resource);

            if (credentials == null)
            {
                credentials = await PromptForCredentials(resource);
            }

            return credentials;
        }

        private CredentialResult GetStoredCredentials(String resource)
        {
            var credentials = this.passwordVault.RetrieveAll();

            if (credentials.Count > 0)
            {
                var credential = credentials.FirstOrDefault(c => c.Resource == resource);

                if (credential != null)
                {
                    credential.RetrievePassword();
                    return new CredentialResult(credential.UserName, credential.Password);
                }
            }

            return null;
        }

        private async Task<CredentialResult> PromptForCredentials(String resource)
        {
            CredentialResult credential = null;

            CredentialPickerOptions options = new CredentialPickerOptions
            {
                Message = "Your credentials will be used to connect to JabbR.",
                Caption = "Please enter your JabbR username and password.",
                CredentialSaveOption = CredentialSaveOption.Unselected,
                AuthenticationProtocol = 0,
                TargetName = resource,
            };

            var result = await CredentialPicker.PickAsync(options);

            if (result.ErrorCode == 0)
            {
                credential = new CredentialResult(result.CredentialUserName, result.CredentialPassword);
            }

            return credential;
        }
    }
}

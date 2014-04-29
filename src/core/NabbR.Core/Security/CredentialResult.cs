using System;

namespace NabbR.Security
{
    public class CredentialResult
    {
        public CredentialResult(String username, String password)
        {
            this.Username = username;
            this.Password = password;
        }

        public String Username { get; private set; }
        public String Password { get; private set; }
    }
}

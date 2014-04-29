using System;
using System.Threading.Tasks;

namespace NabbR.Security
{
    public interface ICredentialManager
    {
        Task<CredentialResult> GetCredentials(String resource);
    }
}

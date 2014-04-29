using NabbR.Services;
using NabbR.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NabbR.Security
{
    class CredentialManager : ICredentialManager
    {
        private readonly IDialogService dialogService;

        public CredentialManager(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public Task<CredentialResult> GetCredentials(String resource)
        {
            TaskCompletionSource<CredentialResult> source = new TaskCompletionSource<CredentialResult>();

            this.dialogService.Show<LoginViewModel>("/login", (vm, result) =>
                {
                    CredentialResult credential = null;

                    if (result)
                    {
                        credential = new CredentialResult(vm.Username, vm.Password);
                    }

                    source.SetResult(credential);
                });

            return source.Task;
        }
    }
}

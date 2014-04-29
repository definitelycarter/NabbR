using NabbR.Commands;
using NabbR.Services;
using System;
using System.Collections.Generic;

namespace NabbR.ViewModels
{
    public class LoginViewModel : DialogViewModel
    {
        private String username;
        private String password;
        private Boolean isLoggingIn;
        private IDelegateCommand loginCommand;
        private readonly IJabbRContext jabbrContext;

        /// <summary>
        /// Creates a new instance of a <see cref="LoginViewModel" />.
        /// </summary>
        /// <param name="jabbrContext">The jabbr context.</param>
        public LoginViewModel(IJabbRContext jabbrContext)
        {
            this.jabbrContext = jabbrContext;
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public String Username
        {
            get { return this.username; }
            set
            {
                if (this.Set(ref this.username, value))
                {
                    this.LoginCommand.RaiseCanExecuteChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public String Password
        {
            get { return this.password; }
            set
            {
                if (this.Set(ref this.password, value))
                {
                    this.LoginCommand.RaiseCanExecuteChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets a boolean indiciating whether or not the user is logging in.
        /// </summary>
        public Boolean IsLoggingIn
        {
            get { return this.isLoggingIn; }
            private set
            {
                if (this.Set(ref this.isLoggingIn, value))
                {
                    this.LoginCommand.RaiseCanExecuteChanged();
                }
            }
        }
        /// <summary>
        /// Gets the login command.
        /// </summary>
        public IDelegateCommand LoginCommand
        {
            get { return this.loginCommand ?? (this.loginCommand = new DelegateCommand(this.HandleLogin, this.CanLogin)); }
        }

        private void HandleLogin()
        {
            this.DialogResult = true;
        }
        private Boolean CanLogin()
        {
            return !this.IsLoggingIn
                && !String.IsNullOrWhiteSpace(this.Username) 
                && !String.IsNullOrWhiteSpace(this.Password);
        }

        public override IEnumerable<CommandViewModel> GetAvailableActions(ViewType viewType)
        {
            yield return CommandViewModel.CreateDefault("Login", this.LoginCommand);
        }
    }
}

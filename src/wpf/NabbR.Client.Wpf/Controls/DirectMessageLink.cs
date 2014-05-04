using FirstFloor.ModernUI.Presentation;
using System;

namespace NabbR.Controls
{
    class DirectMessageLink : Link
    {
        private String username;

        /// <summary>
        /// Gets or sets the username that sent the direct message.
        /// </summary>
        public String Username
        {
            get { return this.username; }
            set
            {
                if (username != value)
                {
                    username = value;
                    this.UpdateDisplayName();
                }
            }
        }

        private void UpdateDisplayName()
        {
            // todo make displayname show unread messages.
            this.DisplayName = this.Username;
        }
    }
}

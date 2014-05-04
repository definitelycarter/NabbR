
using JabbR.Client.Models;
using System;

namespace NabbR.ViewModels.Chat
{
    public class MessageViewModel : ViewModelBase
    {
        private String content;
        private DateTimeOffset messageDateTime;

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public String Content
        {
            get { return this.content; }
            set { this.Set(ref this.content, value); }
        }
        /// <summary>
        /// Gets the message date time.
        /// </summary>
        /// <value>
        /// The message date time.
        /// </value>
        public DateTimeOffset MessageDateTime
        {
            get { return this.messageDateTime; }
            set { this.Set(ref this.messageDateTime, value); }
        }
    }
}

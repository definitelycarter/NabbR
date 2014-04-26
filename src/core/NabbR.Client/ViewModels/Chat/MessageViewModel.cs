
using JabbR.Client.Models;
using System;

namespace NabbR.ViewModels.Chat
{
    public class MessageViewModel : ViewModelBase
    {
        private readonly UserViewModel user;

        private readonly String content;
        private readonly Boolean isHtmlEncoded;
        private readonly DateTimeOffset messageDateTime;
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageViewModel"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MessageViewModel(UserViewModel user, Message message)
        {
            this.user = user;
            this.content = message.Content;
            this.messageDateTime = message.When;
            this.isHtmlEncoded = message.HtmlEncoded;
        }
        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public String Content
        {
            get {return this.content; }
        }
        public UserViewModel User
        {
            get { return this.user; }
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
        }
        public Boolean HtmlEncoded
        {
            get { return this.isHtmlEncoded; }
        }
    }
}

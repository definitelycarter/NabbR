
using JabbR.Client.Models;
using System;

namespace NabbR.ViewModels.Chat
{
    public class MessageViewModel : ViewModelBase
    {
        private String content;
        private Boolean isHtmlEncoded;
        private DateTimeOffset messageDateTime;
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageViewModel"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MessageViewModel(Message message)
        {
            this.content = message.Content;
            this.messageDateTime = message.When;
            this.isHtmlEncoded = message.HtmlEncoded;
        }
        public MessageViewModel()
        {

        }
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
        public Boolean HtmlEncoded
        {
            get { return this.isHtmlEncoded; }
            set { this.Set(ref this.isHtmlEncoded, value); }
        }
    }
}

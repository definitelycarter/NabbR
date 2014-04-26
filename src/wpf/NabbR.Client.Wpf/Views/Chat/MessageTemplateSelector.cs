using NabbR.ViewModels.Chat;
using System;
using System.Windows;
using System.Windows.Controls;

namespace NabbR.Views.Chat
{
    public class MessageTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Gets or sets the notification template.
        /// </summary>
        /// <value>
        /// The notification template.
        /// </value>
        public DataTemplate NotificationTemplate { get; set; }
        /// <summary>
        /// Gets or sets the direct message template.
        /// </summary>
        /// <value>
        /// The direct message template.
        /// </value>
        public DataTemplate DirectMessageTemplate { get; set; }
        /// <summary>
        /// Gets or sets the chat room message template.
        /// </summary>
        /// <value>
        /// The chat room message template.
        /// </value>
        public DataTemplate ChatRoomMessageTemplate { get; set; }
        
        /// <summary>
        /// When overridden in a derived class, returns a <see cref="T:System.Windows.DataTemplate" /> based on custom logic.
        /// </summary>
        /// <param name="item">The data object for which to select the template.</param>
        /// <param name="container">The data-bound object.</param>
        /// <returns>
        /// Returns a <see cref="T:System.Windows.DataTemplate" /> or null. The default value is null.
        /// </returns>
        public override DataTemplate SelectTemplate(Object item, DependencyObject container)
        {
            Type messageType = item.GetType();

            if (messageType == typeof(UserMessageViewModel))
            {
                return this.ChatRoomMessageTemplate;
            }
            else if (messageType == typeof(MessageViewModel))
            {
                return NotificationTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}

using NabbR.ViewModels.Chat;
using System;
using System.Windows;
using System.Windows.Controls;

namespace NabbR.Views.Chat.Templates
{
    public class MessageContentTemplateSelector : DataTemplateSelector
    {
        public override System.Windows.DataTemplate SelectTemplate(Object item, DependencyObject container)
        {
            MessageViewModel message = item as MessageViewModel;
            if (message != null)
            {
                
            }
            return base.SelectTemplate(item, container);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace NabbR.Controls
{
    public class ChatMessagesControl : ItemsControl
    {
        private const String ScrollViewerElementName = "ScrollViewer";

        private ScrollViewer scrollViewer;

        static ChatMessagesControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChatMessagesControl), new FrameworkPropertyMetadata(typeof(ChatMessagesControl)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.scrollViewer = (ScrollViewer)this.Template.FindName(ScrollViewerElementName, this);
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
            {
                this.scrollViewer.ScrollToBottom();
            }
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            this.scrollViewer.ScrollToBottom();
        }
    }
}

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

        public ChatMessagesControl()
        {
            this.ShouldScrollToBottom = true;
        }
        static ChatMessagesControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChatMessagesControl), new FrameworkPropertyMetadata(typeof(ChatMessagesControl)));
        }

        public Boolean ShouldScrollToBottom { get; private set; }

        public override void OnApplyTemplate()
        {
            this.scrollViewer = (ScrollViewer)this.Template.FindName(ScrollViewerElementName, this);
            this.scrollViewer.ScrollChanged += OnScrollChanged;
            base.OnApplyTemplate();
        }

        private void OnScrollChanged(Object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange > 0 && ShouldScrollToBottom)
            {
                this.scrollViewer.ScrollToBottom();
            }
            else if (this.ShouldScrollToBottom && (e.VerticalChange - e.ExtentHeightChange < 0))
            {
                this.ShouldScrollToBottom = false;
            }
            else if (!this.ShouldScrollToBottom && (e.VerticalOffset == this.scrollViewer.ScrollableHeight))
            {
                this.ShouldScrollToBottom = true;
            }
        }
    }
}

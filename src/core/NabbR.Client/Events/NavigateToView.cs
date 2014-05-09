
using System;
using System.Collections.Generic;

namespace NabbR.Events
{
    public class NavigateToView
    {
        public String Uri { get; set; }
    }

    public static class NavigateToViewExtensions
    {
        public static NavigateToView AsViewNavigationArgs(this String uri)
        {
            return new NavigateToView { Uri = uri };
        }

        public static void NavigateToView(this IEventAggregator eventAggregator, String uri)
        {
            eventAggregator.Publish(new NavigateToView { Uri = uri });
        }
    }
}

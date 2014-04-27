using FirstFloor.ModernUI.Windows;
using NabbR.ViewModels;
using NabbR.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace NabbR.Services
{
    class NinjectContentLoader : DefaultContentLoader
    {
        private IDependencyResolver resolver;
        private readonly ViewLocater viewLocater;
        /// <summary>
        /// Creates a new instance of a <see cref="NinjectContentLoader"/>.
        /// </summary>
        public NinjectContentLoader(ViewLocater viewLocater,
                                    IDependencyResolver resolver)
        {
            this.viewLocater = viewLocater;
            this.resolver = resolver;
        }
        /// <summary>
        /// Loads the content from specified uri.
        /// </summary>
        /// <param name="uri">The content uri</param>
        /// <returns>
        /// The loaded content.
        /// </returns>
        protected override Object LoadContent(Uri uri)
        {
            FrameworkElement view = null;

            var viewInfo = this.viewLocater.LocateViewInfo(uri);
            
            if (viewInfo != null)
            {
                var viewType = viewInfo.Item1;
                var viewModelType = viewInfo.Item2;
                var navigationParameters = viewInfo.Item3;

                view = (FrameworkElement)this.resolver.Get(viewType); // get the view.

                if (viewModelType != null)
                {
                    Object viewModel = this.resolver.Get(viewModelType);
                    
                    INavigationAware navigationAware = viewModel as INavigationAware;

                    if (navigationAware != null)
                    {
                        RoutedEventHandler handler = null;
                        handler = (o, e) =>
                            {
                                navigationAware.Navigated(navigationParameters);
                                view.Loaded -= handler;
                            };

                        view.Loaded += handler;
                    }
                    view.DataContext = viewModel;
                }
            }
            return view;
        }

    }
}

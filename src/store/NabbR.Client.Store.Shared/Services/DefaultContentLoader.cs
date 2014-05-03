using System;
using System.Collections.Generic;
using System.Text;
using NabbR.Views;
using Windows.UI.Xaml;
using NabbR.ViewModels;
namespace NabbR.Services
{
    class DefaultContentLoader
    {
        private readonly ViewLocater viewLocater;
        private readonly IDependencyResolver resolver;

        public DefaultContentLoader(ViewLocater viewLocater,
                                    IDependencyResolver resolver)
        {
            this.viewLocater = viewLocater;
            this.resolver = resolver;
        }

        public Object LoadContent(Uri uri)
        {
            var viewInfo = this.viewLocater.LocateViewInfo(uri);

            if (viewInfo != null)
            {
                var viewType = viewInfo.Item1;
                var viewModelType = viewInfo.Item2;
                var navigationParameters = viewInfo.Item3;
                return LoadContent(viewType, viewModelType, navigationParameters);
            }

            return null;
        }

        public Object LoadContent(Uri uri, Type viewModelType)
        {
            var viewModel = (ViewModelBase)this.resolver.Get(viewModelType);
            return LoadContent(uri, viewModel);
        }

        public Object LoadContent<T>(Uri uri, T viewModel) where T : ViewModelBase
        {
            FrameworkElement view = null;

            var viewInfo = this.viewLocater.LocateViewInfo(uri);

            if (viewInfo != null)
            {
                var viewType = viewInfo.Item1;
                var navigationParameters = viewInfo.Item3;

                view = (FrameworkElement)this.resolver.Get(viewType); // get the view.
                this.WireView(view, viewModel, navigationParameters);
            }

            return view;
        }

        private Object LoadContent(Type viewType, Type viewModelType, IDictionary<String, String> navigationParameters)
        {
            FrameworkElement view = (FrameworkElement)this.resolver.Get(viewType);
            ViewModelBase viewModel = (ViewModelBase)this.resolver.Get(viewModelType);
            this.WireView(view, viewModel, navigationParameters);
            return view;
        }

        private void WireView(FrameworkElement view, Object viewModel, IDictionary<String, String> navigationParameters)
        {
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
}

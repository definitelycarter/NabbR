using NabbR.Converters;
using NabbR.ViewModels;
using NabbR.Views;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;

namespace NabbR.Services
{
    class DialogService : IDialogService
    {
        Popup popup;
        private readonly IDependencyResolver resolver;
        private readonly DefaultContentLoader contentLoader;
        public DialogService(Shell shell, 
                             IDependencyResolver resolver,
                             DefaultContentLoader contentLoader)
        {
            popup = (Popup)shell.FindName("PopupDialog");

            this.resolver = resolver;
            this.contentLoader = contentLoader;
        }

        public void Show<T>(String uri, Action<T, Boolean> callback) where T : ViewModelBase
        {
            T viewModel = this.resolver.Get<T>();
            this.Show(uri, viewModel, callback);
        }

        public void Show<T>(String uri, T viewModel, Action<T, Boolean> callback) where T : ViewModelBase
        {
            FrameworkElement view = (FrameworkElement)this.contentLoader.LoadContent(new Uri(uri, UriKind.RelativeOrAbsolute), viewModel);

            DialogViewModel dialogViewModel = viewModel as DialogViewModel;
            popup.Child = view;


            // we need to do alot better here.  i.e. make a custom popup to handle dialogs.
            RoutedEventHandler loaded = null;
            loaded = (o, e) =>
                {
                    view.Loaded -= loaded;
                    popup.HorizontalOffset = (Window.Current.Bounds.Width - view.ActualWidth) / 2;
                    popup.VerticalOffset = (Window.Current.Bounds.Height - view.ActualHeight) / 2;
                };

            view.Loaded += loaded;

            if (dialogViewModel != null)
            {
                EventHandler<Object> closedHandler = null;
                closedHandler = (o, e) =>
                    {
                        popup.Closed -= closedHandler;
                        
                        popup.Child = null;
                        popup.ClearValue(Popup.IsOpenProperty);
                        callback(viewModel, dialogViewModel.DialogResult.GetValueOrDefault());
                    };

                popup.Closed += closedHandler;

                Binding binding = new Binding();
                binding.Path = new PropertyPath("DialogResult");
                binding.Source = dialogViewModel;
                binding.Converter = new NullablePropertyHasValueToBooleanConverter { BooleanValueForNull = true };
                popup.SetBinding(Popup.IsOpenProperty, binding);
            }
            else
            {
                popup.IsOpen = true;
            }
        }
    }
}

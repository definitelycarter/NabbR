using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using NabbR.Helpers;
using NabbR.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace NabbR.Services
{
    class WpfDialogService : IDialogService
    {
        private readonly IContentLoader contentLoader;
        private readonly IDependencyResolver serviceLocator;

        public WpfDialogService(IContentLoader contentLoader, 
                                IDependencyResolver serviceLocator)
        {
            if (contentLoader == null) throw new ArgumentNullException("contentLoader");
            if (serviceLocator == null) throw new ArgumentNullException("serviceLocator");

            this.contentLoader = contentLoader;
            this.serviceLocator = serviceLocator;
        }

        public void Show<T>(String uri, Action<T, Boolean> callback) where T : ViewModelBase
        {
            T viewModel = this.serviceLocator.Get<T>();
            this.Show(uri, viewModel, callback);
        }
        public void Show<T>(String uri, T viewModel, Action<T, Boolean> callback) where T : ViewModelBase
        {
            FrameworkElement view = (FrameworkElement)this.GetView(uri).Result;
            Window window = this.GetDialog(view, viewModel, callback);

            DialogViewModel dialogViewModel = viewModel as DialogViewModel;

            if (dialogViewModel != null)
            {
                Binding binding = new Binding("DialogResult");
                binding.Source = dialogViewModel;
                window.SetBinding(DialogHelper.DialogResultProperty, binding);

                EventHandler closedHandler = null;
                closedHandler = (o, e) =>
                    {
                        window.Closed -= closedHandler;
                        callback(viewModel, dialogViewModel.DialogResult.GetValueOrDefault());
                    };
                window.Closed += closedHandler;
            }
            window.Show();
        }

        private Window GetDialog<T>(FrameworkElement view, T viewModel, Action<T, bool> callback) where T : ViewModelBase
        {
            var commands = viewModel.GetAvailableActions(ViewModelBase.ViewType.Prompt);
            var window = new ModernDialog { Content = view, Buttons =  this.GetButtons(commands) };
            window.DataContext = viewModel;
            return window;
        }

        private Task<Object> GetView(String uri)
        {
            return contentLoader.LoadContentAsync(new Uri(uri, UriKind.RelativeOrAbsolute), CancellationToken.None);
        }

        private IEnumerable<Button> GetButtons(IEnumerable<CommandViewModel> commands)
        {
            DataTemplate template = (DataTemplate)Application.Current.TryFindResource("DialogButtonTemplate");
            
            foreach (var command in commands)
            {
                var button = (Button)template.LoadContent();
                button.DataContext = command;
                yield return button;
            }
        }
    }
}

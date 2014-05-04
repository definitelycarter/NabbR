using FirstFloor.ModernUI.Windows;
using JabbR.Client;
using NabbR.Events;
using NabbR.Security;
using NabbR.Services;
using NabbR.ViewModels;
using NabbR.ViewModels.Chat;
using NabbR.Views;
using Ninject;
using System.Collections.Generic;
using System.Reflection;

namespace NabbR.Client
{
    partial class App
    {
        IDependencyResolver resolver = null;
        private readonly IEnumerable<Assembly> assemblies = new[] {
            typeof(App).Assembly,
            typeof(ShellViewModel).Assembly,
            typeof(IDependencyResolver).Assembly,
        };

        private void Initialize()
        {
            var kernel = new StandardKernel();

            resolver = new NinjectDependencyResolver(kernel);
            kernel.Bind<IDependencyResolver>().ToMethod(_ => resolver);

            var viewLocater = new ViewLocater(assemblies);
            kernel.Bind<ViewLocater>().ToMethod(_ => viewLocater);

            var contentLoader = new NinjectContentLoader(viewLocater, resolver);
            kernel.Bind<IContentLoader>().ToMethod(_ => contentLoader);

            var jabbrClient = new JabbRClient("https://jabbr.net/"); // todo get the url from configuration.
            kernel.Bind<IJabbRClient>().ToMethod(_ => jabbrClient);

            kernel.Bind<IJabbRContext>().To<JabbRContext>().InSingletonScope();
            kernel.Bind<IDialogService>().To<WpfDialogService>().InSingletonScope();

            kernel.Bind<ICredentialManager>().To<CredentialManager>().InSingletonScope();
            kernel.Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();
        }
    }
}

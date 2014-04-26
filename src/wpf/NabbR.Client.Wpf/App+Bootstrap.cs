using FirstFloor.ModernUI.Windows;
using JabbR.Client;
using NabbR.Events;
using NabbR.Services;
using NabbR.ViewModels;
using NabbR.ViewModels.Chat;
using Ninject;
using System.Collections.Generic;
using System.Reflection;

namespace NabbR.Client
{
    partial class App
    {
        IServiceLocator serviceLocator = null;
        private readonly IEnumerable<Assembly> assemblies = new[] {
            typeof(App).Assembly,
            typeof(ShellViewModel).Assembly,
            typeof(IServiceLocator).Assembly,
        };

        private void Initialize()
        {
            var kernel = new StandardKernel();

            var locator = new NinjectServiceLocator(kernel);
            kernel.Bind<IServiceLocator>().ToMethod(_ => locator);
            
            var contentLoader = new NinjectContentLoader(locator, assemblies);
            kernel.Bind<IContentLoader>().ToMethod(_ => contentLoader);

            var jabbrClient = new JabbRClient("https://jabbr.net/"); // todo get the url from configuration.
            kernel.Bind<IJabbRClient>().ToMethod(_ => jabbrClient);

            kernel.Bind<IJabbRContext>().To<JabbRContext>().InSingletonScope();
            kernel.Bind<IEventAggregator>().To<EventAggregator>();
            kernel.Bind<IDialogService>().To<WpfDialogService>();

            serviceLocator = kernel.Get<IServiceLocator>();
        }
    }
}

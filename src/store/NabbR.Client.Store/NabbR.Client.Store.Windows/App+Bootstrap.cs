using JabbR.Client;
using NabbR.Services;
using NabbR.ViewModels;
using NabbR.Views;
using Ninject;
using System.Collections.Generic;
using System.Reflection;

namespace NabbR
{
    partial class App
    {
        private IDependencyResolver resolver = null;
        private readonly IEnumerable<Assembly> assemblies = new[] {
            typeof(App).GetTypeInfo().Assembly,
            typeof(ShellViewModel).GetTypeInfo().Assembly,
            typeof(IDependencyResolver).GetTypeInfo().Assembly,
        };

        private void Initialize()
        {
            var kernel = new StandardKernel();

            resolver = new NinjectDependencyResolver(kernel);
            kernel.Bind<IDependencyResolver>().ToMethod(_ => resolver);

            var viewLocater = new ViewLocater(resolver, assemblies);
            kernel.Bind<ViewLocater>().ToMethod(_ => viewLocater);

            var jabbrClient = new JabbRClient("https://jabbr.net/"); // todo get the url from configuration.
            kernel.Bind<IJabbRClient>().ToMethod(_ => jabbrClient);

            kernel.Bind<IJabbRContext>().To<JabbRContext>().InSingletonScope();
            kernel.Bind<DefaultContentLoader>().ToSelf().InSingletonScope();
            
            kernel.Bind<Shell>().ToSelf().InSingletonScope();
            kernel.Bind<IDialogService>().To<DialogService>();
        }
    }
}

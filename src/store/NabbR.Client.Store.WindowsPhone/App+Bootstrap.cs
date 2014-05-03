using NabbR.Services;
using NabbR.ViewModels;
using System.Collections.Generic;
using System.Reflection;

namespace NabbR
{
    partial class App
    {
        private readonly IEnumerable<Assembly> assemblies = new[] {
            typeof(App).GetTypeInfo().Assembly,
            typeof(ShellViewModel).GetTypeInfo().Assembly,
            typeof(IDependencyResolver).GetTypeInfo().Assembly,
        };

        private void Initialize()
        {
            // todo register phone services.
        }
    }
}

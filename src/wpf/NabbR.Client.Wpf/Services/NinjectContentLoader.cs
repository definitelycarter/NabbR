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
        private readonly IServiceLocator serviceLocator;
        private readonly IEnumerable<Assembly> assemblies;
        private readonly IDictionary<String, Type> registeredViews;

        /// <summary>
        /// Creates a new instance of a <see cref="NinjectContentLoader"/>.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="assemblies">The registered assemblies.</param>
        public NinjectContentLoader(IServiceLocator serviceLocator, IEnumerable<Assembly> assemblies)
        {
            this.serviceLocator = serviceLocator;
            this.assemblies = assemblies;

            IEnumerable<Type> types = assemblies.SelectMany(a => a.DefinedTypes).ToArray();

            this.registeredViews = (from viewType in types
                                    let viewAttribute = viewType.GetCustomAttribute<ViewAttribute>()
                                    where viewAttribute != null
                                    select new { viewAttribute.Uri, ViewType = viewType })
                                    .ToDictionary(a => a.Uri, a => a.ViewType);
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
            var pathAndQuery = this.GetPathAndQueryParameters(uri);

            Type viewType = this.registeredViews
                .Where(kv => kv.Key.Equals(pathAndQuery.Item1, StringComparison.InvariantCultureIgnoreCase))
                .Select(kv => kv.Value)
                .FirstOrDefault();

            if (viewType != null)
            {
                view = (FrameworkElement)this.serviceLocator.Get(viewType);
                
                ViewModelAttribute viewModelAttribute = viewType.GetCustomAttribute<ViewModelAttribute>();
                if (viewModelAttribute != null)
                {
                    Object viewModel = this.serviceLocator.Get(viewModelAttribute.ViewModelType);
                    
                    INavigationAware navigationAware = viewModel as INavigationAware;

                    if (navigationAware != null)
                    {
                        RoutedEventHandler handler = null;
                        handler = (o, e) =>
                            {
                                navigationAware.Navigated(pathAndQuery.Item2);
                                view.Loaded -= handler;
                            };

                        view.Loaded += handler;
                    }
                    view.DataContext = viewModel;
                }
            }
            return view;
        }

        private Tuple<String, IDictionary<String, String>> GetPathAndQueryParameters(Uri uri)
        {
            String path = null;
            IDictionary<String, String> query = new Dictionary<String, String>();
            Int32 queryDelimeter = uri.OriginalString.IndexOf('?');

            if (queryDelimeter > -1)
            {
                path = uri.OriginalString.Substring(0, queryDelimeter);
                var parts = uri.OriginalString.Substring(queryDelimeter + 1).Split('&');

                foreach (String part in parts)
                {
                    Int32 parameterDelimiter = part.IndexOf('=');
                    if (parameterDelimiter == -1)
                    {
                        throw new InvalidOperationException("The query parameters are invalid.");
                    }

                    String parameterName = part.Substring(0, parameterDelimiter);
                    String parameterValue = part.Substring(parameterDelimiter + 1);
                    query.Add(parameterName, parameterValue);
                }
            }
            else
            {
                path = uri.OriginalString;
                
            }

            return Tuple.Create(path, query);
        }
    }
}

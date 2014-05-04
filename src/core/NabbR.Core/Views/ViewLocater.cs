using NabbR.Services;
using NabbR.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NabbR.Views
{
    public class ViewLocater
    {
        private readonly IEnumerable<Assembly> assemblies;
        private readonly IDictionary<String, TypeInfo> registeredViews;

        public ViewLocater(IEnumerable<Assembly> assemblies)
        {
            this.assemblies = assemblies;

            IEnumerable<TypeInfo> types = assemblies.SelectMany(a => a.DefinedTypes).ToArray();

            this.registeredViews = (from viewType in types
                                    let viewAttribute = viewType.GetCustomAttribute<ViewAttribute>()
                                    where viewAttribute != null
                                    select new { viewAttribute.Uri, ViewType = viewType })
                                    .ToDictionary(a => a.Uri, a => a.ViewType);
        }

        public Tuple<Type, Type, IDictionary<String, String>> LocateViewInfo(Uri uri)
        {
            var pathAndQuery = this.GetPathAndQueryParameters(uri);

            TypeInfo viewType = this.registeredViews
                .Where(kv => kv.Key.Equals(pathAndQuery.Item1, StringComparison.OrdinalIgnoreCase))
                .Select(kv => kv.Value)
                .FirstOrDefault();

            if (viewType != null)
            {
                var viewModelType = this.LocateViewModelForView(viewType);
                return Tuple.Create(viewType.AsType(), viewModelType, pathAndQuery.Item2);
            }

            return null;
        }

        public Type LocateViewModelForView(Type viewType)
        {
            return this.LocateViewModelForView(viewType.GetTypeInfo());
        }

        private Type LocateViewModelForView(TypeInfo viewType)
        {
            ViewModelAttribute viewModelAttribute = viewType.GetCustomAttribute<ViewModelAttribute>();

            if (viewModelAttribute != null)
            {
                return viewModelAttribute.ViewModelType;
            }

            return null;
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

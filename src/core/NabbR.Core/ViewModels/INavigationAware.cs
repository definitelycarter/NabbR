using System;
using System.Collections.Generic;

namespace NabbR.ViewModels
{
    public interface INavigationAware
    {
        /// <summary>
        /// Called when navigated.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        void Navigated(IDictionary<String, String> parameters);
    }
}

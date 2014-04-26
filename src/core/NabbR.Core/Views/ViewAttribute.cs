using System;

namespace NabbR.Views
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ViewAttribute : Attribute
    {
        public ViewAttribute(String uri)
        {
            this.Uri = uri;
        }

        public String Uri { get; private set; }
    }
}

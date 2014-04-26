using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NabbR.ViewModels
{
    public class ViewModelAttribute : Attribute
    {
        public ViewModelAttribute(Type viewModelType)
        {
            this.ViewModelType = viewModelType;
        }

        public Type ViewModelType { get; private set; }
    }
}

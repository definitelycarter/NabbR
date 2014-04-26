using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NabbR.Services
{
    class NinjectServiceLocator : IServiceLocator
    {
        private readonly IKernel kernel;

        public NinjectServiceLocator(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public T Get<T>()
        {
            return this.kernel.Get<T>();
        }


        public Object Get(Type type)
        {
            return this.kernel.Get(type);
        }
    }
}

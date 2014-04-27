using Ninject;
using System;

namespace NabbR.Services
{
    class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel)
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
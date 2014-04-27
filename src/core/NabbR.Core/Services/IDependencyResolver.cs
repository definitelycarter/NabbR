
using System;
namespace NabbR.Services
{
    public interface IDependencyResolver
    {
        T Get<T>();
        Object Get(Type type);
    }
}

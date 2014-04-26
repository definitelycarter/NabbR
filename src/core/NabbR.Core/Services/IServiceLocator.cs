
using System;
namespace NabbR.Services
{
    public interface IServiceLocator
    {
        T Get<T>();
        Object Get(Type type);
    }
}


using System;
namespace NabbR.Events
{
    public interface IEventAggregator
    {
        void Publish<T>(T message);
        void Subscribe(Object target);
        void UnSubscribe(Object target);
    }
}

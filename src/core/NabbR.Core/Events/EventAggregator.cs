using System;
using System.Collections.Generic;
using System.Reflection;

namespace NabbR.Events
{
    public class EventAggregator : IEventAggregator
    {
        private static readonly List<WeakHandler> weakHandlers = new List<WeakHandler>();

        public void Publish<T>(T message)
        {
            lock (weakHandlers)
            {
                weakHandlers.RemoveAll(h => !h.Handler.IsAlive);

                foreach (WeakHandler weakHandler in weakHandlers)
                {
                    weakHandler.Send(message);
                }
            }
        }
        public void Subscribe(Object handler)
        {
            lock (weakHandlers)
            {
                weakHandlers.RemoveAll(h => h.Handler.Target == handler);
                weakHandlers.Add(new WeakHandler(handler));
            }
        }
        public void UnSubscribe(Object handler)
        {
            lock (weakHandlers)
            {
                weakHandlers.RemoveAll(h => h.Handler.Target == handler);
            }
        }

        class WeakHandler
        {
            private WeakReference handler;
            private Dictionary<Type, MethodInfo> supportedHandlers = new Dictionary<Type, MethodInfo>();

            public WeakHandler(Object target)
            {
                if (target == null) throw new ArgumentNullException("handler");
                this.handler = new WeakReference(target);

                this.GetSupportedHandlers(target.GetType());
            }

            public WeakReference Handler
            {
                get { return handler; }
            }

            public void Send<T>(T message)
            {
                Object target = this.Handler.Target;

                if (target != null)
                {
                    MethodInfo method;
                    if (supportedHandlers.TryGetValue(typeof(T), out method))
                    {
                        method.Invoke(target, new Object[] { message });
                    }
                }
            }

            private void GetSupportedHandlers(Type t)
            {
                IEnumerable<Type> interfaces = t.GetTypeInfo().ImplementedInterfaces;

                foreach (Type iface in interfaces)
                {
                    TypeInfo typeInfo = iface.GetTypeInfo();
                    if (typeInfo.IsGenericType)
                    {
                        Type genericTypeDefinition = typeInfo.GetGenericTypeDefinition();
                        if (genericTypeDefinition == typeof(IHandle<>))
                        {
                            MethodInfo method = typeInfo.GetDeclaredMethod("Handle");
                            Type messageType = typeInfo.GenericTypeArguments[0];
                            supportedHandlers[messageType] = method;
                        }
                    }
                }
            }
        }
    }
}

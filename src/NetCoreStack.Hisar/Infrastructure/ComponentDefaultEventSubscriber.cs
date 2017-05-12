using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetCoreStack.Hisar
{
    public sealed class ComponentDefaultEventSubscriber : IComponentEventSubscriber
    {
        private readonly Dictionary<Type, Delegate> _handlers;

        public ComponentDefaultEventSubscriber(object instance, string methodName = "Handle", BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException(nameof(methodName));
            }

            _handlers = new Dictionary<Type, Delegate>();

            var methods = instance.GetType().GetTypeInfo().GetMethods(bindingFlags)
                .Where(x => x.Name == methodName &&
                    x.GetParameters().Length == 1 &&
                    x.ReturnType == typeof(void));

            foreach (var method in methods)
            {
                var eventType = method.GetParameters()[0].ParameterType;
                var delegateType = typeof(Action<>).MakeGenericType(eventType);
                var @delegate = method.CreateDelegate(delegateType, instance);
                _handlers.Add(eventType, @delegate);
            }
        }
        
        public bool TryGetEventHandler<TEvent>(out Action<TEvent> handler)
        {
            Delegate @delegate;
            if (_handlers.TryGetValue(typeof(TEvent), out @delegate))
            {
                handler = (Action<TEvent>)@delegate;
                return true;
            }

            handler = null;
            return false;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;

namespace NetCoreStack.Hisar
{
    internal static class ActivatorExtensions
    {
        internal static TImplementation CreateInstance<TImplementation>(this ServiceDescriptor serviceDescriptor, params object[] args)
        {
            if (serviceDescriptor == null)
            {
                throw new ArgumentNullException(nameof(serviceDescriptor));
            }

            var implementationType = serviceDescriptor.ImplementationType;

            if (implementationType == null)
            {
                throw new ArgumentNullException(nameof(implementationType));
            }

            if (typeof(TImplementation).IsAssignableFrom(implementationType))
            {
                return (TImplementation)Activator.CreateInstance(implementationType, args);
            }

            throw new InvalidCastException(nameof(implementationType));
        }
    }
}

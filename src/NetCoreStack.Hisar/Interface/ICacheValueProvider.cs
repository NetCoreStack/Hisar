using NetCoreStack.Contracts;
using System;
using System.Linq.Expressions;

namespace NetCoreStack.Hisar
{
    public interface ICacheValueProvider
    {
        object TryGetValue<TModel>(object id, DateTimeOffset? absoluteExpiration = default(DateTimeOffset?));
    }
}
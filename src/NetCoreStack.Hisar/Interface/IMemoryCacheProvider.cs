using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Contracts;
using System;

namespace NetCoreStack.Hisar
{
    public interface IMemoryCacheProvider : IDisposable
    {
        IModelKeyGenerator ModelKeyGenerator { get; }

        bool TryGetValue(CacheItem key, out IModelKey<string> value);

        bool TryGetValue(CacheItem key, out IModelKey<long> value);

        TModel Set<TModel>(CacheItem key, TModel value);

        TModel GetOrCreate<TModel>(ActionContext context, long id) where TModel : IModelKey<long>;

        TModel GetOrCreate<TModel>(ActionContext context, string id) where TModel : IModelKey<string>;

        void Remove(params string[] keys);

        void Remove<TModel>(string id) where TModel : IModelKey<string>;

        void Remove<TModel>(long id) where TModel : IModelKey<long>;
    }
}

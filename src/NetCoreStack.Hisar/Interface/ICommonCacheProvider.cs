using NetCoreStack.Mvc.Types;
using System;
using System.Collections.Generic;

namespace NetCoreStack.Hisar
{
    public interface ICommonCacheProvider
    {
        List<TEntity> GetList<TEntity>();

        object GetObject(string key);

        object SetObject(string key, object value, CacheProviderOptions options);

        T GetObject<T>(string key);

        void Remove(string key);

        TEntity GetItem<TEntity>(Func<TEntity, bool> idSelector);

        // TODO EntityIdentity
        // TEntity GetItem<TEntity>(long? id) where TEntity : EntityIdentity;
    }
}

using NetCoreStack.Contracts;
using NetCoreStack.Mvc.Types;
using System;
using System.Collections.Generic;

namespace NetCoreStack.Hisar
{
    public interface ICommonCacheProvider
    {
        List<TEntity> GetList<TEntity>();

        object GetItem(string key);

        object SetItem(string key, object value, CacheProviderOptions options);

        void Remove(string key);

        TEntity GetItem<TEntity>(Func<TEntity, bool> idSelector);
        
        TEntity GetItem<TEntity>(string id) where TEntity : IEntityIdentity<string>;

        TEntity GetItem<TEntity, TKey>(TKey id) where TEntity : IEntityIdentity<TKey> where TKey : class;
    }
}

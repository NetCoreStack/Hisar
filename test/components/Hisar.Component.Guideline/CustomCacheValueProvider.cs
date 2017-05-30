using Hisar.Component.Guideline.Models;
using NetCoreStack.Data.Interfaces;
using NetCoreStack.Hisar;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Hisar.Component.Guideline
{
    public class CustomCacheValueProvider : ICacheValueProvider
    {
        private readonly IMongoUnitOfWork _unitOfWork;
        public CustomCacheValueProvider(IMongoUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public object TryGetValue<TModel>(ActionContext context, object id, CacheItem key)
        {
            if (typeof(TModel).Name == nameof(AlbumBson))
            {
                var idStr = id.ToString();
                key.AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(20));
                return _unitOfWork.Repository<AlbumBson>().FirstOrDefault(x => x.Id == idStr);
            }

            return default(TModel);
        }
    }
}

using Hisar.Component.Guideline.Models;
using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Data.Interfaces;
using NetCoreStack.Hisar;
using System;
using System.Linq;

namespace Hisar.Component.Guideline
{
    public class CustomCacheValueProvider : CacheValueProviderBase<AlbumBson>
    {
        private readonly IMongoUnitOfWork _unitOfWork;
        public CustomCacheValueProvider(IMongoUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public override AlbumBson TryGetValue(ActionContext context, object id, CacheItem key)
        {
            var idStr = id.ToString();
            key.AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(20));
            return _unitOfWork.Repository<AlbumBson>().FirstOrDefault(x => x.Id == idStr);
        }
    }
}

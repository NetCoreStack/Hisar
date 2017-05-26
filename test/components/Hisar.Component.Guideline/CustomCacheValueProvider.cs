using Hisar.Component.Guideline.Models;
using NetCoreStack.Data.Interfaces;
using NetCoreStack.Hisar;
using System;
using System.Linq;

namespace Hisar.Component.Guideline
{
    public interface IModel
    {

    }

    public class CustomCacheValueProvider : ICacheValueProvider
    {
        private readonly IMongoUnitOfWork _unitOfWork;
        public CustomCacheValueProvider(IMongoUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public object TryGetValue<TModel>(object id, DateTimeOffset? absoluteExpiration = default(DateTimeOffset?))
        {
            if (typeof(TModel).Name == nameof(AlbumBson))
            {
                var idStr = id.ToString();
                return _unitOfWork.Repository<AlbumBson>().FirstOrDefault(x => x.Id == idStr);
            }

            return default(TModel);
        }
    }
}

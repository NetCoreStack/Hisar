using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Data.Interfaces;
using NetCoreStack.Hisar;
using NetCoreStack.Mvc;
using NetCoreStack.Mvc.Extensions;
using System;
using System.Linq;

namespace Hisar.Component.ContentManagement
{
    public class ContentCacheValueProvider : CacheValueProviderBase<ContentObjectViewModel>
    {
        private readonly IMongoUnitOfWork _unitOfWork;
        private readonly IModelComposer<ContentObjectViewModel> _composer;
        public ContentCacheValueProvider(IMongoUnitOfWork unitOfWork, IModelComposer<ContentObjectViewModel> composer)
        {
            _unitOfWork = unitOfWork;
            _composer = composer;
        }

        public override ContentObjectViewModel TryGetValue(ActionContext context, object id, CacheItem key)
        {
            if (id == null)
                return null;

            key.AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(30));
            var str = id.ToString();
            var icerikViewModel = _unitOfWork.Repository<ContentObject>()
                .Where(x => x.Url == str)
                .Select(x => new ContentObjectViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Url = x.Url,
                }).FirstOrDefault();

            _composer.Invoke(icerikViewModel);
            return icerikViewModel;
        }
    }
}

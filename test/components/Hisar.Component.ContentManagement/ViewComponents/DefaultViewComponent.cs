using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Data.Interfaces;
using NetCoreStack.Hisar;
using System.Linq;
using System.Threading.Tasks;

namespace Hisar.Component.ContentManagement.ViewComponents
{
    public class DefaultViewComponent : HisarViewComponent
    {
        private readonly IMongoUnitOfWork _unitOfWork;

        public DefaultViewComponent(IMongoUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.CompletedTask;

            var contents = _unitOfWork.Repository<ContentObject>()
                .OrderByDescending(x => x.Id)
                .Take(3)
                .Select(x => new ContentObjectViewModel
                {
                    Title = x.Title,
                    Description = x.Description,
                    CreatedDate = x.CreatedDate,
                    Url = x.Url
                }).ToList();

            return View(contents);
        }
    }

}

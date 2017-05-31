using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using NetCoreStack.Data.Interfaces;
using NetCoreStack.Hisar;
using NetCoreStack.Mvc.Types;

namespace Hisar.Component.Guideline
{
    public class GuidelineExceptionFilter : IHisarExceptionFilter
    {
        protected IMongoUnitOfWork GetUnitOfWork(ActionContext context)
        {
            return ServiceProviderServiceExtensions.GetService<IMongoUnitOfWork>(context.HttpContext.RequestServices);
        }

        public void Invoke(ExceptionContext context, SystemLog systemLog)
        {
            using (var unitOfWork = GetUnitOfWork(context))
            {
                unitOfWork.Repository<SystemLog>().SaveAllChanges(systemLog);
            }
        }
    }
}

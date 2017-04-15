using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using NetCoreStack.Data.Interfaces;

namespace NetCoreStack.Hisar.Server
{
    public class HisarServerExceptionFilter : IHisarExceptionFilter
    {
        private IBsonUnitOfWork GetBsonUnitOfWork(ExceptionContext context)
        {
            return ServiceProviderServiceExtensions.GetService<IBsonUnitOfWork>(context.HttpContext.RequestServices);
        }

        public void Invoke(ExceptionContext context, SystemLog systemLog)
        {
            var bsonUnitOfWork = GetBsonUnitOfWork(context);
            bsonUnitOfWork.Repository<SystemLog>().Insert(systemLog);
        }
    }
}
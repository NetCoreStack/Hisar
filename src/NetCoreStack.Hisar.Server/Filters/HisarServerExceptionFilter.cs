using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using NetCoreStack.Data.Interfaces;

namespace NetCoreStack.Hisar.Server
{
    public class HisarServerExceptionFilter : IHisarExceptionFilter
    {
        private IMongoUnitOfWork GetUnitOfWork(ExceptionContext context)
        {
            return ServiceProviderServiceExtensions.GetService<IMongoUnitOfWork>(context.HttpContext.RequestServices);
        }

        public void Invoke(ExceptionContext context, SystemLog systemLog)
        {
            var bsonUnitOfWork = GetUnitOfWork(context);
            bsonUnitOfWork.Repository<SystemLog>().Insert(systemLog);
        }
    }
}
using Microsoft.AspNetCore.Mvc.Filters;

namespace NetCoreStack.Hisar
{
    public interface IHisarExceptionFilter
    {
        void Invoke(ExceptionContext context, SystemLog systemLog);
    }
}

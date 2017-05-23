using Microsoft.AspNetCore.Mvc.Filters;
using NetCoreStack.Mvc.Types;

namespace NetCoreStack.Hisar
{
    public interface IHisarExceptionFilter
    {
        void Invoke(ExceptionContext context, SystemLog systemLog);
    }
}

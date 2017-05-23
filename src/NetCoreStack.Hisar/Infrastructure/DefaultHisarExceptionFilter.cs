using Microsoft.AspNetCore.Mvc.Filters;
using NetCoreStack.Mvc.Types;

namespace NetCoreStack.Hisar
{
    public class DefaultHisarExceptionFilter : IHisarExceptionFilter
    {
        public DefaultHisarExceptionFilter()
        {

        }

        public void Invoke(ExceptionContext context, SystemLog systemLog)
        {
            
        }
    }
}

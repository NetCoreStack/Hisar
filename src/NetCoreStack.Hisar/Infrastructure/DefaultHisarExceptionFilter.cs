using Microsoft.AspNetCore.Mvc.Filters;

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

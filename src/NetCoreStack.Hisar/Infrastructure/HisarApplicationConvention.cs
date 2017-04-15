using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace NetCoreStack.Hisar
{
    public class HisarApplicationConvention : IApplicationModelConvention
    {
        public HisarApplicationConvention()
        {
        }

        public void Apply(ApplicationModel application)
        {
            var count = application.Controllers.Count;
        }
    }
}

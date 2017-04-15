using NetCoreStack.Hisar;
using System.Collections.Generic;

namespace Hisar.Component.Guideline.Filters
{
    public class GuidelineLayoutWebPackFilter : ILayoutFilter
    {

        public void PreInvoke(List<IPreWebPackObject> webpacks)
        {
        }

        public void PostInvoke(List<IPostWebPackObject> webpacks)
        {
            //webpacks.AddPostScripts(
            //    "/js/component.js"
            //);
        }
    }
}
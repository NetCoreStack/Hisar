using System.Collections.Generic;

namespace NetCoreStack.Hisar
{
    public interface ILayoutFilter
    {
        void PreInvoke(List<IPreWebPackObject> webpacks);

        void PostInvoke(List<IPostWebPackObject> webpacks);
    }
}
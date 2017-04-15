using System;
using System.Collections.Generic;

namespace NetCoreStack.Hisar
{
    public interface IEntityFilter
    {
        IList<Type> Invoke();
    }
}
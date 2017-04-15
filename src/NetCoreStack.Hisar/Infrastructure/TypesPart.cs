using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetCoreStack.Hisar
{
    public class TypesPart : ApplicationPart, IApplicationPartTypeProvider
    {
        public TypesPart(params Type[] types)
        {
            Types = types.Select(t => t.GetTypeInfo());
        }

        public override string Name => string.Join(", ", Types.Select(t => t.FullName));

        public IEnumerable<TypeInfo> Types { get; }
    }
}

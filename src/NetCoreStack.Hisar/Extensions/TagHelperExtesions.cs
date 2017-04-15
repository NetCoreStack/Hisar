using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace NetCoreStack.Hisar
{
    public static class TagHelperExtesions
    {
        public static TagHelperContext CreateTagHelperContext(this IWebPackObject webPack, 
            ReadOnlyTagHelperAttributeList allAttributes,
            IDictionary<object, object> items,
            string uniqueId)
        {
            var customAttributeList = new TagHelperAttributeList();
            var attributes = webPack.Resolve();
            foreach (KeyValuePair<string, object> entry in attributes)
            {
                customAttributeList.Add(new TagHelperAttribute(entry.Key, entry.Value));
            }

            foreach (var item in allAttributes)
            {
                if (!customAttributeList.ContainsName(item.Name))
                {
                    customAttributeList.Add(new TagHelperAttribute(item.Name, item.Value));
                }
            }

            return new TagHelperContext(customAttributeList, new Dictionary<object, object>(), uniqueId);
        }
    }
}
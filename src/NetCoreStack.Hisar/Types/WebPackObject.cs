using Microsoft.AspNetCore.Mvc.Rendering;
using NetCoreStack.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;

namespace NetCoreStack.Hisar
{
    public abstract class WebPackObject : IWebPackObject
    {
        protected const string HrefAttributeName = "href";
        protected const string RelAttributeName = "rel";
        protected const string SrcAttributeName = "src";
        protected const string AspFallback = "asp-fallback";
        
        public virtual WebPackSection Section { get; }
        public WebDecoratorNames Decorator { get; }
        public string Path { get; }
        public string FallbackPath { get; }
        public IDictionary<string, object> Attributes { get; }
        public bool AppendVersion { get; set; }

        public WebPackObject(WebDecoratorNames decorator,
            WebPackSection section,
            string path, 
            string fallbackPath,
            object attributes = null)
        {
            if (decorator == null)
            {
                throw new ArgumentNullException(nameof(decorator));
            }

            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (fallbackPath == null)
            {
                throw new ArgumentNullException(nameof(fallbackPath));
            }

            Decorator = decorator;
            Section = section;
            Path = path;
            FallbackPath = fallbackPath;
            Attributes = attributes?.ToDictionary() ?? new Dictionary<string, object>();
        }

        public virtual IDictionary<string, object> Resolve()
        {
            var values = new Dictionary<string, object>();
            // values.Add("asp-append-version", AppendVersion);
            var hrefOrSrc = SrcAttributeName;

            if (Decorator == WebDecoratorNames.Link)
            {
                hrefOrSrc = HrefAttributeName;
                values.Add(HrefAttributeName, Path);
            }
            else if(Decorator == WebDecoratorNames.Script)
            {
                hrefOrSrc = SrcAttributeName;
                values.Add(SrcAttributeName, Path);
            }

            // values.Add($"{AspFallback}-{hrefOrSrc}", FallbackPath);

            values.Merge(Attributes, true);
            return values;
        }

        public virtual void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            TagBuilder builder = new TagBuilder(Decorator.TagName);
            var attributes = Resolve();
            builder.MergeAttributes(attributes);
            builder.WriteTo(writer, encoder);
        }
    }
}

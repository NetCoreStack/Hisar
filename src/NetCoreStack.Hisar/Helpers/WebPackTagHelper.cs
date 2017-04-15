using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Net;

namespace NetCoreStack.Hisar
{
    [HtmlTargetElement("webpack")]
    public class WebPackTagHelper : TagHelper
    {
        private static readonly char[] NameSeparator = new[] { ',' };
        protected IHostingEnvironment HostingEnvironment { get; }

        protected ITagHelperActivator TagHelperActivator { get; }
        protected ILayoutFactory LayoutFactory { get; }

        public string Names { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public WebPackTagHelper(IHostingEnvironment hostingEnvironment,
            ITagHelperActivator activator,
            ILayoutFactory factory)
        {
            HostingEnvironment = hostingEnvironment;
            TagHelperActivator = activator;
            LayoutFactory = factory;
        }

        public override int Order => -1000;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            output.TagName = null;

            foreach (var pack in LayoutFactory.WebPacks)
            {
                var innerContext = pack.CreateTagHelperContext(context.AllAttributes, context.Items, context.UniqueId);
                if (pack.Decorator == WebDecoratorNames.Link)
                {
                    var linkTagHelper = TagHelperActivator.Create<LinkTagHelper>(ViewContext);
                    linkTagHelper.Process(innerContext, output);
                }
                else if(pack.Decorator == WebDecoratorNames.Script)
                {
                    var scriptTagHelper = TagHelperActivator.Create<ScriptTagHelper>(ViewContext);
                    scriptTagHelper.Process(innerContext, output);
                }

                output.Content.AppendHtml(WebUtility.HtmlEncode(Environment.NewLine));
            }
        }
    }
}

using Microsoft.AspNetCore.Html;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreStack.Hisar
{
    public class DefaultLayoutFactory : ILayoutFactory
    {
        private readonly ILayoutFilter _filter;
        public string FallbackRootPath => "/";
        public List<IWebPackObject> WebPacks { get; }

        public DefaultLayoutFactory(ILayoutFilter filter)
        {
            _filter = filter;
            WebPacks = new List<IWebPackObject>();
            PopulatePreWebPacks();
            PopulatePostWebPacks();
        }

        public void PopulatePreWebPacks()
        {
            var packs = new List<IPreWebPackObject>();

            packs.Add(new PreWebPackObject(WebDecoratorNames.Link, 
                $"{FallbackRootPath}favicon.ico",
                string.Empty,
               new { rel = "icon" }));

            packs.Add(new PreWebPackObject(WebDecoratorNames.Link,
               "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css",
               $"{FallbackRootPath}css/bootstrap.min.css",
               new { rel = "stylesheet" }));

            packs.Add(new PreWebPackObject(WebDecoratorNames.Link,
                "https://fonts.googleapis.com/css?family=Roboto+Condensed:300,300i,400,400i,700,700i&amp;subset=latin-ext",
                $"{FallbackRootPath}fonts/google-fonts.min.css",
                new { rel = "stylesheet" }));

            packs.Add(new PreWebPackObject(WebDecoratorNames.Link,
                "https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css",
                $"{FallbackRootPath}fonts/font-awesome.min.css",
                new { rel = "stylesheet" }));
            
            _filter?.PreInvoke(packs);
            WebPacks.AddRange(packs);
        }

        public void PopulatePostWebPacks()
        {
            var packs = new List<IPostWebPackObject>();
            _filter?.PostInvoke(packs);
            WebPacks.AddRange(packs);
        }

        public virtual IHtmlContent CreatePreWebPackSection()
        {
            var builder = new HtmlContentBuilder();
            var packs = WebPacks.Where(w => w.Section == WebPackSection.Pre).ToList();
            packs.ForEach(x => builder.AppendHtml(x));
            return builder;
        }

        public virtual IHtmlContent CreatePostWebPackSection()
        {
            var builder = new HtmlContentBuilder();
            var packs = WebPacks.Where(w => w.Section == WebPackSection.Post).ToList();
            packs.ForEach(x => builder.AppendHtml(x));
            return builder;
        }
    }
}
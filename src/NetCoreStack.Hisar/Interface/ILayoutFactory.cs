using Microsoft.AspNetCore.Html;
using System.Collections.Generic;

namespace NetCoreStack.Hisar
{
    public interface ILayoutFactory
    {
        string FallbackRootPath { get; }
        List<IWebPackObject> WebPacks { get; }
        IHtmlContent CreatePreWebPackSection();
        IHtmlContent CreatePostWebPackSection();
        void PopulatePreWebPacks();
        void PopulatePostWebPacks();
    }
}
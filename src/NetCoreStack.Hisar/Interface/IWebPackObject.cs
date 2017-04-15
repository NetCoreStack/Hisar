using Microsoft.AspNetCore.Html;
using System.Collections.Generic;

namespace NetCoreStack.Hisar
{
    public interface IWebPackObject : IHtmlContent
    {
        WebDecoratorNames Decorator { get; }
        string Path { get; }
        string FallbackPath { get; }
        IDictionary<string, object> Attributes { get; }
        WebPackSection Section { get; }
        bool AppendVersion { get; set; }
        IDictionary<string, object> Resolve();
    }

    public interface IPreWebPackObject : IWebPackObject
    {
    }

    public interface IPostWebPackObject : IWebPackObject
    {
    }
}
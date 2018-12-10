using System;

namespace NetCoreStack.Hisar
{
    public sealed class WebCliProxyInformation
    {
        private static readonly Lazy<WebCliProxyInformation> lazy =
            new Lazy<WebCliProxyInformation>(() => new WebCliProxyInformation());

        public static WebCliProxyInformation Instance { get { return lazy.Value; } }

        public string Address { get; set; } = "localhost:1444";

        public bool EnableLiveReload { get; set; }

        private WebCliProxyInformation()
        {
        }

        public Uri CreateBaseUri()
        {
            string suffix = string.Empty;
            if (!Address.EndsWith("/"))
            {
                suffix = "/";
            }

            if (Address.StartsWith("http"))
            {
                return new Uri(Address);
            }

            return new Uri($"http://{Address}{suffix}");
        }
    }
}

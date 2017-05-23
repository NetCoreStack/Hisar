using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NetCoreStack.Hisar.Server;
using NetCoreStack.Mvc;
using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace NetCoreStack.Hisar.Tests
{
    public class HomeController : HisarControllerServerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }

    public class HostingComponentTypeResolver : IComponentTypeResolver
    {
        public ComponentType Resolve(string componentId)
        {
            return ComponentType.Hosting;
        }
    }

    public class NameResolvers
    {
        private readonly IServiceProvider _resolver;
        private readonly HttpContext _context;
        private readonly ActionContext _actionContext;
        private readonly DefaultComponentTypeResolver _componentTypeResolver;

        public NameResolvers()
        {
            IServiceCollection services = new ServiceCollection();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            _componentTypeResolver = new DefaultComponentTypeResolver();

            services.AddNetCoreStackMvc();
            var componentHelper = new RunningComponentHelperOfT<Startup>(_componentTypeResolver);
            services.AddSingleton<RunningComponentHelper>(componentHelper);

            _resolver = services.BuildServiceProvider();
            _context = CreateHttpContext(_resolver, string.Empty);
            _actionContext = new ActionContext(_context, new RouteData(), new ActionDescriptor());
        }

        private HttpContext CreateHttpContext(IServiceProvider services, string appRoot)
        {
            var context = new DefaultHttpContext();
            context.RequestServices = services;

            context.Request.PathBase = new PathString(appRoot);
            context.Request.Host = new HostString("localhost");

            return context;
        }

        private HomeController CreateController()
        {
            var controller = new HomeController
            {
                ControllerContext = new ControllerContext(
                new ActionContext(
                    _context,
                    new RouteData(),
                    new ControllerActionDescriptor
                    {
                        ControllerTypeInfo = typeof(HomeController).GetTypeInfo()
                    }))
            };

            return controller;
        }

        [Fact]
        public void Resolve_External_ViewName()
        {
            var componentHelper = _resolver.GetService<RunningComponentHelper>();
            componentHelper = new RunningComponentHelperOfT<Startup>(_componentTypeResolver);

            var controller = CreateController();

            var viewName = controller.ResolveViewName("Index");
            Assert.True(viewName == "Index");
        }
    }
}

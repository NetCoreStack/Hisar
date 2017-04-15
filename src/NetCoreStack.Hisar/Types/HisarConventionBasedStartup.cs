using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Routing;
using System;
using System.Reflection;

namespace NetCoreStack.Hisar
{
    internal static class StartupTypeLoader
    {
        internal static HisarConventionBasedStartup CreateHisarConventionBasedStartup(Type startupType, IServiceProvider sp, IHostingEnvironment env)
        {
            var startupMethods = StartupLoader.LoadMethods(sp, startupType, env.EnvironmentName);
            return new HisarConventionBasedStartup(startupMethods, startupType, sp);
        }

        internal static MethodInfo GetConfigureRoutesMethod(Type startupType)
        {
            return startupType.GetMethod("ConfigureRoutes");
        }
    }

    internal class HisarConventionBasedStartup : ConventionBasedStartup
    {
        private readonly Type _startupType;
        private readonly IServiceProvider _sp;
        private readonly MethodInfo _configureRoutes;
        public Action<IRouteBuilder> ConfigureRoutesDelegate { get; }

        public HisarConventionBasedStartup(StartupMethods methods, Type startupType, IServiceProvider sp) 
            : base(methods)
        {
            _sp = sp;
            _startupType = startupType;
            _configureRoutes = StartupTypeLoader.GetConfigureRoutesMethod(_startupType);
            ConfigureRoutesDelegate = routes => ConfigureRoutes(routes);
        }

        public void ConfigureRoutes(IRouteBuilder routes)
        {
            try
            {
                _configureRoutes?.Invoke(null, new object[] { routes });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
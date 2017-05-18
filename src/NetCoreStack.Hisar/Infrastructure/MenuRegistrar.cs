using Microsoft.Extensions.DependencyInjection;

namespace NetCoreStack.Hisar
{
    public class MenuRegistrar<TStartup> where TStartup : class
    {
        private readonly IServiceCollection _services;

        public MenuRegistrar(IServiceCollection services)
        {
            _services = services;
        }

        public void Builder<TBuilder>() where TBuilder : DefaultMenuItemsBuilder<TStartup>
        {
            _services.AddScoped<IMenuItemsBuilder, TBuilder>();
        }
    }
}

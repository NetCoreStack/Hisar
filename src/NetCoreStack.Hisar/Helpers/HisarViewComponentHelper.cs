using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace NetCoreStack.Hisar
{
    public class HisarViewComponentHelper
    {
        private readonly IViewComponentHelper _componentHelper;

        public HisarViewComponentHelper(IViewComponentHelper componentHelper)
        {
            _componentHelper = componentHelper;
        }

        public Task<IHtmlContent> InvokeAsync(string name, object arguments)
        {
            try
            {
                return _componentHelper.InvokeAsync(name, arguments);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<IHtmlContent> InvokeAsync(Type componentType, object arguments)
        {
            try
            {
                return _componentHelper.InvokeAsync(componentType, arguments);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<IHtmlContent> InvokeAsync(string name)
        {
            try
            {
                return _componentHelper.InvokeAsync(name);
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public Task<IHtmlContent> InvokeAsync(Type componentType)
        {
            try
            {
                return _componentHelper.InvokeAsync(componentType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IHtmlContent> InvokeAsync<TComponent>(object arguments)
        {
            try
            {
                return _componentHelper.InvokeAsync<TComponent>(arguments);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IHtmlContent> InvokeAsync<TComponent>()
        {
            try
            {
                return _componentHelper.InvokeAsync<TComponent>();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

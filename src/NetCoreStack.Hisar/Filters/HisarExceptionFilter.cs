using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using NetCoreStack.Contracts;
using NetCoreStack.Mvc;
using NetCoreStack.Mvc.Exceptions;
using NetCoreStack.Mvc.Extensions;
using NetCoreStack.Mvc.Interfaces;
using System;
using System.IO;

namespace NetCoreStack.Hisar
{
    public class HisarExceptionFilter : ExceptionFilterAttribute
    {
        private IModelMetadataProvider GetModelMetadataProvider(ExceptionContext context)
        {
            return ServiceProviderServiceExtensions.GetService<IModelMetadataProvider>(context.HttpContext.RequestServices);
        }

        private IHostingEnvironment GetHostingEnvironment(ExceptionContext context)
        {
            return ServiceProviderServiceExtensions.GetService<IHostingEnvironment>(context.HttpContext.RequestServices);
        }

        private IHisarExceptionFilter GetHisarExceptionFilter(ExceptionContext context)
        {
            return ServiceProviderServiceExtensions.GetService<IHisarExceptionFilter>(context.HttpContext.RequestServices);
        }

        private IClaimsProvider GetClaimsProvider(ExceptionContext context)
        {
            return ServiceProviderServiceExtensions.GetService<IClaimsProvider>(context.HttpContext.RequestServices);
        }

        private string GetIp(ExceptionContext context)
        {
            return context?.HttpContext.Features?.Get<IHttpConnectionFeature>()?.RemoteIpAddress?.ToString();
        }

        private void CreateErrorViewResult(ExceptionContext context, string contentBody, string logGuid)
        {
            var result = new ViewResult { ViewName = "Error", StatusCode = StatusCodes.Status500InternalServerError };
            result.ViewData = new ViewDataDictionary(GetModelMetadataProvider(context), context.ModelState);

            var exceptionContextModel = new BasicExceptionContext
            {
                ExceptionType = context.Exception.GetType().FullName,
                ExceptionDetail = contentBody,
                LogGuid = logGuid,
                Message = ""
            };

            var env = GetHostingEnvironment(context);
            if (env.IsProduction())
                exceptionContextModel.ExceptionDetail = string.Empty;
            else
                exceptionContextModel.Message = context.Exception.Message;

            // Workaround for https://github.com/aspnet/Home/issues/1820
            context.HttpContext.Items.Add(nameof(BasicExceptionContext), exceptionContextModel);
            context.Result = result;
        }

        public override void OnException(ExceptionContext context)
        {
            var logGuid = Guid.NewGuid().ToString("N");
            var hostingEnvironment = GetHostingEnvironment(context);
            var contentBody = $"Unexpected error occurred!<br/> Error Code:{logGuid}";
            var isAjaxRequest = context.HttpContext.Request.IsAjaxRequest();
            var isProduction = hostingEnvironment.IsProduction();
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.ExceptionHandled = true; // mark exception as handled

            var exception = context.Exception;
            if (exception == null && context.Exception.InnerException != null)
                exception = context.Exception.InnerException;

            contentBody = $"{ExceptionFormatter.CreateMessage(exception)}{Environment.NewLine}Error Code:{logGuid}";

            if (isAjaxRequest)
            {
                if (isProduction)
                    context.Result = new JsonResult(new WebResult(content: $"Unexpected error occurred!<br/> Error Code: {logGuid}", resultState: ResultState.Error));
                else
                    context.Result = new JsonResult(new WebResult(content: contentBody, resultState: ResultState.Error));
            }
            else
                CreateErrorViewResult(context, contentBody, logGuid);

            context.Exception.Data.Add(nameof(logGuid), logGuid);
            try
            {
                var claimsProvider = GetClaimsProvider(context);
                var sysLog = new SystemLog()
                {
                    ObjectState = ObjectState.Added,
                    CreatedDate = DateTime.Now,
                    Message = contentBody,
                    Category = nameof(HisarExceptionFilter),
                    Level = (int)LogLevel.Critical,
                    UserId = claimsProvider?.UserId,
                    Ip = GetIp(context),
                    ErrorCode = logGuid
                };

                var filter = GetHisarExceptionFilter(context);
                filter.Invoke(context, sysLog);
            }
            catch (Exception ex)
            {
                var directory = Directory.CreateDirectory(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "fallback_logs"));
                var logFile = $"log-{DateTime.Now.Date.ToString("ddMMyyyy")}-{Guid.NewGuid()}.txt";
                var fullPath = Path.Combine(directory.FullName, logFile);
                File.AppendAllText(fullPath, contentBody);
            }
        }
    }
}
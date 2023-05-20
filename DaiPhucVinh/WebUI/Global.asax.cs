using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using AutoMapper;
using Falcon.Web.Core.Infrastructure;
using Newtonsoft.Json;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using FluentScheduler;
using DaiPhucVinh.Infrastructure;
using DaiPhucVinh.Services.Infrastructure;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.Tasks;
using DaiPhucVinh.Services.Framework;

namespace DaiPhucVinh
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            DefaultModelBinder.ResourceClassKey = "DaiPhucVinh.WebUIResource";
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            var config = new MapperConfiguration(
            cfg =>
            {
                cfg.AddProfile<AutoMapperExtendWebProfile>();
                cfg.AddProfile<AutoMapperApiProfile>();
            });
            AutoMapperExtension.Mapper = config.CreateMapper();
            var container = new Container();
            container.Options.DefaultScopedLifestyle = Lifestyle.CreateHybrid(new WebRequestLifestyle(), new AsyncScopedLifestyle());
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            DependencyRegistra.Register(container);
            WebRegistration.Register(container);
            container.Verify();
            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings =
                new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Local,
                };
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            JobManager.Initialize(new AppTaskSchedulers());
        }
        protected void Application_BeginRequest()
       {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }
        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            //log error
            LogException(exception);
        }
        protected void LogException(Exception exc)
        {
            if (exc == null)
                return;
            //ignore 404 HTTP errors
            var httpException = exc as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
                return;
            try
            {
                //log
                /*var logger = EngineContext.Current.Resolve<ILogger>();
                logger.Error(exc.Message, exc.ToString());*/
                var logger = EngineContext.Current.Resolve<ILogService>();
                logger.InsertLog(exc);

            }
            catch (Exception)
            {
                //don't throw new exception if occurs
            }
        }
    }
}
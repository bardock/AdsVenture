using System;
using System.ComponentModel;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AdsVenture.Commons.Helpers;
using AdsVenture.Core;
using System.Web;
using Bardock.Utils.Web;

namespace AdsVenture.Presentation.ContentServer
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static IServiceLocator ServiceLocator { get; private set; }

        protected void Application_Start()
        {
            TypeDescriptor.AddAttributes(
                typeof(DateTime), new TypeConverterAttribute(typeof(ModelBinders.UtcDateTimeConverter)));

            DependenciesConfig.Register(GlobalConfiguration.Configuration);
            ServiceLocator = DependencyResolver.Current.GetService<Commons.Helpers.IServiceLocator>();

            AreaRegistration.RegisterAllAreas();

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            MappingsConfig.Register();
            ModelBindersConfig.Register();
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Bootstrapper.Init(ServiceLocator);
        }

        protected virtual void Application_BeginRequest()
        {
        }

        protected virtual void Application_EndRequest()
        {
            if (HttpContext.Current.Response.IsRequestBeingRedirected)
            {
                RequestNotifications.Instance.PersistState();
            }
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
        }

        void Application_Error(object sender, EventArgs e)
        {
            Filters.ExceptionLogging.Log(Server.GetLastError());
        }
    }
}

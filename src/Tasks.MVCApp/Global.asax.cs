using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Spark.Web.Mvc;
using MvcExtensions.Services.Impl;
using Tasks.Core.Controllers;
using MvcExtensions.Services;
using Tasks.Core.Services.Impl;
using Tasks.Core.Services;
using Tasks.Core.Model;
using Castle.Core;

namespace Tasks.MVCApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);

            var ioc = new IOC();
            ioc.AddComponent<IRepository<Task>, FakeRepository<Task>>();
            ioc.AddComponentLifeStyle<IMiocService<HomeController>, HomeMioc>(LifestyleType.Transient);

            var fact = new MiocControllerFactory(ioc);
            ControllerBuilder.Current.SetControllerFactory(fact);
            ControllerBuilder.Current.DefaultNamespaces.Add("Tasks.Core.Controllers");

            ViewEngines.Engines.Add(new SparkViewFactory());
        }
    }
}
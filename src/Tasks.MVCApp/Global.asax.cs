using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Spark.Web.Mvc;
using Tasks.Core.Controllers;
using Tasks.Core.Services.Impl;
using Tasks.Core.Services;
using Tasks.Core.Model;
using Castle.Core;
using Castle.MicroKernel.Registration;
using MvcExtensions.Services;
using MvcExtensions.Services.Impl;

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

            var container = new MvcContainer();
            container.AddComponent<IRepository<Task>, FakeRepository<Task>>();
            container.AddComponent<IMvcCustomContainer<HomeController>, HomeContainer>();

            var fact = new MvcContainerControllerFactory(container);
            ControllerBuilder.Current.SetControllerFactory(fact);
            ControllerBuilder.Current.DefaultNamespaces.Add("Tasks.Core.Controllers");

            ViewEngines.Engines.Add(new SparkViewFactory());
        }
    }
}
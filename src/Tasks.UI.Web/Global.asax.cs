using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Spark.Web.Mvc;
using Tasks.Core.Controllers;
using Tasks.Core.Services;
using Tasks.Core.Model;
using Castle.Core;
using Castle.MicroKernel.Registration;
using MvcExtensions.Services;
using MvcExtensions.Services.Impl;
using MvcExtensions.Services.Impl.FluentNHibernate;
using System.IO;
using MvcExtensions.FNHModules.AuditInfo;
using MvcExtensions;
using MvcExtensions.Web;
using MvcExtensions.Web.Services;

namespace Tasks.UI.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default.aspx",                                              // Route name
                "{controller}.aspx/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );
            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );
        }

        private Database GetDb(IUsernameProvider prov)
        {
            var mappings = new MyDomainDefinition(prov);
            var fn = Server.MapPath("~/app_data/tasks.db");
            var db = new SqlLiteDatabase(fn, mappings);
            if (!File.Exists(fn))
                db.CreateDB();
            else
                db.UpdateDB();
            return db;
        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);

            var wunp = new HttpUsernameProvider(RouteTable.Routes);

            var module = new MvcWebExtensionsModule();

            module.Register(GetDb(wunp));

            module.Container.Register(
                Component.For<IMvcCustomContainer<HomeController>>().ImplementedBy<HomeContainer>().LifeStyle.Transient,
                Component.For<HttpUsernameProvider>().Instance(wunp)
            );

            ControllerBuilder.Current.DefaultNamespaces.Add("Tasks.Core.Controllers");
        }
    }
}
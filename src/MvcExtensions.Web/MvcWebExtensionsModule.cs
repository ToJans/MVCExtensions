using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.Model;
using MvcExtensions.Services;
using MvcExtensions.Services.Impl;
using MvcExtensions.Services.Impl.FluentNHibernate;
using MvcExtensions.Web.ModelBinders;
using System.Web.Mvc;
using Spark.Web.Mvc;
using MvcExtensions.Web.Services.Impl;

namespace MvcExtensions.Web
{
    public class MvcWebExtensionsModule : MvcExtensionsModule 
    {
        public void RegisterClassDerivedFromMyTextInModelBinder(Type t)
        {
            var binders = System.Web.Mvc.ModelBinders.Binders;
            var x = typeof(MyTextModelBinder<>).MakeGenericType(t);
            binders.Add(t, (IModelBinder)(Activator.CreateInstance(x)));
        }

        public override void Register(Database database) 
        {
            base.Register(database,true);
            var settings = new Spark.SparkSettings()
                .AddAssembly(this.GetType().Assembly)
                .SetAutomaticEncoding(true)
                .SetDebug(true)
                .AddNamespace(typeof(MvcExtensions.Web.Helpers.HTMLHelperExtensions).Namespace)
                .AddNamespace(typeof(MvcExtensions.Model.VMActionLink).Namespace);
            var spv = new SparkViewFactory(settings);
            var path = "MvcExtensions.Web.Views";
            spv.AddEmbeddedResources(this.GetType().Assembly,path);
            ViewEngines.Engines.Add(spv);

            var fact = new MvcContainerControllerFactory(Container);
            ControllerBuilder.Current.SetControllerFactory(fact);

            foreach (var t in typeof(MyText).Assembly.GetExportedTypes()
                .Where(tp => tp.Namespace == typeof(ShortText).Namespace 
                        && !tp.IsAbstract 
                        && typeof(MyText).IsAssignableFrom(tp)))
            {
                RegisterClassDerivedFromMyTextInModelBinder(t);
            }

        }
    }
}

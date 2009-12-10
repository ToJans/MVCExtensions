using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc ;
using System.Web;
using System.Globalization;
using System.Diagnostics;

namespace MvcExtensions.Services.Impl
{
    public class MiocControllerFactory : DefaultControllerFactory 
    {
        private IIOC _container;
        Func<Type,Mioc> GetMiocForControllerType;

        public MiocControllerFactory(IIOC container):base()
        {
            _container = container;
        }

        protected override IController  GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (typeof(IController).IsAssignableFrom(controllerType))
            {
                var t = typeof(IMiocService<>).MakeGenericType(controllerType);
                try
                {
                    var c = (Mioc)_container.Resolve(t);
                    c.AddComponent("controller",controllerType);
                    var ctrl = (System.Web.Mvc.Controller)c.Resolve(controllerType);
                    return ctrl;
                    
                } catch(Exception ex)
                {
                    Trace.WriteLine("Exception in resolving "+t.FullName);
                    Trace.WriteLine(ex.ToString());
                }
                return (IController)_container.Resolve(controllerType);
            }
            else
            {
                return base.GetControllerInstance(requestContext, controllerType);
            }
        }

        public override void ReleaseController(IController controller)
        {
            _container.Release(controller);
            base.ReleaseController(controller);
        }

    }
}

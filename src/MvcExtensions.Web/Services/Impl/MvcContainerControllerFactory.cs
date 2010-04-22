using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc ;
using System.Web;
using System.Globalization;
using System.Diagnostics;
using MvcExtensions.Services;
using MvcExtensions.Services.Impl;
using Castle.Core;

namespace MvcExtensions.Web.Services.Impl
{
    public class MvcContainerControllerFactory : DefaultControllerFactory 
    {
        private MvcContainer Container;

        public MvcContainerControllerFactory(MvcContainer container):base()
        {
            Container = container;
        }

        protected override IController  GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (typeof(IController).IsAssignableFrom(controllerType))
            {
                var t = typeof(IMvcCustomContainer<>).MakeGenericType(controllerType);
                if (Container.Kernel.HasComponent(t))
                {
                    var childContainer = Container.Resolve(t) as MvcContainer;
                    Container.AddChildContainer(childContainer);
                    if (!childContainer.Kernel.HasComponent(controllerType))
                        childContainer.AddComponentLifeStyle(controllerType.FullName, controllerType, LifestyleType.PerWebRequest);
                    var controller = (IController)childContainer.Resolve(controllerType);
                    Container.RemoveChildContainer(childContainer);
                    return controller;
                }
                return (IController)Container.Resolve(controllerType);
            }
            else
            {
                return null;
                return base.GetControllerInstance(requestContext, controllerType);
            }
        }


        public override void ReleaseController(IController controller)
        {
            Container.Release(controller);
            base.ReleaseController(controller);
        }

    }
}

using System;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace MvcExtensions.Services.Impl
{
    public class IOC : WindsorContainer, IIOC
    {
        public void RegisterGenericTypePerWebRequest(System.Type S, System.Type T)
        {
            this.Register(Component.For(S).ImplementedBy(T).LifeStyle.PerWebRequest);
        }

        public void RegisterTypesImplementingInterfaceTransient<IInterface, TClass>()
        {
            var ass = typeof(TClass).Assembly;

            Register(
                AllTypes
                    .FromAssembly(ass)
                    .Where(t => t.GetInterface(typeof(IInterface).Name)!=null)
                    .Configure(c => c.LifeStyle.Transient));
        }



        public void RegisterTypesWithBaseClassTransient<TBaseClass, TClass>()
        {
            var ass = typeof(TClass).Assembly;

            Register(
                AllTypes
                    .FromAssembly(ass)
                    .BasedOn<TBaseClass>()
                    .Configure(c => c.LifeStyle.Transient));
        }
    }
}

using System;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace MvcExtensions.Services.Impl
{
    public class MvcContainer : WindsorContainer
    {
        public MvcContainer()
        {
            Kernel.AddComponentInstance<IMvcContainer>( this);
        }

        public void RegisterGenericTypeTransient(System.Type S, System.Type T)
        {
            this.Register(Component.For(S).ImplementedBy(T).LifeStyle.Transient);
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

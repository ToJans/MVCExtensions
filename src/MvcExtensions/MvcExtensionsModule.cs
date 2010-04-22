using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.Model;
using MvcExtensions.Services;
using MvcExtensions.Services.Impl;
using Castle.MicroKernel.Registration;
using MvcExtensions.Services.Impl.FluentNHibernate;

namespace MvcExtensions
{
    public class MvcExtensionsModule : IModule
    {
        MvcContainer _Container = null;
        public MvcContainer Container
        {
            get
            {
                _Container = _Container ?? new MvcContainer();
                return _Container;
            }
            set { _Container = value; }
        }
        public MvcExtensionsModule()
        {
        }

        public virtual void Register(Database database)
        {
            Register(database, false);
        }


        public virtual void Register(Database database,bool isweb) 
        {


            var cont = Container;
            cont.AddFacility<Castle.Facilities.FactorySupport.FactorySupportFacility>();

            if (isweb)
            {
                cont.Register(
                    Component.For<IRepository>().ImplementedBy<Repository>().LifeStyle.PerWebRequest,
                    Component.For<Database>().Instance(database),
                    Component.For<IUnitOfWork>().LifeStyle.PerWebRequest
                        .UsingFactoryMethod(x => x.Resolve<Database>().CurrentUnitOfWork)
                    );
            }
            else
            {
                cont.Register(
                    Component.For<IRepository>().ImplementedBy<Repository>(),
                    Component.For<Database>().Instance(database),
                    Component.For<IUnitOfWork>().UsingFactoryMethod(x => x.Resolve<Database>().CurrentUnitOfWork)
                    );
            }
        }
    }
}

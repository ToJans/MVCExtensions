using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;

namespace MvcExtensions.FNHModules
{
    public class ConventionModule<T> : IFNHModule 
        where T:IConvention,new()
    {
        #region IFNHModule Members

        public virtual void Map(MvcExtensions.Model.IDomainDefinition domain, FluentNHibernate.Automapping.AutoPersistenceModel model)
        {
            model.Conventions.Add<T>();
        }

        public virtual void Configure(NHibernate.Cfg.Configuration config)
        {
            return;
        }

        #endregion
    }
}

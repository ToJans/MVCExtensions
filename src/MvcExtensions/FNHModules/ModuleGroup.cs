using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcExtensions.FNHModules
{
    public class ModuleGroup : IFNHModule
    {
        public List<IFNHModule> Items = new List<IFNHModule>();


        #region IFNHModule Members

        public virtual void Map(MvcExtensions.Model.IDomainDefinition domain, FluentNHibernate.Automapping.AutoPersistenceModel model)
        {
            foreach (var m in Items)
                m.Map(domain, model);
        }

        public virtual void Configure(NHibernate.Cfg.Configuration config)
        {
            foreach(var m in Items)
                m.Configure(config);
        }

        #endregion
    }
}

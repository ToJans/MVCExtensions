using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.Model;
using FluentNHibernate.Automapping;

namespace MvcExtensions.FNHModules
{
    public class InterfaceMapModule<I,C> : IFNHModule
        where C : IInterfaceMap<I>, new()
    {
        public bool HasMembers = false;

        public virtual void Map(IDomainDefinition domain, AutoPersistenceModel model)
        {
            var types = domain.DomainAssembly.GetTypes().Where(x =>
            typeof(I).IsAssignableFrom(x) &&
            domain.GetDomainType(x) != DomainType.None);
            if (types.Any())
            {
                OverrideInterface(model,types);
                HasMembers = true;
            }
        }

        private static void OverrideInterface(AutoPersistenceModel model,IEnumerable<Type> MyTypes)
        {
            var modeloverride = model.GetType().GetMethod("Override");
            var interfacemap = new C();
            var cm = typeof(C).GetMethod("Map");
            foreach (var t in MyTypes.Where(x => typeof(I).IsAssignableFrom(x)))
            {
                var act = cm.MakeGenericMethod(t).Invoke(interfacemap, null);
                modeloverride.MakeGenericMethod(t).Invoke(model, new object[] { act });
            }
        }

        #region IFNHModule Members


        public virtual void Configure(NHibernate.Cfg.Configuration config)
        {
            return;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Mapping;
using FluentNHibernate;

namespace MvcExtensions.Services.Impl.FluentNHibernate
{
    public static class AutoPersistenceModelExtensions
    {
        public static AutoPersistenceModel OverrideInterface<I,C>(this AutoPersistenceModel model
            , IEnumerable<Type> MyTypes)
            where C:IInterfaceMap<I>,new()
        {
            var modeloverride = model.GetType().GetMethod("Override");
            var interfacemap = new C();
            var cm = typeof(C).GetMethod("Map");
            foreach (var t in MyTypes.Where(x => typeof(I).IsAssignableFrom(x)))
            {
                var act = cm.MakeGenericMethod(t).Invoke(interfacemap,null);
                modeloverride.MakeGenericMethod(t).Invoke(model, new object[] { act });
            }
            return model;
        }
    }

    public interface IInterfaceMap<I>
    {
        Action<AutoMapping<T>> Map<T>() where T:I;
    }
}

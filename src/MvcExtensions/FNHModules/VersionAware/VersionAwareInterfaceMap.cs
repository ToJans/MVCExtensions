using System;
using FluentNHibernate.Automapping;

namespace MvcExtensions.FNHModules.VersionAware
{
    public class VersionAwareInterfaceMap : IInterfaceMap<IVersionAware>
    {
        public Action<AutoMapping<T>> Map<T>() where T : IVersionAware
        {
            return mapping =>
            {
                mapping.Map(x => x.Version).Column(VersionFilter.COLUMNNAME);
                mapping.ApplyFilter<VersionFilter>();
            };
        }
    }
}

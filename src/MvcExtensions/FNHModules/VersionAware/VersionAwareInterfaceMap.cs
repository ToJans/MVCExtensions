using System;
using FluentNHibernate.Automapping;

namespace MvcExtensions.FNHModules.VersionAware
{
    public class VersionAwareInterfaceMap : IInterfaceMap<IVersionAware>
    {
        public static readonly string ColumnName = "Version";
        public Action<AutoMapping<T>> Map<T>() where T : IVersionAware 
        {
            return mapping =>
            {
                mapping.Map(x => x.Version).Column(ColumnName);
                mapping.ApplyFilter<VersionFilter>();
            };
        }
    }
}

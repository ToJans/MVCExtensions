using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;

namespace MvcExtensions.FNHModules.Searchable
{
    class SearchableInterfaceMap : IInterfaceMap<ISearchable>
    {
        public static readonly string ColumnName = "SearchText";

        #region IInterfaceMap<ISearchable> Members

        public Action<AutoMapping<T>> Map<T>() where T : ISearchable
        {
            return mapping =>
            {
                mapping.Map(x => x.SearchText).Column(ColumnName).Length(4096);
            };
        }

        #endregion
    }
}

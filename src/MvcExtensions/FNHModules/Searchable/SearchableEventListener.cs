using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Event;
using System.Text.RegularExpressions;
using NHibernate.Persister.Entity;

namespace MvcExtensions.FNHModules.Searchable
{
    public class SearchableEventListener : IPreUpdateEventListener, IPreInsertEventListener 
    {
        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            return Checkversion(@event.Persister, @event.State, @event.Entity);
        }

        public bool OnPreInsert(PreInsertEvent @event)
        {
            return Checkversion(@event.Persister, @event.State, @event.Entity);
        }

        private bool Checkversion(IEntityPersister persister, object[] state, object entity)
        {
            var v = entity as ISearchable;
            if (v == null || string.IsNullOrEmpty(v.SearchText))
                return false;
            var newv = CompressText(v.SearchText);
            if (newv != v.SearchText)
            {
                Set(persister, state, SearchableInterfaceMap.ColumnName, newv);
            }
            return false;
        }

        public static Regex MyRegEx = new Regex("[a-z0-9]+");

        string CompressText(string inp)
        {
            return " "+string.Join(" ", MyRegEx.Matches(
                inp.ToLowerInvariant()).Cast<Match>().Select(x => x.Value).Distinct().ToArray()) + " ";
        }

        private void Set(IEntityPersister persister, object[] state, string propertyName, object value)
        {
            var index = Array.IndexOf(persister.PropertyNames, propertyName);
            if (index == -1)
                return;
            state[index] = value;
        }

    }
}

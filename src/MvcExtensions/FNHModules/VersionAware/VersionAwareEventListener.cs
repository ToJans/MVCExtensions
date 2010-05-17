using System;
using NHibernate.Event;
using NHibernate.Persister.Entity;

namespace MvcExtensions.FNHModules.VersionAware
{
    public class VersionAwareEventListener : IPreUpdateEventListener, IPreInsertEventListener 
    {
        IVersionProvider sVersionProvider;

        public VersionAwareEventListener(IVersionProvider sVersionProvider)
        {
            this.sVersionProvider = sVersionProvider;
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            return Checkversion(@event.Persister,@event.State,@event.Entity);
        }

        public bool OnPreInsert(PreInsertEvent @event)
        {
            return Checkversion(@event.Persister,@event.State,@event.Entity);
        }

        private bool Checkversion(IEntityPersister persister, object[] state,object entity)
        {
            var v =entity as IVersionAware;
            if (v == null)
                return false;
            if (v.Version != null)
                return false;
            var cv = v.Version = sVersionProvider.Version;
            Set(persister, state, "Version", cv);
            return false;
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

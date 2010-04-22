using System;
using NHibernate.Event;
using NHibernate.Persister.Entity;

namespace MvcExtensions.FNHModules.AuditInfo
{
    public class AuditInfoEventListener:IPreUpdateEventListener, IPreInsertEventListener
    {
        IUsernameProvider sUsernameProvider;

        public AuditInfoEventListener(IUsernameProvider sUsernameProvider)
        {
            this.sUsernameProvider = sUsernameProvider;
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            var c = @event.Entity as IAuditInfo;
            if (c == null)
                return false;
            c.UpdatedBy = sUsernameProvider.Username;
            Set(@event.Persister, @event.State, "UpdatedBy", c.UpdatedBy);
            c.UpdatedOn = DateTime.Now;
            Set(@event.Persister, @event.State, "UpdatedOn", c.UpdatedOn);
            return false;
        }

        public bool OnPreInsert(PreInsertEvent @event)
        {
            var c = @event.Entity as IAuditInfo;
            if (c == null)
                return false;
            c.CreatedBy = sUsernameProvider.Username; 
            Set(@event.Persister, @event.State, "CreatedBy", c.CreatedBy);
            c.CreatedOn = DateTime.Now;
            Set(@event.Persister, @event.State, "CreatedOn", c.CreatedOn);
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

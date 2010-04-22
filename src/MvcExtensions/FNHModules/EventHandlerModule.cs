using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.Model;
using FluentNHibernate.Automapping;
using NHibernate.Cfg;

namespace MvcExtensions.FNHModules
{
    public class EventHandlerModule : IFNHModule 
    {
        object eventhandler;

        public EventHandlerModule(object eventhandler)
        {
            this.eventhandler = eventhandler;
        }


        #region IFNHModule Members

        public virtual void Map(IDomainDefinition domain,AutoPersistenceModel model)
        {
            return;
        }

        public void Configure(Configuration cfg)
        {
            cfg.EventListeners.PreInsertEventListeners = RegisterInternal(cfg.EventListeners.PreInsertEventListeners, eventhandler).ToArray();
            cfg.EventListeners.PreUpdateEventListeners = RegisterInternal(cfg.EventListeners.PreUpdateEventListeners, eventhandler).ToArray();
        }

        #endregion

        private static IEnumerable<T> RegisterInternal<T>(IEnumerable<T> prev, object eventhandler) where T : class
        {
            foreach (var v in prev)
                yield return v;
            var q = eventhandler as T;
            if (q != null)
            {
                yield return q;
            }
        }

    }
}

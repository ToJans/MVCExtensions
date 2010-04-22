using FluentNHibernate.Automapping;
using MvcExtensions.Model;
using NHibernate.Cfg;

namespace MvcExtensions.FNHModules.AuditInfo
{
    public class AuditInfoModule : EventHandlerModule 
    {
        public AuditInfoModule(IUsernameProvider sUsername): base(new AuditInfoEventListener(sUsername)) 
        {

        }

    }
}

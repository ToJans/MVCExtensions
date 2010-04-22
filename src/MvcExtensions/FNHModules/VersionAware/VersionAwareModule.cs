using System.Linq;
using FluentNHibernate.Automapping;
using MvcExtensions.Helpers;
using MvcExtensions.Model;

namespace MvcExtensions.FNHModules.VersionAware
{
    public class VersionAwareModule : ModuleGroup 
    {
        InterfaceMapModule<IVersionAware, VersionAwareInterfaceMap> imap;
        
        public VersionAwareModule(IVersionProvider sVersionProvider)
        {
            imap = new InterfaceMapModule<IVersionAware,VersionAwareInterfaceMap>();
            Items.Add(imap);
            Items.Add(new EventHandlerModule(new VersionAwareEventListener(sVersionProvider)));
        }

        public void Map(IDomainDefinition domain, AutoPersistenceModel model)
        {
            base.Map(domain, model);
            if (imap.HasMembers)
                model.Add(new VersionFilter());
        }
    }
}

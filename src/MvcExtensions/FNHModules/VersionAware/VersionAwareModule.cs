using System.Linq;
using FluentNHibernate.Automapping;
using MvcExtensions.Helpers;
using MvcExtensions.Model;

namespace MvcExtensions.FNHModules.VersionAware
{
    public class VersionAwareModule : ModuleGroup 
    {
        public VersionAwareModule(IVersionProvider sVersionProvider)
        {
            Items.Add(new InterfaceMapModule<IVersionAware,VersionAwareInterfaceMap>());
            Items.Add(new EventHandlerModule(new VersionAwareEventListener(sVersionProvider)));
        }

        public override void Map(IDomainDefinition domain, AutoPersistenceModel model)
        {
            base.Map(domain, model);
            model.Add(new VersionFilter());
        }
    }
}

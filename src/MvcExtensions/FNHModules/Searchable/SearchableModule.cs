using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.Model;
using FluentNHibernate.Automapping;

namespace MvcExtensions.FNHModules.Searchable
{
    public class SearchableModule : ModuleGroup 
    {
        public SearchableModule()
        {
            Items.Add(new InterfaceMapModule<ISearchable,SearchableInterfaceMap>());
            Items.Add(new EventHandlerModule(new SearchableEventListener()));
        }

        public override void Map(IDomainDefinition domain, AutoPersistenceModel model)
        {
            base.Map(domain, model);
        }

    }
}

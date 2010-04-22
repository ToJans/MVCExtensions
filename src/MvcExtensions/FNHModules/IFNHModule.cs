using FluentNHibernate.Automapping;
using MvcExtensions.Model;

namespace MvcExtensions.FNHModules
{
    public interface IFNHModule
    {
        void Map(IDomainDefinition domain, AutoPersistenceModel model);
        void Configure(NHibernate.Cfg.Configuration config);
    }
}

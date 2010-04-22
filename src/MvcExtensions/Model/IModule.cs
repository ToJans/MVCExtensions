using MvcExtensions.Services.Impl.FluentNHibernate;

namespace MvcExtensions.Model
{
    public interface IModule
    {
        void Register(Database database);
    }
}

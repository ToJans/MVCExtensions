using MvcExtensions.Services.Impl;
using FluentNHibernate.Automapping;

namespace Tasks.Core.Services
{
    public class TaskDatabaseFluentMapping : IFluentMapping
    {
        // More info : http://wiki.fluentnhibernate.org/Auto_mapping
        public AutoPersistenceModel Map()
        {
            return AutoMap.AssemblyOf<Model.Task>()
             .Where(t => t.Namespace == typeof(Model.Task).Namespace);
        }
    }
}

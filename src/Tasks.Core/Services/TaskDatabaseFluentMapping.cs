using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.Services.Impl;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;

namespace Tasks.Core.Services
{
    public class TaskDatabaseFluentMapping : IFluentMapping
    {
        // More info : http://wiki.fluentnhibernate.org/Auto_mapping
        public AutoPersistenceModel Map()
        {
            return AutoMap.AssemblyOf<Model.Task>(t => t.Namespace == typeof(Model.Task).Namespace);
        }
        
    }
}

using MvcExtensions.Services.Impl;
using FluentNHibernate.Automapping;
using System.Reflection;

namespace Tasks.Core.Services
{
    public class TaskDatabaseFluentMapping : IFluentMapping
    {

        public Assembly ModelAssembly
        {
            get { return typeof(Model.Task).Assembly; }
        }


        public MappingType GetMapType(System.Type t)
        {
            if (t.Namespace == typeof(Model.Task).Namespace)
                return MappingType.Normal;
            // else if some condition
            //    return MappingType.Component
            else return MappingType.None;
        }


        public string WriteMappingFilesToPath
        {
            get { return null; }
        }
    }
}

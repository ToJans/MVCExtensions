using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.Automapping;
using System.Reflection;

namespace MvcExtensions.Services.Impl
{
    public enum MappingType
    {
        None,
        Normal,
        AsConcreteBase,
        Component
    }

    public interface IFluentMapping
    {
        // More info : http://wiki.fluentnhibernate.org/Auto_mapping
        Assembly ModelAssembly {get;}
        MappingType GetMapType(Type t);
        string WriteMappingFilesToPath { get; }
    }
}

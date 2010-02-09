using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.Automapping;
using System.Reflection;

namespace MvcExtensions.Services
{
    public enum DomainType
    {
        None,
        Class,
        ClassWithoutBaseClass,
        Component
    }

    public interface IDomainDefinition
    {
        Assembly DomainAssembly {get;}
        DomainType GetDomainType(Type t);
        string WriteHbmFilesToPath { get; }
    }
}

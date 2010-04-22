using System;
using System.Collections.Generic;
using System.Reflection;
using MvcExtensions.FNHModules;

namespace MvcExtensions.Model
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
        IEnumerable<IFNHModule> RegisteredModules { get; }
    }
}

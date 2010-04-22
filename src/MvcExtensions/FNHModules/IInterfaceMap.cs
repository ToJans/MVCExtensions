using System;
using FluentNHibernate.Automapping;

namespace MvcExtensions.FNHModules
{
    public interface IInterfaceMap<I>
    {
        Action<AutoMapping<T>> Map<T>() where T : I;
    }
}

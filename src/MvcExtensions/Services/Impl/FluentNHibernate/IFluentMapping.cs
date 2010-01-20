using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.Automapping;

namespace MvcExtensions.Services.Impl
{
    public interface IFluentMapping
    {
        // More info : http://wiki.fluentnhibernate.org/Auto_mapping
        AutoPersistenceModel Map();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.Services.Impl.FluentNHibernate;

namespace MvcExtensions.Model
{
    public interface IModule
    {
        void Register(Database database);
    }
}

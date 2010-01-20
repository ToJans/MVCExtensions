using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace MvcExtensions.Services
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        ISession Session {get;}

    }
}

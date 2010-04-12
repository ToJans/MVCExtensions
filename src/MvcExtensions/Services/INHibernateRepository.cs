using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace MvcExtensions.Services
{
    public interface INHibernateRepository : IRepository 
    {
        IQueryable<T> Find<T>(Action<ICriteria> filter) where T:class; 
    }
}

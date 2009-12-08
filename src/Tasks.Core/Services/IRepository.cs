using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tasks.Core.Services
{
    public interface IRepository<T> where T : IModelId
    {
        T GetById(int id);
        IQueryable<T> Find { get; }
        void SaveOrUpdate(T instance);
        void Delete(T instance);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.Model;

namespace MvcExtensions.Services
{
    public interface IRepository<T> where T : IModelId
    {
        T GetById(int id);
        IQueryable<T> Find { get; }
        void SaveOrUpdate(T instance);
        void Delete(T instance);
    }
}

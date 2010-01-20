using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.Model;

namespace MvcExtensions.Services
{
    public interface IRepository 
    {
        T GetById<T>(int id) where T : IModelId;
        IQueryable<T> Find<T>() where T : IModelId;
        void SaveOrUpdate(IModelId instance);
        void Delete(IModelId instance);
    }
}

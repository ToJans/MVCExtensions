using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.Services;
using FluentNHibernate;
using NHibernate.Linq;
using System.Linq.Expressions;

namespace MvcExtensions.Services.Impl.FluentNHibernate
{
    public class Repository : IRepository 
    {

        protected NHibernate.ISession session { get;set; }

        public Repository(IUnitOfWork UnitOfWork)
        {
            this.session = UnitOfWork.Session;
        }

        public T GetById<T>(int id) where T:Model.IModelId
        {
            return session.Get<T>(id);
        }

        public IQueryable<T> Find<T>() where T : Model.IModelId
        {
            return session.Linq<T>();
        }

        public void SaveOrUpdate(Model.IModelId instance)
        {
            session.SaveOrUpdate(instance);
        }

        public void Delete(Model.IModelId instance)
        {
            session.Delete(instance);
        }

    }
}

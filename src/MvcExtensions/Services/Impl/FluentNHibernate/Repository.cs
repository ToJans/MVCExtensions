using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;

namespace MvcExtensions.Services.Impl.FluentNHibernate
{
    public class Repository : IRepository , INHibernateRepository 
    {

        protected NHibernate.ISession session { get;set; }

        protected List<Action<NHibernate.ICriteria>> Filters = new List<Action<NHibernate.ICriteria>>();

        public Repository(IUnitOfWork UnitOfWork)
        {
            this.session = UnitOfWork.Session;
        }

        public T GetById<T>(int id) where T:Model.IModelId
        {
            return session.Get<T>(id);
        }

        public void SaveOrUpdate(Model.IModelId instance)
        {
            session.SaveOrUpdate(instance);
        }

        public void Delete(Model.IModelId instance)
        {
            session.Delete(instance);
        }


        #region INHibernateRepository Members

        public IQueryable<T> Find<T>(Action<NHibernate.ICriteria> filter) where T:class 
        {
            var criteria = session.CreateCriteria<T>();
            filter(criteria);
            foreach (var cr in Filters)
                cr(criteria);
            return criteria.List<T>().AsQueryable();
        }

        #endregion

        #region IRepository Members


        public IQueryable<T> Find<T>() where T : MvcExtensions.Model.IModelId
        {
            return session.Linq<T>();

        }

        #endregion

        public void SetFilter<I>(Predicate<I> check)
        {

        }
    }
}

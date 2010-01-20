using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.Model;

namespace MvcExtensions.Services.Impl
{
    [Obsolete("Use MvcExtensions.Services.Impl.FluentNHibernate.SqlLiteInMemoryDatabase instead of MvcExtensions.Services.Impl.FakeRepository")]
    public class FakeRepository : IRepository
    {
        // this should be some kind of database instead of the array
        public Dictionary<int, object> Instances = new Dictionary<int, object>();

        public FakeRepository()
        {
        }

        public FakeRepository(IEnumerable<IModelId> instances)
        {
            if (instances != null) 
                foreach (var i in instances)
                    Instances.Add(i.Id, i);
        }

        #region IRepository<T> Members

        public T GetById<T>(int id) where T:Model.IModelId 
        {
            return Instances.Where(i=>i.Key == id && i.Value.GetType() == typeof(T))
                .Select(i=>i.Value).Cast<T>().FirstOrDefault() ;
        }

        public IQueryable<T> Find<T>() where T : Model.IModelId 
        {
            return Instances.Values.Where(i => i.GetType() == typeof(T)).Cast<T>().AsQueryable<T>();
        }

        public void SaveOrUpdate(IModelId instance)
        {
            if (Instances.Values.Contains(instance)) return;
            // since in this fake repository nothing is actually written to db
            // this function does nothing except updating the id for new objects
            if (instance.Id == 0)
            {
                instance.Id = Instances.Keys.OrderByDescending(i=>i).FirstOrDefault()+1;
                Instances.Add(instance.Id, instance);
            }
        }

        public void Delete(IModelId instance)
        {
            Instances.Remove(instance.Id);
        }

        #endregion
    }
}

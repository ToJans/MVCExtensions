using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tasks.Core.Services.Impl
{
    public class FakeRepository<T> : IRepository<T> where T:IModelId 
    {
        // this should be some kind of database instead of the array
        public static Dictionary<int, T> Instances = new Dictionary<int, T>();

        public FakeRepository(IEnumerable<T> instances)
        {
            if (instances != null) 
                foreach (var i in instances)
                    Instances.Add(i.Id, i);
        }

        #region IRepository<T> Members

        public T GetById(int id)
        {
            return Instances[id];
        }

        public IQueryable<T> Find
        {
            get { return Instances.Values.AsQueryable(); }
        }

        public void SaveOrUpdate(T instance)
        {
            // since in this fake repository nothing is actually written to db
            // this function does nothing except updating the id for new objects
            if (instance.Id == 0)
            {
                instance.Id = Instances.Keys.OrderByDescending(i=>i).FirstOrDefault()+1;
                Instances.Add(instance.Id, instance);
            }
        }

        public void Delete(T instance)
        {
            Instances.Remove(instance.Id);
        }

        #endregion
    }
}

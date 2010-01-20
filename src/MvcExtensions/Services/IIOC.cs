using System;
namespace MvcExtensions.Services
{
    public interface IIOC  
    {
        T Resolve<T>();
        T Resolve<T>(string name);
        object Resolve(Type t);
        void Release(object o);
    }
}

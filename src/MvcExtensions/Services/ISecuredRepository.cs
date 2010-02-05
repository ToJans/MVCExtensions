using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcExtensions.Services
{
    public interface ISecured
    {
        string[] OwningEntityGroups { get;}
    }

    public interface ISecurity 
    {
        IQueryable<T> Filter<T>(IQueryable<T> input);
        bool IsAllowed<T>(T entity,params string[] actions);
    }
}

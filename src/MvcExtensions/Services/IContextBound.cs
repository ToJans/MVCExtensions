using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcExtensions.Services
{
    public interface IContextBound<T> where T:new() 
    {
        T ContextValue { get; set; }
    }
}

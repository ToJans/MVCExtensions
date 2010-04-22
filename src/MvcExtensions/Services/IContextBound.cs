using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcExtensions.Services
{
    public interface IContextBound<T> 
    {
        T ContextValue { get; set; }
    }
}

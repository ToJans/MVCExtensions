using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcExtensions.Services
{
    public interface IConverter<TFrom,TTo>
    {
        TTo Convert(TFrom source);
        TFrom ConvertBack(TTo destination);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.Model;

namespace MvcExtensions.FNHModules.MyText
{
    public class MyValidatedXlatTextInterfaceMap : IInterfaceMap<MyValidatedXlatText>
    {

        #region IInterfaceMap<MyValidatedXlatText> Members

        public Action<FluentNHibernate.Automapping.AutoMapping<T>> Map<T>() where T : MyValidatedXlatText
        {
            return m =>
                {
                    m.IgnoreProperty(x => x.Value);
                };
        }

        #endregion
    }
}

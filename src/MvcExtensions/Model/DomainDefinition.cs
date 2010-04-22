using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.FNHModules;

namespace MvcExtensions.Model
{
    public abstract class DomainDefinition : IDomainDefinition 
    {
        #region IDomainDefinition Members

        public abstract System.Reflection.Assembly DomainAssembly
        {
            get;
        }

        public abstract DomainType GetDomainType(Type t);

        public virtual string WriteHbmFilesToPath
        {
            get { return null; }
        }

        public virtual IEnumerable<IFNHModule> RegisteredModules
        {
            get { yield return new DefaultMvcExtensionsModules(); }
        }

        #endregion
    }
}

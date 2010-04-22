using System.Collections.Generic;
using System.Reflection;
using MvcExtensions.FNHModules;
using MvcExtensions.FNHModules.AuditInfo;
using MvcExtensions.Model;

namespace Tasks.Core.Services
{
    public class MyDomainDefinition : DomainDefinition 
    {
        IUsernameProvider usernameprovider;

        public MyDomainDefinition(IUsernameProvider usernameprovider)
        {
            this.usernameprovider = usernameprovider;
        }

        public override Assembly DomainAssembly 
        {
            get { return typeof(Model.Task).Assembly; }
        }

        public override DomainType GetDomainType(System.Type t)
        {
            if (t.Namespace == typeof(Model.Task).Namespace)
                return DomainType.Class;
            else return DomainType.None;
        }

        public override IEnumerable<IFNHModule> RegisteredModules
        {
            get { 
                yield return new DefaultMvcExtensionsModules();
                yield return new AuditInfoModule(usernameprovider); 
            }
        }
    }
}

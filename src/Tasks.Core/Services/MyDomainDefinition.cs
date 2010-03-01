using MvcExtensions.Services;
using System.Reflection;

namespace Tasks.Core.Services
{
    public class MyDomainDefinition : IDomainDefinition 
    {

        public Assembly DomainAssembly 
        {
            get { return typeof(Model.Task).Assembly; }
        }

        public DomainType GetDomainType(System.Type t)
        {
            if (t.Namespace == typeof(Model.Task).Namespace)
                return DomainType.Class;
            else return DomainType.None;
            // not used here : DomainType.ClassWithoutBaseClass, DomainType.Component
            
        }


        public string WriteHbmFilesToPath
        {
            get { return null; }
        }
    }
}

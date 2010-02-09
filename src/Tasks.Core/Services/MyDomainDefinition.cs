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
            else if (t.Namespace == typeof(Model.Component.EmailText).Namespace)
                return DomainType.Component;
            else return DomainType.None;
            // not used here : DomainType.ClassWithoutBaseClass
        }


        public string WriteHbmFilesToPath
        {
            get { return null; }
        }
    }
}

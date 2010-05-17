using FluentNHibernate.Mapping;
using NHibernate;

namespace MvcExtensions.FNHModules.VersionAware
{


    public class VersionFilter : FilterDefinition
    {
        public static readonly string FilterName = "MyVersionFilter";
        public static readonly string ParameterName = "Version";
        public static readonly string Condition = 
            string.Format("{0} IS NULL OR {0} = :{1}", VersionAwareInterfaceMap.ColumnName,ParameterName);

        public VersionFilter()
        {
            WithName(FilterName).WithCondition(Condition).AddParameter(ParameterName, NHibernateUtil.String);
        }

        public static void Enable(NHibernate.ISession session, string version)
        {
            session.EnableFilter(FilterName).SetParameter(ParameterName, version);
        }

        public static void Disable(NHibernate.ISession session)
        {
            session.DisableFilter(FilterName);
        }
    }
}

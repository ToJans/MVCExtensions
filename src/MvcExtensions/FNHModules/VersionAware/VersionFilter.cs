using FluentNHibernate.Mapping;
using NHibernate;

namespace MvcExtensions.FNHModules.VersionAware
{


    public class VersionFilter : FilterDefinition
    {
        public static readonly string FILTERNAME = "MyVersionFilter";
        public static readonly string COLUMNNAME = "Version";
        public static readonly string CONDITION = COLUMNNAME + " = :" + COLUMNNAME;

        public VersionFilter()
        {
            WithName(FILTERNAME).WithCondition(CONDITION).AddParameter(COLUMNNAME, NHibernateUtil.String);
        }

        public static void Enable(NHibernate.ISession session, string version)
        {
            session.EnableFilter(FILTERNAME).SetParameter(COLUMNNAME, version);
        }

        public static void Disable(NHibernate.ISession session)
        {
            session.DisableFilter(FILTERNAME);
        }
    }
}

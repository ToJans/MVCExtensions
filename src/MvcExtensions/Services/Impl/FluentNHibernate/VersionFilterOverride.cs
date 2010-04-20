using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Mapping;
using MvcExtensions.Model;
using NHibernate;
using FluentNHibernate.MappingModel;

namespace MvcExtensions.Services.Impl.FluentNHibernate
{
    public interface IVersionAware
    {
        string Version { get; set; }
    }

    public class VersionAwareInterfaceMap : IInterfaceMap<IVersionAware>
    {
        public Action<AutoMapping<T>> Map<T>() where T : IVersionAware
        {
            return mapping =>
                {
                    mapping.Map(x => x.Version).Column(VersionFilter.COLUMNNAME);
                    mapping.ApplyFilter<VersionFilter>();
                };
        }
    }

    public class VersionFilter : FilterDefinition
    {
        public static readonly string FILTERNAME = "MyVersionFilter";
        public static readonly string COLUMNNAME = "Version";
        public static readonly string CONDITION = COLUMNNAME + " = :" + COLUMNNAME;

        public VersionFilter()
        {
            WithName(FILTERNAME).WithCondition(CONDITION).AddParameter(COLUMNNAME, NHibernateUtil.String);
        }

        public static void EnableVersionFilter(NHibernate.ISession session, string version)
        {
            session.EnableFilter(FILTERNAME).SetParameter(COLUMNNAME, version);
        }

        public static void DisableVersionFilter(NHibernate.ISession session)
        {
            session.DisableFilter(FILTERNAME);
        }

    }

    public class ContextVersion
    {
        public string Version { get; set; }
    }

    public class VersionInterceptor : EmptyInterceptor
    {
        IContextBound<ContextVersion> sContextBoundVersion;

        public VersionInterceptor(IContextBound<ContextVersion> sContextBoundVersion)
        {
            this.sContextBoundVersion = sContextBoundVersion;
        }

        public override void SetSession(ISession session)
        {
            VersionFilter.EnableVersionFilter(session, sContextBoundVersion.ContextValue.Version);
        }

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, NHibernate.Type.IType[] types)
        {
            var versionAware = entity as IVersionAware;
            if (versionAware != null && versionAware.Version != sContextBoundVersion.ContextValue.Version)
            {
                int index = Array.FindIndex(propertyNames, x => x.Equals(VersionFilter.COLUMNNAME));
                state[index] = versionAware.Version = sContextBoundVersion.ContextValue.Version;
                return true;
            }
            return false;
        }
    }
}

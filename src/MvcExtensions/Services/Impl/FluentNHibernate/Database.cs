using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Automapping;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Mapping;
using MvcExtensions.Services.Impl.NHibernateUserTypes;
using MvcExtensions.Model;

namespace MvcExtensions.Services.Impl.FluentNHibernate
{
    public abstract class Database : IUnitOfWorkFactory 
    {
        protected ISessionFactory SessionFactory;
        protected static ISession LocalSession;

        public Action CreateDB { get; protected set; }
        public Action UpdateDB { get; protected set; }

        protected bool IsConcreteBaseType(Type t)
        {
            if (t.BaseType == null) return true;
            return t.BaseType.IsGenericType ||
                       t.BaseType.IsAbstract ||
                       typeof(MyText).IsAssignableFrom(t);
                       
        }

        protected Database(IPersistenceConfigurer pcfg, IDomainDefinition mappings)
        {
            var  clsmaps = new DomainType[]{DomainType.Class,DomainType.ClassWithoutBaseClass};
            var cfg = Fluently.Configure()
                .Database(pcfg)
                .Mappings(m => {
                    var am1 = AutoMap.Assembly(mappings.DomainAssembly)
                        .Where(t => clsmaps.Contains(mappings.GetDomainType(t)))
                        .Conventions.Add<BitmapUserTypeConvention>()
                        .Conventions.Add<ColorUserTypeConvention>()
                        .Conventions.Add<CascadeSaveOrUpdateConvention>()
                        .OverrideAll(pig =>
                        {
                            pig.IgnoreProperties(pi => pi.Name == "Value" && pi.ReflectedType.BaseType == typeof (MyValidatedXlatText));
                        })
                       .Setup(c =>
                       {
                           c.IsComponentType = t => typeof(MyText).IsAssignableFrom(t) ||
                               mappings.GetDomainType(t) == DomainType.Component;
                           c.IsConcreteBaseType = t => IsConcreteBaseType(t) ||
                               mappings.GetDomainType(t) == DomainType.ClassWithoutBaseClass;
                           c.GetComponentColumnPrefix = pi =>
                           {
                               return pi.Name;
                           };
                       })
                       ;

                    
                    m.AutoMappings.Add(am1);

                    if (mappings.WriteHbmFilesToPath != null)
                    {
                        foreach (var v in m.AutoMappings)
                        {
                            v.CompileMappings();
                            v.WriteMappingsTo(mappings.WriteHbmFilesToPath);
                        }
                    }
                }).BuildConfiguration();
            SessionFactory = cfg.BuildSessionFactory();
            LocalSession = SessionFactory.OpenSession();
            CreateDB = () =>
            {
                new SchemaExport(cfg).Execute(false, true, false, LocalSession.Connection, null);
            };
            UpdateDB = () =>
            {
                new SchemaUpdate(cfg).Execute(false, true);
            };
        }

        
        #region IUnitOfWorkFactory Members

        public virtual IUnitOfWork CurrentUnitOfWork
        {
            get {
                LocalSession = null;
                return new UnitOfWork(()=>SessionFactory.OpenSession(),true,false ); 
            }
        }

        public ISession CurrentSession
        {
            get {return SessionFactory.OpenSession();}
        }

        #endregion

    }

    public class SqlLiteInMemoryDatabase : Database
    {
        public SqlLiteInMemoryDatabase(IDomainDefinition mappings) : base(SQLiteConfiguration.Standard.InMemory(),mappings)
        {
            CreateDB();
        }
        public override IUnitOfWork CurrentUnitOfWork
        {
            get
            {
                return new UnitOfWork(() => LocalSession, false, true); 
            }
        }
    }

    public class SqlLiteDatabase : Database
    {
        public SqlLiteDatabase(string filename,IDomainDefinition mappings) : base(SQLiteConfiguration.Standard.UsingFile(filename),mappings) 
        { 
        }

        public override IUnitOfWork CurrentUnitOfWork
        {
            get
            {
                return new UnitOfWork(() => SessionFactory.OpenSession(), false, false); 
            }
        }
    }

    public class Sql2005Database : Database
    {
        public Sql2005Database(string connectionstring, IDomainDefinition mappings) :
            base(MsSqlConfiguration.MsSql2005
            .ConnectionString(connectionstring)
            , mappings)
        { }
    }
}

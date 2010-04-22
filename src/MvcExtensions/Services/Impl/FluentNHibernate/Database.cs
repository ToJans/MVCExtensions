using System;
using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using MvcExtensions.Model;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace MvcExtensions.Services.Impl.FluentNHibernate
{
    public abstract class Database : IUnitOfWorkFactory 
    {
        protected ISessionFactory SessionFactory;
        protected ISession LocalSession;
        public Action CreateDB { get; protected set; }
        public Action UpdateDB { get; protected set; }
        public Action DropDB { get; protected set; }

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
                .Mappings(m =>
                {
                    var am1 = AutoMap.Assembly(mappings.DomainAssembly)
                        .Where(t => clsmaps.Contains(mappings.GetDomainType(t)))
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
                       });

                    foreach (var mod in mappings.RegisteredModules)
                        mod.Map(mappings, am1);

                    am1.GetType().GetMethod("UseOverridesFromAssemblyOf").MakeGenericMethod(mappings.GetType()).Invoke(am1, null);
                    am1.Alterations(a =>
                    {
                        a.GetType().GetMethod("AddFromAssemblyOf").MakeGenericMethod(mappings.GetType()).Invoke(a, null);
                    });
                    m.AutoMappings.Add(am1);


                    if (mappings.WriteHbmFilesToPath != null)
                    {
                        foreach (var v in m.AutoMappings)
                        {
                            v.BuildMappings();
                            v.WriteMappingsTo(mappings.WriteHbmFilesToPath);
                        }
                    }
                }).ExposeConfiguration(c =>
                {
                    foreach (var mod in mappings.RegisteredModules)
                        mod.Configure(c);
                }).BuildConfiguration();
            SessionFactory = cfg.BuildSessionFactory();
            
            CreateDB = () =>
            {
                LocalSession = LocalSession??SessionFactory.OpenSession();
                new SchemaExport(cfg).Execute(false, true, false, LocalSession.Connection, null);
            };
            UpdateDB = () =>
            {
                new SchemaUpdate(cfg).Execute(false, true);
            };
            DropDB = () =>
            {
                new SchemaExport(cfg).Drop(true, true);
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
                LocalSession = LocalSession ?? SessionFactory.OpenSession();
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

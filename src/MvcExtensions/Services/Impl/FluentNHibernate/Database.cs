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

        protected Database(IPersistenceConfigurer pcfg, IFluentMapping mappings)
        {
            var cfg = Fluently.Configure()
                .Database(pcfg)
                .Mappings(m => m.AutoMappings.Add(
                    mappings.Map()
                    .Conventions.Add<BitmapUserTypeConvention>()
                    .Conventions.Add<ColorUserTypeConvention>()
                    .Setup(c=>c.IsComponentType = t=>t.IsSubclassOf(typeof(MyText)))
                    ))
                .BuildConfiguration();
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
        public SqlLiteInMemoryDatabase(IFluentMapping mappings) : base(SQLiteConfiguration.Standard.InMemory(),mappings)
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
        public SqlLiteDatabase(string filename,IFluentMapping mappings) : base(SQLiteConfiguration.Standard.UsingFile(filename),mappings) 
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
        public Sql2005Database(string connectionstring, IFluentMapping mappings) :
            base(MsSqlConfiguration.MsSql2005
            .ConnectionString(connectionstring)
            , mappings)
        { }
    }
}

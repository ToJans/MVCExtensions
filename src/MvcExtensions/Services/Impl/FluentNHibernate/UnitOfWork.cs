using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcExtensions.Services;
using NHibernate;

namespace MvcExtensions.Services.Impl
{
    public class UnitOfWork : IUnitOfWork
    {
        public NHibernate.ISession Session { get; private set; }
        private ITransaction transaction;
        bool _useSingleSession;

        public UnitOfWork(Func<NHibernate.ISession> sessioncontstructor,bool useTransactions,bool useSingleSession)
        {
            _useSingleSession = useSingleSession;
            Session = sessioncontstructor();
            
            if (useTransactions)
                transaction = Session.BeginTransaction();
        }

        #region IUnitOfWork Members

        public void Commit()
        {
            if (transaction != null)
            {
                transaction.Commit();
                transaction.Dispose();
                transaction = null;
            }
            Session.Flush();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_useSingleSession) return;
            if (transaction!=null && transaction != null)
            {
                transaction.Rollback();
                transaction.Dispose();
                transaction = null;
            }
            Session.Dispose();
            Session = null;
        }

        #endregion
    }
}

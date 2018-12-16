using System.Web.Http.Filters;
using Mashreq.FXEgypt.WebAPI.Infra.Security;
using NHibernate;

namespace MSLA.Server.WebAPI.Infra.ActionFilters
{
    public interface IActionTransactionHelper
    {
        void BeginTransaction();
        void EndTransaction(HttpActionExecutedContext context);
        void CloseSession();
    }

    public class ActionTransactionHelper : IActionTransactionHelper
    {
        private readonly ICurrentSessionContextAdapter _currentSessionContextAdapter;
        private readonly ISessionFactory _session;

        public ActionTransactionHelper(ISessionFactory session,
            ICurrentSessionContextAdapter currentSessionContextAdapter)
        {
            _session = session;
            _currentSessionContextAdapter = currentSessionContextAdapter;
        }

        public bool TransactionProcessed { get; private set; }
        public bool SessionClosed { get; private set; }

        public void BeginTransaction()
        {
            ISession session = _session.GetCurrentSession();
            if (session != null)
            {
                session.BeginTransaction();
            }
        }

        public void EndTransaction(HttpActionExecutedContext context)
        {
            ISession session = _session.GetCurrentSession();
            if (session == null) return;

            if (!session.Transaction.IsActive) return;

            if (context.Exception == null)
            {
                session.Flush();
                session.Transaction.Commit();
            }
            else
            {
                session.Transaction.Rollback();
            }

            TransactionProcessed = true;
        }

        public void CloseSession()
        {
            if (_currentSessionContextAdapter.HasBind(_session))
            {
                ISession session = _session.GetCurrentSession();
                session.Close();
                session.Dispose();
                _currentSessionContextAdapter.UnBind(_session);
                SessionClosed = true;
            }
        }
    }
}
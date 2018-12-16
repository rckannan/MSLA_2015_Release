using System.Web.Http.Controllers;
using log4net;

namespace MSLA.Server.WebAPI.Infra.ActionFilters
{
    public interface IActionLogHelper
    {
        void LogEntry(HttpActionDescriptor actiondesc);
        void LogExit(HttpActionDescriptor actiondesc);
        void LogAction(HttpActionDescriptor actiondesc, string prefix);
        void Logging(string actiondesc);
    }

    public class ActionLogHelper : IActionLogHelper
    {
        private const string EnteringText = "ENTERING";
        private const string ExitingText = "EXITING";
        private const string LogTextFormatString = "{0} {1}::{2}";

        private readonly ILog _logger;

        public ActionLogHelper(ILog logger)
        {
            _logger = logger;
        }

        public void LogEntry(HttpActionDescriptor actiondesc)
        {
            LogAction(actiondesc, EnteringText);
        }

        public void LogExit(HttpActionDescriptor actiondesc)
        {
            LogAction(actiondesc, ExitingText);
        }

        public void Logging(string actiondesc)
        {
            _logger.DebugFormat(LogTextFormatString, actiondesc);
        }

        public void LogAction(HttpActionDescriptor actiondesc, string prefix)
        {
            _logger.DebugFormat(LogTextFormatString, prefix, actiondesc.ControllerDescriptor.ControllerType.FullName,
                actiondesc.ActionName);
        }
    }
}
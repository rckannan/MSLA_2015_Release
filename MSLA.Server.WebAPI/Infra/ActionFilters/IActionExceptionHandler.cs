using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using log4net;
using System;
using System.Globalization;

namespace MSLA.Server.WebAPI.Infra.ActionFilters
{
    public interface IActionExceptionHandler
    {
        void HandleException(HttpActionExecutedContext actionExecutedContext);
    }

    public class ActionExceptionHandler : IActionExceptionHandler
    {
        private readonly IExceptionMessageFormatter _exceptionMessageFormatter;
        private readonly ILog _logger;

        public ActionExceptionHandler(ILog logger, IExceptionMessageFormatter exceptionMessageFormatter)
        {
            _logger = logger;
            _exceptionMessageFormatter = exceptionMessageFormatter;
        }

        public bool ExceptionHandled { get; private set; }

        public void HandleException(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;
            if (exception == null) return;
            ExceptionHandled = true;

            _logger.Error("Error Occured : ", exception);

            string exceptionreason = _exceptionMessageFormatter.GetCompleteException(exception);
            actionExecutedContext.Response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ReasonPhrase = exceptionreason
            };
        }
    }

    public interface IExceptionMessageFormatter
    {
        string GetCompleteException(Exception ex);
    }

    public class ExceptionMessageFormatter : IExceptionMessageFormatter
    {
        public string GetCompleteException(Exception ex)
        {
            var exceptions = ex.Message.ToString(CultureInfo.InvariantCulture);
            var innerexe = ex.InnerException;
            while (innerexe != null)
            {
                exceptions += " --> " + innerexe.Message.ToString(CultureInfo.InvariantCulture);
                innerexe = innerexe.InnerException;
            }
            return exceptions;
        }
    }
}
using System;
using System.Net;

namespace MSLA.Server.WebAPI.Infra.Base
{
    public class GenericBOResponse : IDisposable
    {
        public Server.Base.SimpleBOMaster data { get; set; }
        public HttpStatusCode status { get; set; }
        public string statusText { get; set; } 
        public GenericBOResponse()
        {
            data = null;
            status = HttpStatusCode.Unused;
            statusText = string.Empty;
        }

        public void Dispose()
        {
            data.Dispose();
        }
    }

    public class GenericDBResponse : IDisposable
    {
        public MSLA.Server.Data.SimpleTable data { get; set; }
        public HttpStatusCode status { get; set; }
        public string statusText { get; set; }

        public GenericDBResponse()
        {
            data = null;
            status = HttpStatusCode.Unused;
            statusText = string.Empty;
        }

        public void Dispose()
        {
            data.Dispose();
        }
    }
}
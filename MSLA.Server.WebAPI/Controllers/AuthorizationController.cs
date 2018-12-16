using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MSLA.Server.WebAPI.Infra.Base;

namespace MSLA.Server.WebAPI.Controllers
{
    public class AuthorizationController : ApiController
    {
        [Route("api/Authorization/GetMenus")]
        public HttpResponseMessage Get()
        {
            try
            {
                IEnumerable<string> custHeader = null;
                Request.Headers.TryGetValues("sessionID", out custHeader);
                var sessionId = new Guid(Convert.ToString(custHeader.FirstOrDefault())) ;
                
 
                return Request.CreateResponse(HttpStatusCode.OK, MenuHelper.GetMenuFromDb(sessionId));
            }

            catch (Exception ex)
            {
                var response = new GenericDBResponse()
                {
                    status = HttpStatusCode.InternalServerError,
                    statusText = ex.Message.ToString()
                };

                return Request.CreateResponse<GenericDBResponse>(HttpStatusCode.InternalServerError, response);
            }
        }
    }
}

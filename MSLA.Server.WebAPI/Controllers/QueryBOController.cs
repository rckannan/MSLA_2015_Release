using MSLA.Server.WebAPI.Infra.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MSLA.Server.WebAPI.Controllers
{
    [Infra.ActionFilters.LoggingNHibernateSession]
    public class QueryBOController : ApiController
    {
        private readonly IBOReader _BOReader;
        public QueryBOController(IBOReader BOReader)
        {
            _BOReader = BOReader;
        }

        //public HttpResponseMessage Get(string DocType, string Doc_ID)
        //{
        //    MSLA.Server.Login.LogonResult myLogonResult;
        //    Server.Security.SimpleUserInfo myUser = new Server.Security.SimpleUserInfo();

        //    MSLA.Server.Login.LogonInfo myLogonInfo = new Server.Login.LogonInfo("chinnakannanr");
        //    MSLA.Server.Login.Logon myLogon = new Server.Login.Logon();
        //    myLogonResult = myLogon.TryLogin(myLogonInfo);

        //    if (myLogonResult.Status == MSLA.Server.Login.Logon.LogonState.Succeeded)
        //    {
        //        myUser = Server.Login.LogonService.SaveLogonInfo(myLogonResult);
        //        myUser.MainDBName = myLogonResult.MainDBName;
        //    }
        //    else
        //    {
        //        myUser.User_ID = -1;
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, _BOReader.FetchBOMaster(DocType, Doc_ID, myLogonResult.Session_ID));
        //}

        [Route("api/QueryBO/FetchBO/{DocType}/{Doc_ID}")]
        public HttpResponseMessage Get(string DocType, string Doc_ID)
        {
            //MSLA.Server.Login.LogonResult myLogonResult;
            //Server.Security.SimpleUserInfo myUser = new Server.Security.SimpleUserInfo();

            //MSLA.Server.Login.LogonInfo myLogonInfo = new Server.Login.LogonInfo("chinnakannanr");
            //MSLA.Server.Login.Logon myLogon = new Server.Login.Logon();
            //myLogonResult = myLogon.TryLogin(myLogonInfo);

            //if (myLogonResult.Status == MSLA.Server.Login.Logon.LogonState.Succeeded)
            //{
            //    myUser = Server.Login.LogonService.SaveLogonInfo(myLogonResult);
            //    myUser.MainDBName = myLogonResult.MainDBName;
            //}
            //else
            //{
            //    myUser.User_ID = -1;
            //}
            IEnumerable<string> custHeader = null;
            Request.Headers.TryGetValues("sessionID", out custHeader);
            var sessionId = new Guid(Convert.ToString(custHeader.FirstOrDefault()));

            return Request.CreateResponse(HttpStatusCode.OK, _BOReader.FetchBOMaster(DocType, Doc_ID, sessionId));
        }

        [Route(Name ="FetchBO")]
        [HttpPost]
        public HttpResponseMessage FetchBO(MasterCriteriaBase myBOCriteria, Guid Request_ID)
        {
            return Request.CreateResponse(HttpStatusCode.OK,_BOReader.FetchBOMaster(myBOCriteria, Request_ID));
        }


        [Route("api/QueryBO/SaveBO")]
        [HttpPost]
        public HttpResponseMessage SaveBO(Server.Base.SimpleBOMaster myBO)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _BOReader.SaveBOMaster(myBO));
        }

        
        [Route("api/QueryBO/DeleteBO")]
        [HttpPost]
        public HttpResponseMessage DeleteBO(MSLA.Server.Base.SimpleBOMaster myBOMaster, Guid Request_ID)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _BOReader.DeleteBOMaster(myBOMaster, Request_ID));
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}

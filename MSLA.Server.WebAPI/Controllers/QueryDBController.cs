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
    [Authorize]
    public class QueryDBController : ApiController
    {
        private readonly IDBQuerier _DBQuerier;
        public QueryDBController(IDBQuerier DBQuerier)
        {
            _DBQuerier = DBQuerier;
        }

        [Route("api/QueryDB/FillDT")]
        public HttpResponseMessage Get()
        {
            //Reterive data for PSRE

            try
            {
                //MSLA.Server.Data.DataCommand cmm = new Server.Data.DataCommand();
                //cmm.CommandText = "select * from Eibor.tblTenor";
                //cmm.CommandType = Server.Data.EnDataCommandType.Text;
                ////cmm.Parameters = cmdParams;
                //cmm.ConnectionType= Server.Data.DBConnectionType.OLTPDB;
                //cmm.CommandTimeout = cmdTimeout; 


                GenericQyRequest reqob = new GenericQyRequest()
                {
                    RequestObject = "EiborFetch",
                    Params = new List<Params>(){
                        new  Params()
                        {
                            Name = "@Tenor_ID",
                            direction = Server.Data.DataParameter.EnParameterDirection.Input,
                            ParamType = Server.Data.DataParameter.EnDataParameterType.BigInt,
                            value = 100001
                        }
                    }
                };

                var cmm = _DBQuerier.GenerateMSLAQueryOb(reqob);

                if (cmm == null)
                {
                    var _resp = new GenericDBResponse()
                    {
                        status = HttpStatusCode.NotImplemented,
                        statusText = "SQL Object has not found in the list."
                    };

                    return Request.CreateResponse<GenericDBResponse>(HttpStatusCode.NotImplemented, _resp);
                }

                var _response = new GenericDBResponse()
                {
                    status = HttpStatusCode.Found,
                    data = MSLA.Server.Data.DataConnect.FillDt(MSLA.Server.Data.DataCommand.GetSQLCommand(cmm), cmm.ConnectionType)
                };
                var resp = Request.CreateResponse<GenericDBResponse>(HttpStatusCode.OK, _response);
                return resp;
            }

            catch (Exception ex)
            {
                var _response = new GenericDBResponse()
                {
                    status = HttpStatusCode.InternalServerError,
                    statusText = ex.Message.ToString()
                };

                return Request.CreateResponse<GenericDBResponse>(HttpStatusCode.InternalServerError, _response);
            }
        }

        [Route("api/QueryDB/FillDS")]
        [HttpGet]
        public HttpResponseMessage FillDS()
        {
            //Reterive data for PSRE

            try
            {
                //MSLA.Server.Data.DataCommand cmm = new Server.Data.DataCommand();
                //cmm.CommandText = "select * from Eibor.tblTenor";
                //cmm.CommandType = Server.Data.EnDataCommandType.Text;
                ////cmm.Parameters = cmdParams;
                //cmm.ConnectionType= Server.Data.DBConnectionType.OLTPDB;
                //cmm.CommandTimeout = cmdTimeout; 


                GenericQyRequest reqob = new GenericQyRequest()
                {
                    RequestObject = "EiborFetch",
                    Params = new List<Params>(){
                        new  Params()
                        {
                            Name = "@Tenor_ID",
                            direction = Server.Data.DataParameter.EnParameterDirection.Input,
                            ParamType = Server.Data.DataParameter.EnDataParameterType.BigInt,
                            value = 100001
                        }
                    }
                };

                var cmm = _DBQuerier.GenerateMSLAQueryOb(reqob);

                var _response = new GenericDBResponse()
                {
                    status = HttpStatusCode.Found,
                    //data = MSLA.Server.Data.DataConnect.FillDS(MSLA.Server.Data.DataCommand.GetSQLCommand(cmm), cmm.ConnectionType)
                };
                var resp = Request.CreateResponse<GenericDBResponse>(HttpStatusCode.OK, _response);
                return resp;
            }

            catch (Exception ex)
            {
                var _response = new GenericDBResponse()
                {
                    status = HttpStatusCode.InternalServerError,
                    statusText = ex.Message.ToString()
                };

                return Request.CreateResponse<GenericDBResponse>(HttpStatusCode.InternalServerError, _response);
            }
        }


        [Route("api/QueryDB/FillDT")] 
        [HttpPost]
        public HttpResponseMessage Post(GenericQyRequest Reqobj)
        {
            //Reterive data for PSRE

            try
            {
                //IEnumerable<string> custHeader = null;
                //Request.Headers.TryGetValues("sessionID", out custHeader); 

                var cmm = _DBQuerier.GenerateMSLAQueryOb(Reqobj);

                if (cmm == null)
                {
                    var _resp = new GenericDBResponse()
                    {
                        status = HttpStatusCode.NotImplemented,
                        statusText = "SQL Object has not found in the list."
                    };

                    return Request.CreateResponse<GenericDBResponse>(HttpStatusCode.NotImplemented, _resp);
                }


                var _response = new GenericDBResponse()
                {
                    status = HttpStatusCode.Found,
                    data = MSLA.Server.Data.DataConnect.FillDt(MSLA.Server.Data.DataCommand.GetSQLCommand(cmm), cmm.ConnectionType)
                };
                var resp = Request.CreateResponse<GenericDBResponse>(HttpStatusCode.OK, _response);
                return resp;
            }

            catch (Exception ex)
            {
                var _response = new GenericDBResponse()
                {
                    status = HttpStatusCode.InternalServerError,
                    statusText = ex.Message.ToString()
                };

                return Request.CreateResponse<GenericDBResponse>(HttpStatusCode.InternalServerError, _response);
            }

        }

        [Route("api/QueryDB/ExecNonQry")]
        [HttpPost]
        public HttpResponseMessage ExecNonQry(GenericQyRequest Reqobj)
        {
            //Reterive data for PSRE

            try
            {
                var cmm = _DBQuerier.GenerateMSLAQueryOb(Reqobj);

                if (cmm == null)
                {
                    var _resp = new GenericDBResponse()
                    {
                        status = HttpStatusCode.NotImplemented,
                        statusText = "SQL Object has not found in the list."
                    };

                    return Request.CreateResponse<GenericDBResponse>(HttpStatusCode.NotImplemented, _resp);
                }

                var Qry = MSLA.Server.Data.DataCommand.GetSQLCommand(cmm);
                //MSLA.Server.Data.DataConnect.ExecCMM(, cmm.ConnectionType);
                var _response = new GenericDBResponse()
                {
                    status = HttpStatusCode.OK,
                    data = MSLA.Server.Data.DataConnect.FillDt(MSLA.Server.Data.DataCommand.GetSQLCommand(cmm), cmm.ConnectionType)
                };
                var resp = Request.CreateResponse<GenericDBResponse>(HttpStatusCode.OK, _response);
                return resp;
            }

            catch (Exception ex)
            {
                var _response = new GenericDBResponse()
                {
                    status = HttpStatusCode.InternalServerError,
                    statusText = ex.Message.ToString()
                };

                return Request.CreateResponse<GenericDBResponse>(HttpStatusCode.InternalServerError, _response);
            }

        }

        [Route("api/QueryDB/PostException")]
        [HttpPost]
        public HttpResponseMessage PostException(ExceptionLog reqObject)
        {
            try
            {
                IEnumerable<string> custHeader = null;
                Request.Headers.TryGetValues("sessionID", out custHeader);
                var sessionId = new Guid(Convert.ToString(custHeader.FirstOrDefault()));
                MenuHelper.PushException(reqObject, sessionId); 
                return Request.CreateResponse(HttpStatusCode.OK);
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

        [Route("api/QueryDB/PostFeedback")]
        [HttpPost]
        public HttpResponseMessage PostFeedback(Feedbacks reqobj)
        {
            try
            {
                IEnumerable<string> custHeader = null;
                Request.Headers.TryGetValues("sessionID", out custHeader);
                var sessionId = new Guid(Convert.ToString(custHeader.FirstOrDefault()));
                MenuHelper.PushFeedback(reqobj, sessionId);
                return Request.CreateResponse(HttpStatusCode.OK);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MSLA.Server.Base;
using MSLA.Server.Security;

namespace MSLA.Server.WebAPI.Infra.Base
{
    public interface IBOReader
    {
        GenericBOResponse FetchBOMaster(MasterCriteriaBase myBOCriteria, Guid Request_ID);

        GenericBOResponse FetchBOMaster(string DocType, string Doc_ID, Guid Request_ID);

        GenericBOResponse SaveBOMaster(Server.Base.SimpleBOMaster myBO);

        GenericBOResponse DeleteBOMaster(MSLA.Server.Base.SimpleBOMaster myBO, Guid Request_ID);

        MSLA.Server.Data.SimpleTable InvokeMethod(string assemblyName, string objectNamespace, string className, object[] constructorArgs,
               string methodName, object[] methodArgs,
               Server.Security.SimpleUserInfo UserInfo, Guid Request_ID);
    }

    public class BOReader : IBOReader
    { 
        public GenericBOResponse FetchBOMaster(MasterCriteriaBase myBOCriteria, Guid Request_ID)
        {
            var _response = new GenericBOResponse();
            try
            {

                MSLA.Server.BO.MasterBase myBo = MSLA.Server.BO.MasterBase.DataPortal_Fetch(myBOCriteria, Server.Login.LogonService.FetchLogonInfo(Request_ID));
                return new GenericBOResponse()
                {
                    data = myBo.ConstructSimpleBO(),
                    status = System.Net.HttpStatusCode.Found
                };

            }
            catch (Exception ex)
            {
                MSLA.Server.Exceptions.ServiceExceptionHandler.HandleException(myBOCriteria.UserInfo, ex, Request_ID.ToString());
                return new GenericBOResponse()
                {
                    data = null,
                    status = System.Net.HttpStatusCode.NotAcceptable,
                    statusText = ex.Message
                };
            }
        }

        public GenericBOResponse FetchBOMaster(string DocType, string Doc_ID, Guid Request_ID)
        {
            var _response = new GenericBOResponse();
            var LoginInfo = Server.Login.LogonService.FetchLogonInfo(Request_ID);
            try
            {

                MasterCriteriaBase bse = new MasterCriteriaBase()
                {
                    DocMaster_ID = Convert.ToInt64(Doc_ID),
                    DocMasterType = DocType,
                    PropertyCollection = new Dictionary<string, object>()
                };

                MSLA.Server.BO.MasterBase myBo = MSLA.Server.BO.MasterBase.DataPortal_Fetch(bse, LoginInfo);
                return new GenericBOResponse()
                {
                    data = myBo.ConstructSimpleBO(),
                    status = System.Net.HttpStatusCode.Found
                };

            }
            catch (Exception ex)
            {
                MSLA.Server.Exceptions.ServiceExceptionHandler.HandleException(Request_ID, LoginInfo.User_ID, ex);
                return new GenericBOResponse()
                {
                    data = null,
                    status = System.Net.HttpStatusCode.NotAcceptable,
                    statusText = ex.Message
                };
            }
        }

        public MSLA.Server.Data.SimpleTable InvokeMethod(string assemblyName, string objectNamespace, string className, object[] constructorArgs, string methodName, object[] methodArgs, SimpleUserInfo UserInfo, Guid Request_ID)
        {
            try
            {
                if (constructorArgs != null)
                {   // Convert the UserInfo to server side User Info
                    for (int i = 0; i < constructorArgs.Length; i++)
                    {
                        if (constructorArgs[i].ToString() == "Server.Security.SimpleUserInfo")
                        {
                            constructorArgs[i] = Server.Login.LogonService.FetchLogonInfo(UserInfo.Session_ID);
                        }
                        else if (constructorArgs[i] is Server.Data.SimpleTable)
                        {
                            constructorArgs[i] = Server.Data.DataConnect.ResolveToSystemTable(constructorArgs[i] as MSLA.Server.Data.SimpleTable);
                        }
                    }
                }

                if (methodArgs != null)
                {   // Convert the UserInfo to server side User Info
                    for (int i = 0; i < methodArgs.Length; i++)
                    {
                        if (methodArgs[i].ToString() == "Server.Security.SimpleUserInfo")
                        {
                            methodArgs[i] = Server.Login.LogonService.FetchLogonInfo(UserInfo.Session_ID);
                        }
                        if (methodArgs[i] is Server.Data.SimpleTable)
                        {
                            methodArgs[i] = Server.Data.DataConnect.ResolveToSystemTable(methodArgs[i] as MSLA.Server.Data.SimpleTable);
                        }
                    }
                }

                MSLA.Server.Data.SimpleTable dtResult = new Server.Data.SimpleTable();
                object cInstance = MSLA.Server.Utilities.ReflectionHelper.CreateObject(assemblyName, objectNamespace, className, constructorArgs);
                if (cInstance != null)
                {
                    object res = MSLA.Server.Utilities.ReflectionHelper.CallMethod(cInstance, methodName, methodArgs);
                    if (res is MSLA.Server.Data.SimpleTable)
                    {
                        dtResult = res as MSLA.Server.Data.SimpleTable;
                    }
                    if (res is System.Data.DataTable)
                    {
                        dtResult = MSLA.Server.Data.DataConnect.ResolveToSimpleTable(res as System.Data.DataTable);
                    }
                }
                return dtResult;
            }
            catch (Exception ex)
            {
                MSLA.Server.Exceptions.ServiceExceptionHandler.HandleException(UserInfo, ex, Request_ID.ToString()); 
            }
            return new MSLA.Server.Data.SimpleTable();
        }

        public GenericBOResponse SaveBOMaster(SimpleBOMaster myBO)
        {
            MSLA.Server.BO.MasterBase mySavedBO = null;
            try
            {

                mySavedBO = MSLA.Server.BO.MasterBase.ConstructMasterBO(myBO, Server.Login.LogonService.FetchLogonInfo(myBO.UserInfo.Session_ID));

                mySavedBO.DataPortal_Save();
                return new GenericBOResponse()
                {
                    data = mySavedBO.ConstructSimpleBO(),
                    status = System.Net.HttpStatusCode.Created,
                    statusText = "Successfully saved."
                };

            }
            catch (MSLA.Server.Validations.ValidateException ex)
            {
                if (mySavedBO != null)
                {
                    if (mySavedBO.HasBrokenSaveRules)
                    {
                        myBO.BrokenSaveRules = mySavedBO.BrokenSaveRules;
                    }
                    return new GenericBOResponse()
                    {
                        data = mySavedBO.ConstructSimpleBO(),
                        status = System.Net.HttpStatusCode.PreconditionFailed ,
                        statusText = "Broken rules! validations failed."
                    };
                }
                else
                {
                    MSLA.Server.Exceptions.ServiceExceptionHandler.HandleException(myBO.UserInfo, ex, myBO.UserInfo.Session_ID.ToString());
                    return new GenericBOResponse()
                    {
                        data = null,
                        status = System.Net.HttpStatusCode.NotAcceptable,
                        statusText = ex.Message
                    };
                }
            }
            catch (Exception ex)
            {
                MSLA.Server.Exceptions.ServiceExceptionHandler.HandleException(myBO.UserInfo, ex, myBO.UserInfo.Session_ID.ToString());
                return new GenericBOResponse()
                {
                    data = null,
                    status = System.Net.HttpStatusCode.InternalServerError,
                    statusText = ex.Message
                };
            }
        }

        public GenericBOResponse DeleteBOMaster(SimpleBOMaster myBO, Guid Request_ID)
        {
            GenericBOResponse _response = new GenericBOResponse();
            MSLA.Server.BO.MasterBase mySavedBO = null;
            try
            {

               mySavedBO = MSLA.Server.BO.MasterBase.ConstructMasterBO(myBO, Server.Login.LogonService.FetchLogonInfo(myBO.UserInfo.Session_ID));

                mySavedBO.DataPortal_Delete();

                _response = new GenericBOResponse()
                {
                    data = new Server.Base.SimpleBOMaster(),
                    status = System.Net.HttpStatusCode.OK
                };

            }
            catch (MSLA.Server.Validations.ValidateException ex)
            {
                if (mySavedBO != null)
                {
                    if (mySavedBO.HasBrokenSaveRules)
                    {
                        if (mySavedBO.HasBrokenDeleteRules)
                        {
                            myBO.BrokenDeleteRules = mySavedBO.BrokenDeleteRules;
                        } 
                    }
                    _response = new GenericBOResponse()
                    {
                        data = mySavedBO.ConstructSimpleBO(),
                        status = System.Net.HttpStatusCode.PreconditionFailed
                    }; 
                }
                else
                {
                    MSLA.Server.Exceptions.ServiceExceptionHandler.HandleException(myBO.UserInfo, ex, myBO.UserInfo.Session_ID.ToString());
                    _response = new GenericBOResponse()
                    {
                        data = null,
                        status = System.Net.HttpStatusCode.NotAcceptable,
                        statusText = ex.Message
                    };
                }
            }
            catch (Exception ex)
            {
                MSLA.Server.Exceptions.ServiceExceptionHandler.HandleException(myBO.UserInfo, ex, myBO.UserInfo.Session_ID.ToString());
                _response = new GenericBOResponse()
                {
                    data = null,
                    status = System.Net.HttpStatusCode.InternalServerError,
                    statusText = ex.Message
                };
            }
            return _response;
        }

        
    }
}
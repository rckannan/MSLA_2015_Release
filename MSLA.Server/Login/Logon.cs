using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace MSLA.Server.Login
{
    public interface ILogon
    {
        LogonResult TryLogin(LogonInfo myLogonInfo); 

        LogonResult TryLoginMain(LogonInfo myLogonInfo);

        LogonResult TryLoginUPA(LogonInfo myLogonInfo);
        void UserSessionOpen(LogonResult LogResult);

        bool AddRefreshToken(MSLA.Server.Login.RefreshToken token);

        RefreshToken FindRefreshToken(Int64 userId);

        WebClient GetWebClient(string webClientId);
    }


    /// <summary>The Logon Class</summary>
    //[System.Diagnostics.DebuggerStepThrough()]
    public partial class Logon
        : MarshalByRefObject, ILogon
    {
        public Logon()
            : base()
        {
        }

        /// <summary>Enum For Logon State</summary>
        public enum LogonState
        {
            /// <summary>Logon Failed</summary>
            Failed = 0,
            /// <summary>Logon Succeeded</summary>
            Succeeded = 1
        }

        /// <summary>Use this method to Login. Returns Logon Result</summary>
        /// <param name="myLogonInfo">The Logon Info</param>
        public LogonResult TryLogin(LogonInfo myLogonInfo)
        {
            LogonResult myLogonResult = null;
            SqlConnection cnLogin = null;
            try
            {
                //  *****   Now that the connection succeeded, we verify
                //   *****   whether the UserName and password
                SqlCommand cmm = new SqlCommand();
                System.Int64 myUserID;
                Boolean UserIsSuperUser = false;
                cnLogin = Data.DataAccess.GetCn(Data.DBConnectionType.MainDB);
                cmm.CommandType = CommandType.StoredProcedure;
                //cmm.CommandText = "Main.spUserValidateWithoutPwd";
                cmm.CommandText = "Main.spUserValidateWithoutPwdWeb";
                cmm.Connection = cnLogin;
                if (cmm.Connection.State == ConnectionState.Closed)
                {
                    cmm.Connection.Open();
                }
                cmm.Parameters.Add("@SuperUser", SqlDbType.VarChar, 20).Value = Security.Encryption64.DecryptFromBase64String(Utilities.AppConfig.Item("Superuser").ToString());
                //cmm.Parameters.Add("@SuperPass", SqlDbType.VarChar, 16).Value = Security.Encryption64.DecryptFromBase64String(Utilities.AppConfig.Item("Superpwd").ToString());
                cmm.Parameters.Add("@UserName", SqlDbType.VarChar, 20).Value = myLogonInfo.UserName;
                //cmm.Parameters.Add("@UserPass", SqlDbType.VarChar, 16).Value = myLogonInfo.UserPass;
                cmm.Parameters.Add("@UserIsSuperUser", SqlDbType.Bit, 0).Value = 0;
                cmm.Parameters["@UserIsSuperUser"].Direction = ParameterDirection.InputOutput;
                cmm.Parameters.Add("@FullUserName", SqlDbType.VarChar, 50).Value = "";
                cmm.Parameters["@FullUserName"].Direction = ParameterDirection.InputOutput;
                cmm.Parameters.Add("@User_ID", SqlDbType.BigInt, 0).Value = -1;
                cmm.Parameters["@User_ID"].Direction = ParameterDirection.InputOutput;
                cmm.Parameters.Add("@Message", SqlDbType.VarChar, 250).Value = "";
                cmm.Parameters["@Message"].Direction = ParameterDirection.InputOutput;
                cmm.Parameters.Add("@Machine_Name", SqlDbType.VarChar, 250).Value = myLogonInfo.ClientMachineName;
                cmm.Parameters.Add("@ClientIP", SqlDbType.VarChar, 250).Value = myLogonInfo.ClientIP;
                cmm.Parameters.Add("@ClientMAC", SqlDbType.VarChar, 250).Value = myLogonInfo.ClientMAC;
                cmm.Parameters.Add("@SessionMessage", SqlDbType.VarChar, 250).Value = "";
                cmm.Parameters["@SessionMessage"].Direction = ParameterDirection.InputOutput;
                cmm.ExecuteNonQuery();

                if (Convert.ToString(cmm.Parameters["@Message"].Value) != "OK")
                {
                    if (Convert.ToString(cmm.Parameters["@SessionMessage"].Value) == "")
                    {
                        // ****    Logon Failed due to invalid username/password
                        //Utilities.LogWriter.LogFailedSession(myLogonInfo, cnLogin);
                    }
                    //  Exceptions.ServiceExceptionHandler.HandleException(-1, string.Empty, new Exception(Convert.ToString(cmm.Parameters["@Message"].Value))); 

                    throw new Exception(Convert.ToString(cmm.Parameters["@Message"].Value));

                }
                else //'   *****   Logon Succeeded
                {
                    if (Convert.ToBoolean(cmm.Parameters["@UserIsSuperUser"].Value))
                    { UserIsSuperUser = true; }

                    myUserID = Convert.ToInt64(cmm.Parameters["@User_ID"].Value);
                    myLogonResult = new LogonResult(LogonState.Succeeded, Convert.ToString(cmm.Parameters["@FullUserName"].Value), cmm.Connection.ToString(), UserIsSuperUser,
                                                    myUserID, myLogonInfo.UserName, myLogonInfo.SQLServer, String.Empty, cnLogin.Database);
                    myLogonResult.LogonInfoUsed = myLogonInfo;
                    //  ****    Open User Session
                    UserSessionOpen(myLogonResult);
                }
            }
            catch (Exception exLogon)
            {
                // Exceptions.ServiceExceptionHandler.HandleException(-1, string.Empty, exLogon); 
                throw new LogonException(exLogon);
            }
            finally
            {
                if (cnLogin.State != ConnectionState.Closed)
                { cnLogin.Close(); }
            }
            myLogonResult.SetMainDBName(cnLogin.Database);
            return myLogonResult;
        }

        public LogonResult TryLoginMain(LogonInfo myLogonInfo)
        {
            LogonResult myLogonResult = null;
            SqlConnection cnLogin = null;
            try
            {
                //  *****   Now that the connection succeeded, we verify
                //   *****   whether the UserName and password
                SqlCommand cmm = new SqlCommand();
                System.Int64 myUserID;
                Boolean UserIsSuperUser = false;
                cnLogin = Data.DataAccess.GetCn(Data.DBConnectionType.MainDB);
                cmm.CommandType = CommandType.StoredProcedure;
                cmm.CommandText = "Main.spUserValidate";
                cmm.Connection = cnLogin;
                if (cmm.Connection.State == ConnectionState.Closed)
                {
                    cmm.Connection.Open();
                }
                cmm.Parameters.Add("@SuperUser", SqlDbType.VarChar, 20).Value = Security.Encryption64.DecryptFromBase64String(Utilities.AppConfig.Item("Superuser").ToString());
                cmm.Parameters.Add("@SuperPass", SqlDbType.VarChar, 16).Value = Security.Encryption64.DecryptFromBase64String(Utilities.AppConfig.Item("Superpwd").ToString());
                cmm.Parameters.Add("@UserName", SqlDbType.VarChar, 20).Value = myLogonInfo.UserName;
                cmm.Parameters.Add("@UserPass", SqlDbType.VarChar, 16).Value = myLogonInfo.UserPass;
                cmm.Parameters.Add("@UserIsSuperUser", SqlDbType.Bit, 0).Value = 0;
                cmm.Parameters["@UserIsSuperUser"].Direction = ParameterDirection.InputOutput;
                cmm.Parameters.Add("@FullUserName", SqlDbType.VarChar, 50).Value = "";
                cmm.Parameters["@FullUserName"].Direction = ParameterDirection.InputOutput;
                cmm.Parameters.Add("@User_ID", SqlDbType.BigInt, 0).Value = -1;
                cmm.Parameters["@User_ID"].Direction = ParameterDirection.InputOutput;
                cmm.Parameters.Add("@Message", SqlDbType.VarChar, 250).Value = "";
                cmm.Parameters["@Message"].Direction = ParameterDirection.InputOutput;
                cmm.Parameters.Add("@Machine_Name", SqlDbType.VarChar, 250).Value = myLogonInfo.ClientMachineName;
                cmm.Parameters.Add("@ClientIP", SqlDbType.VarChar, 250).Value = myLogonInfo.ClientIP;
                cmm.Parameters.Add("@ClientMAC", SqlDbType.VarChar, 250).Value = myLogonInfo.ClientMAC;
                cmm.Parameters.Add("@SessionMessage", SqlDbType.VarChar, 250).Value = "";
                cmm.Parameters["@SessionMessage"].Direction = ParameterDirection.InputOutput;
                cmm.ExecuteNonQuery();

                if (Convert.ToString(cmm.Parameters["@Message"].Value) != "OK")
                {
                    if (Convert.ToString(cmm.Parameters["@SessionMessage"].Value) == "")
                    {
                        // ****    Logon Failed due to invalid username/password
                        //Utilities.LogWriter.LogFailedSession(myLogonInfo, cnLogin);
                    }
                    //  Exceptions.ServiceExceptionHandler.HandleException(-1, string.Empty, new Exception(Convert.ToString(cmm.Parameters["@Message"].Value))); 

                    throw new Exception(Convert.ToString(cmm.Parameters["@Message"].Value));

                }
                else //'   *****   Logon Succeeded
                {
                    if (Convert.ToBoolean(cmm.Parameters["@UserIsSuperUser"].Value))
                    { UserIsSuperUser = true; }

                    myUserID = Convert.ToInt64(cmm.Parameters["@User_ID"].Value);
                    myLogonResult = new LogonResult(LogonState.Succeeded, Convert.ToString(cmm.Parameters["@FullUserName"].Value), cmm.Connection.ToString(), UserIsSuperUser,
                                                    myUserID, myLogonInfo.UserName, myLogonInfo.SQLServer, String.Empty, cnLogin.Database);
                    myLogonResult.LogonInfoUsed = myLogonInfo;
                    //  ****    Open User Session
                    UserSessionOpen(myLogonResult);
                }
            }
            catch (Exception exLogon)
            {
                // Exceptions.ServiceExceptionHandler.HandleException(-1, string.Empty, exLogon); 
                throw new LogonException(exLogon);
            }
            finally
            {
                if (cnLogin.State != ConnectionState.Closed)
                { cnLogin.Close(); }
            }
            myLogonResult.SetMainDBName(cnLogin.Database);
            return myLogonResult;
        }

        public LogonResult TryLoginUPA(LogonInfo myLogonInfo)
        {
            LogonResult myLogonResult = null;
            SqlConnection cnLogin = null;
            try
            {
                //  *****   Now that the connection succeeded, we verify
                //   *****   whether the UserName and password
                SqlCommand cmm = new SqlCommand();
                System.Int64 myUserID;
                Boolean UserIsSuperUser = false;
                cnLogin = Data.DataAccess.GetCn(Data.DBConnectionType.MainDB);
                cmm.CommandType = CommandType.StoredProcedure;
                cmm.CommandText = "Main.spUPAUserValidateWOPwd";
                cmm.Connection = cnLogin;
                if (cmm.Connection.State == ConnectionState.Closed)
                {
                    cmm.Connection.Open();
                }
                cmm.Parameters.Add("@SuperUser", SqlDbType.VarChar, 20).Value = Security.Encryption64.DecryptFromBase64String(Utilities.AppConfig.Item("Superuser").ToString());
                //cmm.Parameters.Add("@SuperPass", SqlDbType.VarChar, 16).Value = Security.Encryption64.DecryptFromBase64String(Utilities.AppConfig.Item("Superpwd").ToString());
                cmm.Parameters.Add("@UserName", SqlDbType.VarChar, 20).Value = myLogonInfo.UserName;
                //cmm.Parameters.Add("@UserPass", SqlDbType.VarChar, 16).Value = myLogonInfo.UserPass;
                cmm.Parameters.Add("@UserIsSuperUser", SqlDbType.Bit, 0).Value = 0;
                cmm.Parameters["@UserIsSuperUser"].Direction = ParameterDirection.InputOutput;
                cmm.Parameters.Add("@FullUserName", SqlDbType.VarChar, 50).Value = "";
                cmm.Parameters["@FullUserName"].Direction = ParameterDirection.InputOutput;
                cmm.Parameters.Add("@User_ID", SqlDbType.BigInt, 0).Value = -1;
                cmm.Parameters["@User_ID"].Direction = ParameterDirection.InputOutput;
                cmm.Parameters.Add("@Message", SqlDbType.VarChar, 250).Value = "";
                cmm.Parameters["@Message"].Direction = ParameterDirection.InputOutput;
                cmm.Parameters.Add("@Machine_Name", SqlDbType.VarChar, 250).Value = myLogonInfo.ClientMachineName;
                cmm.Parameters.Add("@ClientIP", SqlDbType.VarChar, 250).Value = myLogonInfo.ClientIP;
                cmm.Parameters.Add("@ClientMAC", SqlDbType.VarChar, 250).Value = myLogonInfo.ClientMAC;
                cmm.Parameters.Add("@SessionMessage", SqlDbType.VarChar, 250).Value = "";
                cmm.Parameters["@SessionMessage"].Direction = ParameterDirection.InputOutput;
                cmm.ExecuteNonQuery();

                if (Convert.ToString(cmm.Parameters["@Message"].Value) != "OK")
                {
                    if (Convert.ToString(cmm.Parameters["@SessionMessage"].Value) == "")
                        throw new Exception(Convert.ToString(cmm.Parameters["@Message"].Value));
                }
                else //'   *****   Logon Succeeded
                {
                    if (Convert.ToBoolean(cmm.Parameters["@UserIsSuperUser"].Value))
                    { UserIsSuperUser = true; }

                    myUserID = Convert.ToInt64(cmm.Parameters["@User_ID"].Value);
                    myLogonResult = new LogonResult(LogonState.Succeeded, Convert.ToString(cmm.Parameters["@FullUserName"].Value), cmm.Connection.ToString(), UserIsSuperUser,
                                                    myUserID, myLogonInfo.UserName, myLogonInfo.SQLServer, String.Empty, cnLogin.Database);
                    myLogonResult.LogonInfoUsed = myLogonInfo;
                    //  ****    Open User Session
                    UserSessionOpen(myLogonResult);
                }
            }
            catch (Exception exLogon)
            {
                // Exceptions.ServiceExceptionHandler.HandleException(-1, string.Empty, exLogon); 
                throw new LogonException(exLogon);
            }
            finally
            {
                if (cnLogin.State != ConnectionState.Closed)
                { cnLogin.Close(); }
            }
            myLogonResult.SetMainDBName(cnLogin.Database);
            return myLogonResult;
        }

        public static Boolean IsValidUser(String UserName)
        {
            SqlCommand cmm = new SqlCommand();
            cmm.CommandText = "select COUNT(*) from Main.tblUser where fldUserName=@UserName and fldActiveUser=1";
            cmm.Parameters.Add("@UserName", SqlDbType.VarChar).Value = UserName;

            using (SqlConnection cn = Data.DataAccess.GetCn(Data.DBConnectionType.MainDB))
            {
                try
                {
                    cn.Open();
                    cmm.Connection = cn;
                    int result = (int)cmm.ExecuteScalar();

                    if (result == 1)
                    {
                        return true;
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    cn.Close();
                }
            }
            return false;
        }

        public static Boolean IsValidUPAUser(String UserName)
        {
            SqlCommand cmm = new SqlCommand();
            cmm.CommandText = "select COUNT(*) from Main.tblUPAUser where fldUserName=@UserName and fldActiveUser=1";
            cmm.Parameters.Add("@UserName", SqlDbType.VarChar).Value = UserName;

            using (SqlConnection cn = Data.DataAccess.GetCn(Data.DBConnectionType.MainDB))
            {
                try
                {
                    cn.Open();
                    cmm.Connection = cn;
                    int result = (int)cmm.ExecuteScalar();

                    if (result == 1)
                    {
                        return true;
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    cn.Close();
                }
            }
            return false;
        }

        public void UserSessionOpen(LogonResult LogResult)
        {

            SqlCommand Cmm = new SqlCommand();
            Cmm.CommandType = CommandType.StoredProcedure;
            Cmm.CommandText = "Main.spUserSessionAdd";

            Cmm.Parameters.Add("@Session_ID", SqlDbType.UniqueIdentifier).Value = LogResult.Session_ID;
            Cmm.Parameters.Add("@User_ID", SqlDbType.BigInt, 0).Value = LogResult.User_ID;
            Cmm.Parameters.Add("@UserName", SqlDbType.VarChar, 50).Value = LogResult.LogonName;
            Cmm.Parameters.Add("@MachineName", SqlDbType.VarChar, 250).Value = LogResult.LogonInfoUsed.ClientMachineName;
            Cmm.Parameters.Add("@ClientHostName", SqlDbType.VarChar, 250).Value = LogResult.LogonInfoUsed.ClientHostName;
            Cmm.Parameters.Add("@ClientDomain", SqlDbType.VarChar, 250).Value = LogResult.LogonInfoUsed.ClientDomain;
            Cmm.Parameters.Add("@ClientIP", SqlDbType.VarChar, 250).Value = LogResult.LogonInfoUsed.ClientIP;
            Cmm.Parameters.Add("@ClientMAC", SqlDbType.VarChar, 250).Value = LogResult.LogonInfoUsed.ClientMAC;

            Data.DataConnect.ExecCMM(LogResult, ref Cmm, Data.DBConnectionType.MainDB);

        }

        /// <summary>Closes a User Session</summary>
        /// <param name="UserInfo">The User Info</param>
        public static void UserSessionClose(Security.IUser UserInfo)
        {
            SqlCommand Cmm = new SqlCommand();
            Cmm.CommandType = CommandType.StoredProcedure;
            Cmm.CommandText = "Main.spUserSessionClose";

            Cmm.Parameters.Add("@Session_ID", SqlDbType.UniqueIdentifier).Value = UserInfo.Session_ID;

            Data.DataConnect.ExecCMM(UserInfo, ref Cmm, Data.DBConnectionType.MainDB);

        }

        /// <summary>Keeps a User Session alive. Returns true if the session is kept alive</summary>
        /// <param name="UserInfo">The User Info</param>
        public static bool UserSessionKeepAlive(Security.IUser UserInfo)
        {
            SqlCommand Cmm = new SqlCommand();
            Cmm.CommandType = CommandType.StoredProcedure;
            Cmm.CommandText = "Main.spUserSessionUpdate";

            Cmm.Parameters.Add("@Session_ID", SqlDbType.UniqueIdentifier).Value = UserInfo.Session_ID;
            Cmm.Parameters.Add("@IsAlive", SqlDbType.Bit).Value = false;
            Cmm.Parameters["@IsAlive"].Direction = ParameterDirection.InputOutput;

            Data.DataConnect.ExecCMM(UserInfo, ref Cmm, Data.DBConnectionType.MainDB);
            return Convert.ToBoolean(Cmm.Parameters["@IsAlive"].Value);
        }

        public bool AddRefreshToken(RefreshToken token)
        {
            bool retval = false;
            SqlConnection cnLogin = null;
            try
            { 
                SqlCommand cmm = new SqlCommand(); 
                cnLogin = Data.DataAccess.GetCn(Data.DBConnectionType.MainDB);
                cmm.CommandType = CommandType.StoredProcedure;
                //cmm.CommandText = "Main.spUserValidateWithoutPwd";
                cmm.CommandText = "Main.spRefreshTokenAddUpdate";
                cmm.Connection = cnLogin;
                if (cmm.Connection.State == ConnectionState.Closed)
                {
                    cmm.Connection.Open();
                }

                cmm.Parameters.AddWithValue("@user_ID", token.fldUserId);
                cmm.Parameters.AddWithValue("@UserName", token.fldSubject);
                cmm.Parameters.AddWithValue("@WebClient_Id", token.fldWebClientId);
                cmm.Parameters.AddWithValue("@IssuedUtc", token.fldIssuedUtc);
                cmm.Parameters.AddWithValue("@ExpiresUtc", token.fldExpiresUtc);
                cmm.Parameters.AddWithValue("@ProtectedTicket", token.fldProtectedTicket);
                cmm.Parameters.AddWithValue("@RemoteIP", token.fldRemoteIP);
                var parm = new SqlParameter("@RefreshToken_Id", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                cmm.Parameters.Add(parm); 
                 
                cmm.ExecuteNonQuery();

                retval =  Convert.ToBoolean(Convert.ToInt64(cmm.Parameters["@RefreshToken_Id"].Value) > 0);
            }
            catch (Exception exLogon)
            { 
                throw new LogonException(exLogon);
            }
            finally
            {
                if (cnLogin.State != ConnectionState.Closed)
                { cnLogin.Close(); }
            }
            
            return retval;
        }

        public RefreshToken FindRefreshToken(long userId)
        {
            try
            {
                SqlCommand cmm = new SqlCommand();
                var cnLogin = Data.DataAccess.GetCn(Data.DBConnectionType.MainDB);
                cmm.CommandType = CommandType.Text;
                //cmm.CommandText = "Main.spUserValidateWithoutPwd";
                cmm.CommandText = "select * from Main.tblRefreshTokens where fldUser_ID =  " + userId;
                cmm.Connection = cnLogin;
                var da = new SqlDataAdapter(cmm);
                var dt = new DataTable();
                da.Fill(dt);

                var user = new RefreshToken();

                if (dt.Rows.Count <= 0) return null;
                user.fldUserId = Convert.ToInt64(dt.Rows[0]["fldUser_Id"]);
                user.fldProtectedTicket = Convert.ToString(dt.Rows[0]["fldProtectedTicket"]);
                user.fldExpiresUtc = Convert.ToDateTime(dt.Rows[0]["fldExpiresUtc"]);
                user.fldIssuedUtc = Convert.ToDateTime(dt.Rows[0]["fldIssuedUtc"]);
                user.fldSubject = Convert.ToString(dt.Rows[0]["fldUserName"]);
                user.fldWebClientId = Convert.ToString(dt.Rows[0]["fldWebClient_Id"]);
                user.fldRemoteIP = Convert.ToString(dt.Rows[0]["fldRemoteIP"]);
                return user; 
 
            }
            catch (Exception exLogon)
            {
                throw new LogonException(exLogon);
            }
           
        }

        public WebClient GetWebClient(string webClientId)
        {
            try
            {
                SqlCommand cmm = new SqlCommand();
                var cnLogin = Data.DataAccess.GetCn(Data.DBConnectionType.MainDB);
                cmm.CommandType = CommandType.Text; 
                cmm.CommandText = "select * from Main.tblWebClients where fldWebClient_ID = '" + webClientId + "'";
                cmm.Connection = cnLogin;
                var da = new SqlDataAdapter(cmm);
                var dt = new DataTable();
                da.Fill(dt);

                var client = new WebClient();

                if (dt.Rows.Count <= 0) return null;
                client.fldWebClientId = Convert.ToString(dt.Rows[0]["fldWebClient_Id"]);
                client.fldSecretCode = Convert.ToString(dt.Rows[0]["fldSecretCode"]);
                client.fldAllowedOrigin = Convert.ToString(dt.Rows[0]["fldAllowedOrigin"]);
                client.fldName = Convert.ToString(dt.Rows[0]["fldName"]);
                client.fldActive = Convert.ToBoolean(dt.Rows[0]["fldActive"]);
                client.fldApplicationType = Convert.ToInt32(dt.Rows[0]["fldApplicationType"]);
                client.fldRefreshTokenLifeTime = Convert.ToInt32(dt.Rows[0]["fldRefreshTokenLifeTime"]);
                return client;

            }
            catch (Exception exLogon)
            {
                throw new LogonException(exLogon);
            }
        }

        //public LogonResult VerifyLogin(LogonInfo myLogonInfo)
        //{

        //    LogonResult myLogonResult=null;
        //    //String myConnectString;
        //    SqlConnection cnLogin = null;

        //    try
        //    {

        //        //   ****    try to open the connection with SQL Server
        //        try
        //        {
        //            cnLogin = Data.DataAccess.GetCn(Data.DBConnectionType.MainDB);
        //            cnLogin.Open();

        //        }
        //        catch (Exception exCnOpen)
        //        {
        //            try
        //            {
        //                if (cnLogin.State != ConnectionState.Closed)
        //                { cnLogin.Close(); }
        //            }
        //            catch { }
        //           // Exceptions.ServiceExceptionHandler.HandleException(-1, string.Empty, exCnOpen); 
        //            throw new LogonException(exCnOpen);
        //        }

        //        //  *****   Now that the connection succeeded, we verify
        //        //   *****   whether the UserName and password
        //        SqlCommand cmm = new SqlCommand();
        //        System.Int64 myUserID;
        //        Boolean UserIsSuperUser = false;

        //        cmm.CommandType = CommandType.StoredProcedure;
        //        cmm.CommandText = "Main.spVerifyUserSession";
        //        cmm.Connection = cnLogin;

        //        cmm.Parameters.Add("@UserName", SqlDbType.VarChar, 20).Value = "";
        //        cmm.Parameters["@UserName"].Direction = ParameterDirection.InputOutput;
        //        cmm.Parameters.Add("@FullUserName", SqlDbType.VarChar, 50).Value = "";
        //        cmm.Parameters["@FullUserName"].Direction = ParameterDirection.InputOutput;
        //        cmm.Parameters.Add("@User_ID", SqlDbType.BigInt, 0).Value = myLogonInfo.User_ID;
        //        cmm.Parameters.Add("@Session_ID", SqlDbType.VarChar, 200).Value = myLogonInfo.Session_ID.ToString();
        //        cmm.ExecuteNonQuery();

        //            myUserID = Convert.ToInt64(cmm.Parameters["@User_ID"].Value);
        //            myLogonResult = new LogonResult(LogonState.Succeeded, Convert.ToString(cmm.Parameters["@FullUserName"].Value), cmm.Connection.ToString(), UserIsSuperUser,
        //                                            myLogonInfo.User_ID, Convert.ToString(cmm.Parameters["@UserName"].Value), myLogonInfo.SQLServer, String.Empty);
        //            myLogonResult.LogonInfoUsed = myLogonInfo;
        //            //  ****    Open User Session
        //            UserSessionOpen(myLogonResult);
        //    }
        //    catch (Exception exLogon)
        //    {
        //       // Exceptions.ServiceExceptionHandler.HandleException(-1, string.Empty, exLogon); 

        //        throw new LogonException(exLogon);
        //    }
        //    finally
        //    {
        //        if (cnLogin.State != ConnectionState.Closed)
        //        { cnLogin.Close(); }
        //    }
        //    myLogonResult.SetMainDBName(myLogonInfo.MainDB);
        //    return myLogonResult;
        //}
    }
}

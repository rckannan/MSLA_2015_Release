using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using MSLA.Server.Data;
using MSLA.Server.Security;
using Newtonsoft.Json;
using System.Globalization;
namespace MSLA.Server.WebAPI.Infra.Base
{
    public class MenuResult
    {
        public List<Menu> Menus { get; set; }
        public string MenuStates { get; set; }
    }
    public class MenuHelper
    {
        public static MenuResult GetMenuFromDb(Guid sessionId)
        { 
            try
            {
                return new MenuResult()
                {
                    Menus = ProcessStates(sessionId),
                   MenuStates = JsonConvert.SerializeObject(GetMenus(sessionId), Formatting.None)
                };
            }
            catch (Exception ex)
            {
                MSLA.Server.Exceptions.ServiceExceptionHandler.HandleException(null, ex, "Web API Call - GetMenuFromDB");
            }
            return null;
        }

        private static List<Menu> GetStates(Guid sessionId)
        {
            MSLA.Server.Data.DataCommand cmm = null;
            cmm = new Server.Data.DataCommand();
            cmm.CommandText = "select * from Main.tblWebMenu";
            cmm.CommandType = EnDataCommandType.Text;

            cmm.ConnectionType = DBConnectionType.MainDB;
            cmm.CommandTimeout = 30;

            var iLogon = ActionFilters.WebContainerManager.Get<Login.ILogonService>();
            IUser logonSession = iLogon.FetchLogonInfo(sessionId);


            var res = MSLA.Server.Data.DataConnect.FillDt(logonSession, MSLA.Server.Data.DataCommand.GetSQLCommand(cmm), cmm.ConnectionType);
            var menus = new List<Menu>();
            foreach (System.Data.DataRow drrow in res.Rows)
            {
                menus.Add(new Menu()
                {
                    fldWebMenu_Id = Convert.ToInt64(drrow["fldWebMenu_Id"]),
                    fldModule_ID = Convert.ToInt64(drrow["fldModule_ID"]),
                    fldParentMenu_ID = Convert.ToInt64(drrow["fldParentMenu_ID"]),
                    fldMenuName = Convert.ToString(drrow["fldMenuName"]),
                    fldstateName = Convert.ToString(drrow["fldstateName"]),
                    fldisabstract = Convert.ToBoolean(drrow["fldisabstract"]),
                    fldtemplateUrl = Convert.ToString(drrow["fldtemplateUrl"]),
                    fldurl = Convert.ToString(drrow["fldurl"]),
                    fldcontrollerName = Convert.ToString(drrow["fldcontrollerName"]),
                    fldcontrollerNameAs = Convert.ToString(drrow["fldcontrollerNameAs"]),
                    fldMenuType = Convert.ToString(drrow["fldMenuType"]),
                    fldIcon = Convert.ToString(drrow["fldIcon"]),
                    fldPriority = Convert.ToDecimal(drrow["fldPriority"])
                });
            }

            return menus;
        }

        #region States 

        private static MenuFinal GetMenus(Guid sessionID)
        {
            var menus = GetStates(sessionID);
            //List<MenuData> mnu = new List<MenuData>();
            MenuFinal mnu = new MenuFinal { Menudata = new List<MenuData>() };
            mnu.Menudata.AddRange(menus
                          .Where(c => c.fldParentMenu_ID == -1)
                         .Select(c => new MenuData()
                         {
                             fldParentMenu_ID = c.fldParentMenu_ID,
                             icon = c.fldIcon,

                             priority = c.fldPriority,

                             state = c.fldstateName,

                             name = c.fldMenuName,
                             type = c.fldMenuType,
                             children = GetChildrenMenu(menus, c.fldWebMenu_Id)

                         })
                          .ToList());

            HieararchyWalk(menus);

            return mnu;
        }

        private static List<MenuData> GetChildrenMenu(List<Menu> comments, Int64 parentId)
        {
            return comments
                    .Where(c => c.fldParentMenu_ID == parentId)
                    .Select(c => new MenuData()
                    {
                        fldParentMenu_ID = c.fldParentMenu_ID,
                        icon = c.fldIcon,

                        priority = c.fldPriority,

                        state = c.fldstateName,

                        name = c.fldMenuName,
                        type = c.fldMenuType,
                        children = GetChildrenMenu(comments, c.fldWebMenu_Id)

                    })
                    .ToList();
        }

        public static void HieararchyWalkMenu(List<Menu> hierarchy)
        {
            if (hierarchy != null)
            {
                foreach (var item in hierarchy)
                {
                    HieararchyWalk(item.fldChildren);
                }
            }
        }


        #endregion

        private static List<Menu> ProcessStates(Guid sessionId)
        {
            var menus = GetStates(sessionId);

            var menustruct = menus
                .Where(c => c.fldParentMenu_ID == -1)
                .Select(c => new Menu()
                {
                    fldMenuName = c.fldMenuName,
                    fldModule_ID = c.fldModule_ID,

                    fldParentMenu_ID = c.fldParentMenu_ID,
                    fldMenuType = c.fldMenuType,
                    fldPriority = c.fldPriority,
                    fldWebMenu_Id = c.fldWebMenu_Id,
                    fldstateName = c.fldstateName,
                    fldisabstract = c.fldisabstract,
                    fldurl = c.fldurl,
                    fldcontrollerName = c.fldcontrollerName,

                    fldcontrollerNameAs = c.fldcontrollerNameAs,
                    fldtemplateUrl = c.fldtemplateUrl,

                    fldChildren = GetChildren(menus, c.fldWebMenu_Id)

                })
                .ToList();

            HieararchyWalk(menustruct);

            return menustruct;
        }

        private static List<Menu> GetChildren(List<Menu> comments, Int64 parentId)
        {
            return comments
                    .Where(c => c.fldParentMenu_ID == parentId)
                    .Select(c => new Menu()
                    {
                        fldMenuName = c.fldMenuName,
                        fldModule_ID = c.fldModule_ID,

                        fldParentMenu_ID = c.fldParentMenu_ID,
                        fldMenuType = c.fldMenuType,
                        fldPriority = c.fldPriority,
                        fldWebMenu_Id = c.fldWebMenu_Id,
                        fldstateName = c.fldstateName,
                        fldisabstract = c.fldisabstract,
                        fldurl = c.fldurl,
                        fldcontrollerName = c.fldcontrollerName,

                        fldcontrollerNameAs = c.fldcontrollerNameAs,
                        fldtemplateUrl = c.fldtemplateUrl,

                        fldChildren = GetChildren(comments, c.fldWebMenu_Id)

                    })
                    .ToList();
        }

        private static void HieararchyWalk(List<Menu> hierarchy)
        {
            if (hierarchy != null)
            {
                foreach (var item in hierarchy)
                {
                    HieararchyWalk(item.fldChildren);
                }
            }
        }


        public static void PushException(ExceptionLog log, Guid sessionId)
        {
            MSLA.Server.Data.DataCommand cmm = null;
            cmm = new Server.Data.DataCommand();
            cmm.CommandText = "Main.spWebExceptionLogsAdd";
            cmm.CommandType = EnDataCommandType.StoredProcedure;

            cmm.ConnectionType = DBConnectionType.MainDB;

            var param = new MSLA.Server.Data.DataParameter()
            {
                ParameterName = "@User_ID",
                DBType = DataParameter.EnDataParameterType.BigInt,
                Direction = DataParameter.EnParameterDirection.Input,
                Value = log.user_ID 
            };
            cmm.Parameters.Add(param);

            param = new MSLA.Server.Data.DataParameter()
            {
                ParameterName = "@WebClient_Id",
                DBType = DataParameter.EnDataParameterType.NVarChar,
                Direction = DataParameter.EnParameterDirection.Input,
                Size =  128,
                Value = log.fldWebClient_Id
            };
            cmm.Parameters.Add(param);

            param = new MSLA.Server.Data.DataParameter()
            {
                ParameterName = "@LoggedOn",
                DBType = DataParameter.EnDataParameterType.DateTime,
                Direction = DataParameter.EnParameterDirection.Input, 
                Value = log.timestamp  
            };
            cmm.Parameters.Add(param);

            param = new MSLA.Server.Data.DataParameter()
            {
                ParameterName = "@Exception",
                DBType = DataParameter.EnDataParameterType.VarChar,
                Direction = DataParameter.EnParameterDirection.Input,
                Size = 8000,
                Value = log.Ex
            };
            cmm.Parameters.Add(param);

            param = new MSLA.Server.Data.DataParameter()
            {
                ParameterName = "@status",
                DBType = DataParameter.EnDataParameterType.VarChar,
                Direction = DataParameter.EnParameterDirection.Input,
                Size = 8000,
                Value = log.status
            };
            cmm.Parameters.Add(param);

            param = new MSLA.Server.Data.DataParameter()
            {
                ParameterName = "@statusText",
                DBType = DataParameter.EnDataParameterType.VarChar,
                Direction = DataParameter.EnParameterDirection.Input,
                Size = 8000,
                Value = log.statusText
            };
            cmm.Parameters.Add(param);

            param = new MSLA.Server.Data.DataParameter()
            {
                ParameterName = "@stack",
                DBType = DataParameter.EnDataParameterType.VarChar,
                Direction = DataParameter.EnParameterDirection.Input,
                Size = 8000,
                Value = log.stack
            };
            cmm.Parameters.Add(param);

            param = new MSLA.Server.Data.DataParameter()
            {
                ParameterName = "@stackDetail",
                DBType = DataParameter.EnDataParameterType.VarChar,
                Direction = DataParameter.EnParameterDirection.Input,
                Size = 8000,
                Value = log.stackArg
            };
            cmm.Parameters.Add(param);

            param = new MSLA.Server.Data.DataParameter()
            {
                ParameterName = "@StateName",
                DBType = DataParameter.EnDataParameterType.VarChar,
                Direction = DataParameter.EnParameterDirection.Input,
                Size = 250,
                Value = log.menu
            };
            cmm.Parameters.Add(param);

           

            var iLogon = ActionFilters.WebContainerManager.Get<Login.ILogonService>();
            IUser logonSession = iLogon.FetchLogonInfo(sessionId);
            var cmd = MSLA.Server.Data.DataCommand.GetSQLCommand(cmm);

            MSLA.Server.Data.DataConnect.ExecCMM(logonSession, ref cmd, cmm.ConnectionType);
             
        }

        public static void PushFeedback(Feedbacks log, Guid sessionId)
        {
            MSLA.Server.Data.DataCommand cmm = null;
            cmm = new Server.Data.DataCommand();
            cmm.CommandText = "Main.spWebClientFeedbackAdd";
            cmm.CommandType = EnDataCommandType.StoredProcedure;

            cmm.ConnectionType = DBConnectionType.MainDB; 

            var param = new MSLA.Server.Data.DataParameter()
            {
                ParameterName = "@User_ID",
                DBType = DataParameter.EnDataParameterType.BigInt,
                Direction = DataParameter.EnParameterDirection.Input,
                Value = log.user_ID
            };
            cmm.Parameters.Add(param);

            param = new MSLA.Server.Data.DataParameter()
            {
                ParameterName = "@WebClient_Id",
                DBType = DataParameter.EnDataParameterType.NVarChar,
                Direction = DataParameter.EnParameterDirection.Input,
                Size = 128,
                Value = log.webClientID
            };
            cmm.Parameters.Add(param);

            param = new MSLA.Server.Data.DataParameter()
            {
                ParameterName = "@LoggedOn",
                DBType = DataParameter.EnDataParameterType.DateTime,
                Direction = DataParameter.EnParameterDirection.Input,
                Value = log.updatedOn
            };
            cmm.Parameters.Add(param);

            param = new MSLA.Server.Data.DataParameter()
            {
                ParameterName = "@Menu",
                DBType = DataParameter.EnDataParameterType.VarChar,
                Direction = DataParameter.EnParameterDirection.Input,
                Size = 250,
                Value = log.menu
            };
            cmm.Parameters.Add(param);

            param = new MSLA.Server.Data.DataParameter()
            {
                ParameterName = "@Comments",
                DBType = DataParameter.EnDataParameterType.VarChar,
                Direction = DataParameter.EnParameterDirection.Input,
                Size = 8000,
                Value = log.description
            };
            cmm.Parameters.Add(param); 

            var iLogon = ActionFilters.WebContainerManager.Get<Login.ILogonService>();
            IUser logonSession = iLogon.FetchLogonInfo(sessionId);
            var cmd = MSLA.Server.Data.DataCommand.GetSQLCommand(cmm);

            MSLA.Server.Data.DataConnect.ExecCMM(logonSession, ref cmd, cmm.ConnectionType);

        }

        public static DateTime ParseDate(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            string[] formats = {"ddd MMM dd yyyy hh:mm:ss 'UTC'zzz",
                           "ddd MMM d yyyy hh:mm:ss 'UTC'zzz",
                            "ddd MMM d hh:mm:ss 'UTC'zzz yyyy",
                            "ddd MMM dd hh:mm:ss 'UTC'zzz yyyy",
                            "ddd MMM dd yyyy hh:mm:ss 'GMT'zzz",
                           "ddd MMM d yyyy hh:mm:ss 'GMT'zzz",
                           "ddd MMM d hh:mm:ss 'GMT'zzz yyyy",
                            "ddd MMM dd hh:mm:ss 'GMT'zzz yyyy",
                            "dd-MM-yyyy",
                            "yyyy-MM-dd'T'hh:mm:ss"
                           };
            DateTime fecha;
            //Try the default
            if (!DateTime.TryParse(value, out fecha))
            {
                if (!DateTime.TryParseExact(value, formats,
                                           new CultureInfo("en-US"),
                                           DateTimeStyles.None, out fecha))
                {
                    if (value.Length > 24)
                    {
                        return ParseDate(value.Substring(0, 24));
                    }
                    else
                    {
                        throw new FormatException();
                    }
                }

            }
            return fecha;

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace MSLA.Server.WebAPI.Infra.Base
{


    //public class WebClient
    //{
    //    public string fldWebClientId { get; set; } 
    //    public string fldSecretCode { get; set; } 
    //    public string fldName { get; set; }
    //    public int fldApplicationType { get; set; }
    //    public bool fldActive { get; set; }
    //    public int fldRefreshTokenLifeTime { get; set; } 
    //    public string fldAllowedOrigin { get; set; }
    //}

    public enum ApplicationTypes
    {
        JavaScript = 0,
        NativeConfidential = 1
    }

    //public class User
    //{
    //    public Int64 fldUserId { get; set; }
    //    public string fldUserName { get; set; }
    //    public string fldFullUserName { get; set; }
    //    public string fldEmailAddress { get; set; }
    //    public bool fldActiveUser { get; set; }
    //}

    //public class RefreshToken
    //{ 
    //    public Int64 fldUserId { get; set; } 
    //    public string fldSubject { get; set; } 
    //    public string fldWebClientId { get; set; }
    //    public DateTime fldIssuedUtc { get; set; }
    //    public DateTime fldExpiresUtc { get; set; } 
    //    public string fldProtectedTicket { get; set; } 
    //    public string fldRemoteIP { get; set; }
    //}

    public class Menu
    {
        public Int64 fldWebMenu_Id { get; set; }
        public Int64 fldModule_ID { get; set; }
        public Int64 fldParentMenu_ID { get; set; }
        public string fldMenuName { get; set; }
        public string fldstateName { get; set; }
        public bool fldisabstract { get; set; }
        public string fldtemplateUrl { get; set; }
        public string fldurl { get; set; }
        public string fldcontrollerName { get; set; }
        public string fldcontrollerNameAs { get; set; }
        public string fldMenuType { get; set; }

        public string fldIcon { get; set; }
        public decimal fldPriority { get; set; }

        public List<Menu> fldChildren { get; set; }
    }


    public class MenuData
    {
        public string name { get; set; }
        public string icon { get; set; }
        public string type { get; set; }
        public decimal priority { get; set; }
        public string state { get; set; }
        public List<MenuData> children { get; set; }
        public Int64 fldParentMenu_ID { get; set; }
    }

    public class MenuFinal
    {
        [JsonProperty("Menudatas")]
        public List<MenuData> Menudata { get; set; }
    }

    public class ExceptionLog
    {
        public Int64 user_ID { get; set; }
        public string fldWebClient_Id { get; set; } 
        public string Ex { get; set; }
        public string status { get; set; } 
        public string statusText { get; set; }
        public string stack { get; set; } 
        public string stackArg { get; set; }
        public DateTime timestamp { get; set; } 
        public string menu { get; set; } 
        public ExceptionLog()
        {
            fldWebClient_Id = string.Empty;
        }
    }

    public class Feedbacks
    {
        public Int64 user_ID { get; set; }
        public string webClientID { get; set; } 
        public string menu { get; set; }
        public string description { get; set; }
        public DateTime updatedOn { get; set; } 
        public Feedbacks()
        {
        }
    }
}
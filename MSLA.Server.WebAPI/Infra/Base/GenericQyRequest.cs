using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSLA.Server.WebAPI.Infra.Base
{
    public class GenericQyRequest
    {
        public string RequestObject { get; set; }
        public List<Params> Params { get; set; }
        public int TimeOut { get; set; }

        public GenericQyRequest()
        {
            RequestObject = string.Empty;
            Params = new List<Params>();
            TimeOut = 30;
        }
    }

    public struct Params
    {
        public string Name { get; set; }
        public object value { get; set; }
        public MSLA.Server.Data.DataParameter.EnParameterDirection direction { get; set; }
        public MSLA.Server.Data.DataParameter.EnDataParameterType ParamType { get; set; }
    }
}
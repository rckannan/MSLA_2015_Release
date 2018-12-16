using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using MSLA.Server.Data;

namespace MSLA.Server.WebAPI.Infra.Base
{
    public interface IDBQuerier
    {
        QueryObject ReadXMLQuery(string ObjectName);
        MSLA.Server.Data.DataCommand GenerateMSLAQueryOb(GenericQyRequest reqObj);
    }

    public class DBQuerier : IDBQuerier
    {
        static String templatepath;
        static XDocument xdoc;
        static QueryObject _QueryObject;
        public DBQuerier()
        {
            templatepath = ConfigurationManager.AppSettings["FileTemplatePath"];
            xdoc = XDocument.Load(templatepath);
        }

        public DataCommand GenerateMSLAQueryOb(GenericQyRequest reqObj)
        {
            MSLA.Server.Data.DataCommand cmm = null;
            try
            {
                QueryObject qObj = new QueryObject();
                //if (!(_cachedItems.TryGetValue(reqObj.RequestObject, out qObj)))
                //{
                //    qObj = XMLHelper.ReadXMLQuery(reqObj.RequestObject);
                //    _cachedItems.TryAdd(reqObj.RequestObject, qObj);
                //}

                qObj = XMLHelper.ReadXMLQuery(reqObj.RequestObject);

                if (qObj.Query == null)
                {
                    return null;
                }

                //Load the MSLA Object
                cmm = new Server.Data.DataCommand();
                cmm.CommandText = qObj.Query;
                cmm.CommandType = qObj.EnDataCommandType;

                cmm.ConnectionType = qObj.DBConnectionType;
                cmm.CommandTimeout = qObj.timeOut;

                //Load the params
                if (reqObj.Params.Count() > 0)
                {
                    foreach (var param in reqObj.Params)
                    {
                        var param1 = new Server.Data.DataParameter();
                        param1.ParameterName = param.Name;
                        param1.Value = param.value;
                        param1.Direction = param.direction;
                        param1.DBType = param.ParamType;

                        cmm.Parameters.Add(param1);
                    }
                }
            }
            catch (Exception ex)
            {
                MSLA.Server.Exceptions.ServiceExceptionHandler.HandleException(null, ex, "Web API Call");
            }
            return cmm;
        }

        public QueryObject ReadXMLQuery(string ObjectName)
        {

            var selectedTemplate = xdoc.Descendants("query").Where(x => (string)x.Attribute("name") == ObjectName);

            if (selectedTemplate == null) return _QueryObject;

            _QueryObject = new QueryObject();
            _QueryObject.ObjectName = ObjectName;
            _QueryObject.Query = selectedTemplate.Select(x => x.Element("BaseQuery").Value).FirstOrDefault();
            _QueryObject.DBConnectionType = (MSLA.Server.Data.DBConnectionType)Convert.ToInt32(selectedTemplate.Select(x => x.Attribute("DBType").Value).FirstOrDefault());
            _QueryObject.EnDataCommandType = (MSLA.Server.Data.EnDataCommandType)Convert.ToInt32(selectedTemplate.Select(x => x.Attribute("QueryType").Value).FirstOrDefault());

            return _QueryObject;
        }
    } 
     

    public class QueryObject
    {
        public string ObjectName { get; set; }
        public string Query { get; set; }
        public MSLA.Server.Data.DBConnectionType DBConnectionType { get; set; }
        public MSLA.Server.Data.EnDataCommandType EnDataCommandType { get; set; }
        public int timeOut { get; set; }
    }
}
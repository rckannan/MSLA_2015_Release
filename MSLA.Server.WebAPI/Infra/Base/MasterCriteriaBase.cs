using System.Collections.Generic;

namespace MSLA.Server.WebAPI.Infra.Base
{
    public class MasterCriteriaBase
         : MSLA.Server.BO.IMasterCriteria
    {
        long docMaster_ID = -1;
        string docMasterType = string.Empty;
        Server.Security.SimpleUserInfo _UserInfo;
        //Guid _Session_ID = new Guid();
        //long _User_ID = -1;
        Dictionary<string, object> _propertyCollection = new Dictionary<string, object>();

        //[DataMember]
        public long DocMaster_ID
        {
            get { return docMaster_ID; }
            set { docMaster_ID = value; }
        }

        //[DataMember]
        public string DocMasterType
        {
            get { return docMasterType; }
            set { docMasterType = value; }
        }

        //[DataMember]
        public Server.Security.SimpleUserInfo UserInfo
        {
            get { return _UserInfo; }
            set { _UserInfo = value; }
        }
        //public Guid Session_ID
        //{
        //    get { return _Session_ID; }
        //    set { _Session_ID = value; }
        //}

        //[DataMember]
        //public long User_ID
        //{
        //    get { return _User_ID; }
        //    set { _User_ID = value; }
        //}

        //[DataMember]
        public Dictionary<string, object> PropertyCollection
        {
            get { return _propertyCollection; }
            set { _propertyCollection = value; }
        }
    }
}
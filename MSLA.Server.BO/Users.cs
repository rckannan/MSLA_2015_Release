using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using MSLA.Server.Rules;

namespace MSLA.Server.BO
{
    [Serializable()]
    public class Users : MasterBase
    {
        #region Properties 
        public Int64 fldUser_ID { get; set; }  
        public string fldUserName { get; set; }
        public object fldPassword { get; set; }
        public string fldFullUserName { get; set; }
        public bool fldActiveUser { get; set; }
        public DateTime fldLastUpdated { get; set; }
        public int fldFailedAttempt { get; set; }
        public DateTime fldPasswordLastUpdated { get; set; }
        public bool fldForceChangePassword { get; set; }
        public string fldEmailAddress { get; set; }
        public string fldGroupName { get; set; }
        public string fldDomainName { get; set; }
        public Int16 fldUserStatus { get; set; }
        public Int16 fldManualImportAccessStatus { get; set; }
        public bool fldIsExportAllowed { get; set; }
        #endregion
        protected Users(MSLA.Server.Security.IUser userInfo):base(userInfo , MSLA.Server.Data.DBConnectionType.MainDB)
        {
            OnInitilize();
        }

        private void OnInitilize()
        {
            
        }
        protected override void FetchOrCreate(IMasterCriteria mastCriteria)
        {
            if (mastCriteria.DocMaster_ID == -1)
            {
                fldUser_ID = -1;
                fldActiveUser = true;
                fldIsExportAllowed = fldForceChangePassword = false;
                fldDomainName =
                    fldEmailAddress = fldFullUserName = fldGroupName = fldUserName  = string.Empty;
                fldFailedAttempt = fldUserStatus = fldManualImportAccessStatus = 0;
            }
            else
            { 
                MSLA.Server.Entity.EntityManager.FillMaster(this, "main", "tblUser", "fldUser_ID", mastCriteria.DocMaster_ID); 
            }
        }

        
        protected override void AfterSave(MasterSaveResult localSaveResult)
        {
            this.SetDocMaster_ID(localSaveResult.fldMasterDoc_ID);
            this.fldLastUpdated  = localSaveResult.fldLastUpdated;
        }

        protected override MasterSaveResult SaveControlTran(SqlConnection cn, SqlTransaction cnTran)
        {
            MSLA.Server.Base.MasterSaveBase.MasterSaveResult SaveResult = new MSLA.Server.Base.MasterSaveBase.MasterSaveResult();
            MSLA.Server.Entity.EntityUnit euControl = new MSLA.Server.Entity.EntityUnit("main", "tblUser", MSLA.Server.Entity.EntityUnit.enTableType.Master);
             euControl.SQLConnect = cn;
            euControl.SQLTran = cnTran;
            euControl.PrimaryKey = "fldUser_ID";
            euControl.SaveControl(this);

            SaveResult.fldMasterDoc_ID = Convert.ToInt64(euControl.Parameters["@User_ID"].Value); 
            SaveResult.fldLastUpdated = Convert.ToDateTime(euControl.Parameters["@LastUpdated"].Value);


            return SaveResult;
        }
        protected override void SetDocMaster_ID(long itemId)
        {
            this.fldUser_ID = itemId;
        }

        protected override void ValidateBeforeSave()
        {
            MSLA.Server.Rules.BrokenRule BrRule;
            if (this.fldUserName == string.Empty)
            {
                BrRule = new MSLA.Server.Rules.BrokenRule(this.ToString(), "fldUserName", "Please enter user Name.");
                BrokenSaveRules.Add(BrRule);
            }

            if (this.fldUserName != string.Empty)
            {
                var qry = "select count(1) as Count from Main.tblUser where fldUser_ID != @user_ID and fldUserName =  @UserName";
                using (var cmd = new SqlCommand(qry))
                {
                    cmd.Parameters.Add("@user_ID", SqlDbType.BigInt, 0).Value = this.fldUser_ID ;
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 250).Value =  this.fldUserName ;
                    var Result =  MSLA.Server.Data.DataConnect.FillDt(cmd, this.UserInfo, MSLA.Server.Data.DBConnectionType.MainDB) ;
                    if (Convert.ToInt32(Result.Rows[0]["Count"])  > 0)
                    {
                        BrRule = new MSLA.Server.Rules.BrokenRule("fldUser", "fldUser", "User already exist.");
                        BrokenSaveRules.Add(BrRule);
                    }
                } 
            }
        }

        protected override void ValidateBeforeDelete()
        {
            throw new System.NotImplementedException();
        }

        protected override MasterSaveResult DeleteControlTran(SqlConnection cn, SqlTransaction cnTran)
        {
            throw new System.NotImplementedException();
        }

        protected override string MasterTableName => "main.tblUser";
    }
}

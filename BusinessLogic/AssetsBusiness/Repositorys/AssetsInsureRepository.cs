using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.AssetsBusiness.Models.AssetsInsure;
using BaseCommon.Models;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsInsure
    {
        public string AssetsId { get; set; }
        public string InsureCompany { get; set; }
        public string InsureAmount { get; set; }
        public string Remark { get; set; }
        public string ApproveState { get; set; }
        public string CreateId { get; set; }
        public string CreateTime { get; set; }
    }

    public class AssetsInsureRepository : ApproveMasterRepository
    {

        public AssetsInsureRepository()
        {
            DefaulteGridSortField = "AssetsInsureNo";
            MasterTable = "AssetsInsure";
        }


        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            ApproveListCondition acondition = condition as ApproveListCondition;
            if (DataConvert.ToString(acondition.ListMode) != "")
                wcd.DBPara.Add("approver", acondition.Approver);
            ListModel model = JsonHelper.Deserialize<ListModel>(acondition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.AssetsInsureNo) != "")
            {
                wcd.Sql += @" and AssetsInsure.assetsInsureNo like '%'+@assetsInsureNo+'%'";
                wcd.DBPara.Add("assetsInsureNo", model.AssetsInsureNo);
            }
            if (DataConvert.ToString(model.AssetsInsureName) != "")
            {
                wcd.Sql += @" and AssetsInsure.assetsInsureName like '%'+@assetsInsureName+'%'";
                wcd.DBPara.Add("assetsInsureName", model.AssetsInsureName);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            ApproveListCondition acondition = condition as ApproveListCondition;
            string lsql = " where 1=1";
            if (DataConvert.ToString(acondition.ListMode) != "")
            {
                if (DataConvert.ToString(acondition.ListMode) == "approve")
                {
                    lsql = @",AppApprove
                     where AppApprove.tableName='AssetsInsure' and AppApprove.approveState='O'
                      and AssetsInsure.AssetsInsureId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(acondition.ListMode) == "reapply")
                {
                    lsql = @" where AssetsInsure.createId=@approver and AssetsInsure.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select  AssetsInsure.AssetsInsureId AssetsInsureId,
                        AssetsInsure.AssetsInsureNo AssetsInsureNo,
                        AssetsInsure.AssetsInsureName AssetsInsureName,
                        (select userName from AppUser where AssetsInsure.createId=AppUser.userId) createId,
                        AssetsInsure.createTime createTime ,
                        (select userName from AppUser where AssetsInsure.updateId=AppUser.userId) updateId,
                        AssetsInsure.updateTime updateTime ,
                        AssetsInsure.updatePro updatePro
                from AssetsInsure  {0} " , lsql);
            return subViewSql;
        }


        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            model.ApproveTableName = "AssetsInsure";
            model.ApprovePkField = "assetsInsureId";
            if (formMode != "new" && formMode != "new2")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("AssetsInsureId", primaryKey);
                string sql = @"select * from AssetsInsure where AssetsInsureId=@AssetsInsureId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.AssetsInsureId = primaryKey;
                model.AssetsInsureNo = DataConvert.ToString(dr["AssetsInsureNo"]);
                model.AssetsInsureName = DataConvert.ToString(dr["AssetsInsureName"]);
                model.InsureDate = DataConvert.ToDateTime(dr["insureDate"]);
            }
        }


        public override DataTable GetEntryGridDataTable(Dictionary<string, object> paras, string formMode, string primaryKey, string formVar)
        {
            if (formMode == "new" || formMode == "new2")
            {
                return null;
            }
            else
            {
                if (!paras.ContainsKey("AssetsInsureId"))
                    paras.Add("AssetsInsureId", primaryKey);
                string sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                    AssetsInsureDetail.InsureCompany InsureCompany,
                    AssetsInsureDetail.InsureAmount InsureAmount,
                    AssetsInsureDetail.remark Remark,
                AssetsInsureDetail.approveState ApproveState,
                AssetsInsureDetail.createId CreateId,
                AssetsInsureDetail.createTime CreateTime
                from AssetsInsureDetail,Assets where AssetsInsureDetail.assetsId=Assets.assetsId and AssetsInsureDetail.AssetsInsureId=@AssetsInsureId  ");
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                return dtGrid;
            }
            
        }

        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsInsure where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsInsure";
            DataRow dr = dt.NewRow();
            string assetsInsureId = IdGenerator.GetMaxId(dt.TableName);
            int retApprove = InitFirstApproveTask("AssetsInsure", "AssetsInsureId", assetsInsureId, viewTitle, "", sysUser.UserId);
            EntryModel myModel = model as EntryModel;
            dr["AssetsInsureNo"] = myModel.AssetsInsureNo;
            dr["AssetsInsureName"] = myModel.AssetsInsureName;
            dr["insureDate"] = DataConvert.ToDBObject(myModel.InsureDate);
            string updateType = "Add";
            if (retApprove != 0)
            {
                dr["approveState"] = "O";
                updateType = "ApproveAdd";
            }
            dr["AssetsInsureId"] = assetsInsureId;
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            List<AssetsInsure> gridData = JsonHelper.JSONStringToList<AssetsInsure>(DataConvert.ToString(myModel.EntryGridString));
            foreach (AssetsInsure assetsInsure in gridData)
            {
                AddDetail(assetsInsure, assetsInsureId, sysUser, viewTitle, updateType);
                if (retApprove != 0)
                {
                    UpdateAssetsState(assetsInsure.AssetsId, "II", sysUser.UserId, viewTitle);
                }
            }

            return 1;
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsInsureId", pkValue);
            string sql = @"select * from AssetsInsure where AssetsInsureId=@AssetsInsureId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsInsure";
            string AssetsInsureId = DataConvert.ToString(dt.Rows[0]["AssetsInsureId"]);
            EntryModel myModel = model as EntryModel;
            dt.Rows[0]["AssetsInsureNo"] = myModel.AssetsInsureNo;
            dt.Rows[0]["AssetsInsureName"] = myModel.AssetsInsureName;
            dt.Rows[0]["insureDate"] = DataConvert.ToDBObject(myModel.InsureDate);
            string updateType = "Modified";
            if (formMode == "reapply")
            {
                dt.Rows[0]["approveState"] = "O";
                updateType = "Reapply";
            }
            Update5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            DeleteDetail(pkValue,sysUser,viewTitle);
            List<AssetsInsure> gridData = JsonHelper.JSONStringToList<AssetsInsure>(DataConvert.ToString(myModel.EntryGridString));
            foreach (AssetsInsure AssetsInsure in gridData)
            {
                AddDetail(AssetsInsure, pkValue, sysUser, viewTitle, updateType);
            }
            if (formMode == "reapply")
                InitFirstApproveTask("AssetsInsure", "AssetsInsureId", AssetsInsureId, viewTitle, formMode, sysUser.UserId);
            return 1;
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsInsureId", pkValue);
            string sql = @"select * from AssetsInsure where AssetsInsureId=@AssetsInsureId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsInsure";
            dt.Rows[0].Delete();
            DbUpdate.Update(dt);
            DeleteDetail(pkValue, sysUser, viewTitle);
            DeleteApproveData("AssetsInsure", pkValue, sysUser.UserId);
            return 1;
        }

        protected int AddDetail(AssetsInsure AssetsInsure, string AssetsInsureId, UserInfo sysUser, string viewTitle, string updateType)
        {
            string sql = @"select * from AssetsInsureDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsInsureDetail";
            DataRow dr = dt.NewRow();
            dr["AssetsInsureId"] = AssetsInsureId;
            dr["assetsId"] = AssetsInsure.AssetsId;
            dr["remark"] = AssetsInsure.Remark;
            dr["insureCompany"] = AssetsInsure.InsureCompany;
            dr["insureAmount"] = AssetsInsure.InsureAmount;
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            if (DataConvert.ToString(AssetsInsure.CreateId) != "")
            {
                if (updateType == "Reapply")
                    dr["approveState"] = "O";
                else
                    dr["approveState"] = AssetsInsure.ApproveState;
                dr["createId"] = AssetsInsure.CreateId;
                dr["createTime"] = AssetsInsure.CreateTime;
                Update5Field(dt, sysUser.UserId, viewTitle);
            }
            else
            {
                if (updateType == "ApproveAdd")
                    dr["approveState"] = "O";
                Create5Field(dt, sysUser.UserId, viewTitle);
            }
            return DbUpdate.Update(dt);
        }

        protected int DeleteDetail(string pkValue, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsInsureId", pkValue);
            string sql = @"select * from AssetsInsureDetail where AssetsInsureId=@AssetsInsureId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsInsureDetail";
            foreach (DataRow dr in dt.Rows)
            {
                UpdateAssetsState(DataConvert.ToString(dr["assetsId"]), "A", sysUser.UserId, viewTitle);
                dr.Delete();
            }
            return DbUpdate.Update(dt);
        }

        public override int DealEndApprove(string approvePkValue, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsInsureId", approvePkValue);
            string sql = @"select * from AssetsInsureDetail where assetsInsureId=@assetsInsureId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            foreach (DataRow dr in dtAssets.Rows)
            {
                UpdateAssetsState(DataConvert.ToString(dr["assetsId"]), "A", sysUser.UserId, viewTitle);
            }
            return 1;
        }

    }
}

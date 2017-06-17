using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.AssetsBusiness.Models.AssetsLease;
using BaseCommon.Models;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsLease
    {
        public string AssetsId { get; set; }
        public string LeaseCompany { get; set; }
        public string LeaseAmount { get; set; }
        public string Remark { get; set; }
        public string ApproveState { get; set; }
        public string CreateId { get; set; }
        public string CreateTime { get; set; }
    }

    public class AssetsLeaseRepository : ApproveMasterRepository
    {

        public AssetsLeaseRepository()
        {
            DefaulteGridSortField = "AssetsLeaseNo";
            MasterTable = "AssetsLease";
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
            if (DataConvert.ToString(model.AssetsLeaseNo) != "")
            {
                wcd.Sql += @" and AssetsLease.assetsLeaseNo like '%'+@assetsLeaseNo+'%'";
                wcd.DBPara.Add("assetsLeaseNo", model.AssetsLeaseNo);
            }
            if (DataConvert.ToString(model.AssetsLeaseName) != "")
            {
                wcd.Sql += @" and AssetsLease.assetsLeaseName like '%'+@AssetsLeaseName+'%'";
                wcd.DBPara.Add("assetsLeaseName", model.AssetsLeaseName);
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
                     where AppApprove.tableName='AssetsLease' and AppApprove.approveState='O'
                      and AssetsLease.AssetsLeaseId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(acondition.ListMode) == "reapply")
                {
                    lsql = @" where AssetsLease.createId=@approver and AssetsLease.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select AssetsLease.AssetsLeaseId AssetsLeaseId,
                        AssetsLease.AssetsLeaseNo AssetsLeaseNo,
                        AssetsLease.AssetsLeaseName AssetsLeaseName,
                        (select userName from AppUser where AssetsLease.createId=AppUser.userId) createId,
                        AssetsLease.createTime createTime ,
                        (select userName from AppUser where AssetsLease.updateId=AppUser.userId) updateId,
                        AssetsLease.updateTime updateTime ,
                        AssetsLease.updatePro updatePro
                from AssetsLease  {0} ",  lsql);
            return subViewSql;
        }


        public void  SetModel(string primaryKey, string formMode, EntryModel model)
        {
            model.ApproveTableName = "AssetsLease";
            model.ApprovePkField = "assetsLeaseId";
            if (formMode != "new" && formMode != "new2")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("AssetsLeaseId", primaryKey);
                string sql = @"select * from AssetsLease where AssetsLeaseId=@AssetsLeaseId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.AssetsLeaseId = primaryKey;
                model.AssetsLeaseNo = DataConvert.ToString(dr["assetsLeaseNo"]);
                model.AssetsLeaseName = DataConvert.ToString(dr["assetsLeaseName"]);
                model.LeaseDate = DataConvert.ToDateTime(dr["leaseDate"]);
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
                if (!paras.ContainsKey("AssetsLeaseId"))
                    paras.Add("AssetsLeaseId", primaryKey);
                string sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                    AssetsLeaseDetail.sellCompany sellCompany,
                    AssetsLeaseDetail.sellAmount sellAmount,
                    AssetsLeaseDetail.remark Remark,
                AssetsLeaseDetail.approveState ApproveState,
                AssetsLeaseDetail.createId CreateId,
                AssetsLeaseDetail.createTime CreateTime
                from AssetsLeaseDetail,Assets where AssetsLeaseDetail.assetsId=Assets.assetsId and AssetsLeaseDetail.AssetsLeaseId=@AssetsLeaseId  ");
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                return dtGrid;
            }
           
        }

        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsLease where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsLease";
            DataRow dr = dt.NewRow();
            string assetsLeaseId = IdGenerator.GetMaxId(dt.TableName);
            int retApprove = InitFirstApproveTask("AssetsLease", "AssetsLeaseId", assetsLeaseId, viewTitle, "", sysUser.UserId);
            EntryModel myModel = model as EntryModel;
            dr["assetsLeaseNo"] = myModel.AssetsLeaseNo;
            dr["assetsLeaseName"] = myModel.AssetsLeaseName;
            dr["leaseDate"] = DataConvert.ToDBObject(myModel.LeaseDate);
            string updateType = "Add";
            if (retApprove != 0)
            {
                dr["approveState"] = "O";
                updateType = "ApproveAdd";
            }
            dr["AssetsLeaseId"] = assetsLeaseId;
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            List<AssetsLease> gridData = JsonHelper.JSONStringToList<AssetsLease>(DataConvert.ToString(myModel.EntryGridString));
            foreach (AssetsLease assetsLease in gridData)
            {
                AddDetail(assetsLease, assetsLeaseId, sysUser, viewTitle, updateType);
                if (retApprove != 0)
                {
                    UpdateAssetsState(assetsLease.AssetsId, "LI", sysUser.UserId, viewTitle);
                }
            }

            return 1;
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsLeaseId", pkValue);
            string sql = @"select * from AssetsLease where AssetsLeaseId=@AssetsLeaseId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsLease";
            string AssetsLeaseId = DataConvert.ToString(dt.Rows[0]["AssetsLeaseId"]);
            EntryModel myModel = model as EntryModel;
            dt.Rows[0]["assetsLeaseNo"] = myModel.AssetsLeaseNo;
            dt.Rows[0]["assetsLeaseName"] = myModel.AssetsLeaseName;
            dt.Rows[0]["leaseDate"] = DataConvert.ToDBObject(myModel.LeaseDate);
            string updateType = "Modified";
            if (formMode == "reapply")
            {
                dt.Rows[0]["approveState"] = "O";
                updateType = "Reapply";
            }
            Update5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            DeleteDetail(pkValue,sysUser,viewTitle);
            List<AssetsLease> gridData = JsonHelper.JSONStringToList<AssetsLease>(DataConvert.ToString(myModel.EntryGridString));
            foreach (AssetsLease AssetsLease in gridData)
            {
                AddDetail(AssetsLease, pkValue, sysUser, viewTitle, updateType);
            }
            if (formMode == "reapply")
                InitFirstApproveTask("AssetsLease", "AssetsLeaseId", AssetsLeaseId, viewTitle, formMode, sysUser.UserId);
            return 1;
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsLeaseId", pkValue);
            string sql = @"select * from AssetsLease where AssetsLeaseId=@AssetsLeaseId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsLease";
            dt.Rows[0].Delete();
            DbUpdate.Update(dt);
            DeleteDetail(pkValue, sysUser, viewTitle);
            DeleteApproveData("AssetsLease", pkValue, sysUser.UserId);
            return 1;
        }

        protected int AddDetail(AssetsLease AssetsLease, string AssetsLeaseId, UserInfo sysUser, string viewTitle, string updateType)
        {
            string sql = @"select * from AssetsLeaseDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsLeaseDetail";
            DataRow dr = dt.NewRow();
            dr["AssetsLeaseId"] = AssetsLeaseId;
            dr["assetsId"] = AssetsLease.AssetsId;
            dr["remark"] = AssetsLease.Remark;
            dr["leaseCompany"] = AssetsLease.LeaseCompany;
            dr["leaseAmount"] = AssetsLease.LeaseAmount;
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            if (DataConvert.ToString(AssetsLease.CreateId) != "")
            {
                if (updateType == "Reapply")
                    dr["approveState"] = "O";
                else
                    dr["approveState"] = AssetsLease.ApproveState;
                dr["createId"] = AssetsLease.CreateId;
                dr["createTime"] = AssetsLease.CreateTime;
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
            paras.Add("AssetsLeaseId", pkValue);
            string sql = @"select * from AssetsLeaseDetail where AssetsLeaseId=@AssetsLeaseId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsLeaseDetail";
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
            paras.Add("assetsLeaseId", approvePkValue);
            string sql = @"select * from AssetsLeaseDetail where assetsLeaseId=@assetsLeaseId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            foreach (DataRow dr in dtAssets.Rows)
            {
                UpdateAssetsState(DataConvert.ToString(dr["assetsId"]), "A", sysUser.UserId, viewTitle);
            }
            return 1;
        }

    }
}

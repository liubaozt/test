using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using log4net;

namespace BusinessLogic.AssetsBusiness
{
    public class AssetsLease
    {
        public string AssetsId { get; set; }
        public string AssetsNo { get; set; }
        public string AssetsName { get; set; }
        public string AssetsTypeId { get; set; }
        public string AssetsClassId { get; set; }
        public string LeaseCompany { get; set; }
        public string LeaseAmount { get; set; }
        public string Remark { get; set; }
    }

    public class AssetsLeaseRepository : ApproveMasterRepository
    {

        public AssetsLeaseRepository()
        {
            DefaulteGridSortField = "AssetsLeaseNo";
        }


        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("AssetsLeaseNo") && DataConvert.ToString(paras["AssetsLeaseNo"]) != "")
                whereSql += @" and AssetsLease.AssetsLeaseNo like '%'+@AssetsLeaseNo+'%'";
            if (paras.ContainsKey("AssetsLeaseName") && DataConvert.ToString(paras["AssetsLeaseName"]) != "")
                whereSql += @" and AssetsLease.AssetsLeaseName like '%'+@AssetsLeaseName+'%'";
            return whereSql;
        }

        protected override string SubSelectSql(Dictionary<string, object> paras)
        {
            int pageIndex = DataConvert.ToInt32(paras["pageIndex"]);
            int rowNum = DataConvert.ToInt32(paras["rowNum"]);
            int rowSize = pageIndex * rowNum; //子查询返回行数的尺寸
            string lsql = " where 1=1";
            if (paras.ContainsKey("approveMode"))
            {
                if (DataConvert.ToString(paras["approveMode"]) == "approve")
                {
                    lsql = @",AppApprove
                     where AppApprove.tableName='AssetsLease' and AppApprove.approveState='O'
                      and AssetsLease.AssetsLeaseId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(paras["approveMode"]) == "reapply")
                {
                    lsql = @" where AssetsLease.createId=@approver and AssetsLease.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select top {0} AssetsLease.AssetsLeaseId AssetsLeaseId,
                        AssetsLease.AssetsLeaseNo AssetsLeaseNo,
                        AssetsLease.AssetsLeaseName AssetsLeaseName,
                        (select userName from AppUser where AssetsLease.createId=AppUser.userId) createId,
                        AssetsLease.createTime createTime ,
                        (select userName from AppUser where AssetsLease.updateId=AppUser.userId) updateId,
                        AssetsLease.updateTime updateTime ,
                        AssetsLease.updatePro updatePro
                from AssetsLease  {2}  {3}", DataConvert.ToString(rowSize), AppMember.AppLanguage.ToString(), lsql, WhereSql(paras));
            return subViewSql;
        }





        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from AssetsLease  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetAssetsLease(string AssetsLeaseId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsLeaseId", AssetsLeaseId);
            string sql = @"select * from AssetsLease where AssetsLeaseId=@AssetsLeaseId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        public DataTable GetAssetsDetailScrap(string AssetsLeaseId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsLeaseId", AssetsLeaseId);
            string sql = @"select * from AssetsLeaseDetail where AssetsLeaseId=@AssetsLeaseId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

        public override DataTable GetEntryGridDataTable(Dictionary<string, object> paras, string formMode, string primaryKey)
        {
            string sql = "";
            if (formMode == "new" || formMode == "new2")
            {
                sql = string.Format(@"select Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                    '' SellCompany,
                    '' SellAmount,
                    '' Remark
                from Assets where 1<>1  ");
            }
            else
            {
                if (!paras.ContainsKey("AssetsLeaseId"))
                    paras.Add("AssetsLeaseId", primaryKey);
                sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                    AssetsLeaseDetail.sellCompany sellCompany,
                    AssetsLeaseDetail.sellAmount sellAmount,
                    AssetsLeaseDetail.remark Remark
                from AssetsLeaseDetail,Assets where AssetsLeaseDetail.assetsId=Assets.assetsId and AssetsLeaseDetail.AssetsLeaseId=@AssetsLeaseId  ");
            }
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsLease where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsLease";
            DataRow dr = dt.NewRow();
            string assetsLeaseId = IdGenerator.GetMaxId(dt.TableName);
            int retApprove = InitFirstApproveTask("AssetsLease", "AssetsLeaseId", assetsLeaseId, viewTitle);
            foreach (KeyValuePair<string, object> kv in objs)
            {
                if (kv.Key != "gridData")
                    dr[kv.Key] = kv.Value;
            }
            if (retApprove != 0)
                dr["approveState"] = "O";
            dr["AssetsLeaseId"] = assetsLeaseId;
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);
            dbUpdate.Update(dt);
            List<AssetsLease> gridData = JsonHelper.JSONStringToList<AssetsLease>(DataConvert.ToString(objs["gridData"]));
            foreach (AssetsLease assetsLease in gridData)
            {
                AddDetail(assetsLease, assetsLeaseId, sysUser.UserId, viewTitle);
                if (retApprove != 0)
                {
                    UpdateAssetsState(assetsLease.AssetsId, "LI", sysUser.UserId, viewTitle);
                }
            }

            return 1;
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsLeaseId", pkValue);
            string sql = @"select * from AssetsLease where AssetsLeaseId=@AssetsLeaseId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsLease";
            string AssetsLeaseId = DataConvert.ToString(dt.Rows[0]["AssetsLeaseId"]);
            foreach (KeyValuePair<string, object> kv in objs)
            {
                if (kv.Key != "gridData")
                    dt.Rows[0][kv.Key] = kv.Value;
            }
            if (formMode == "reapply")
                dt.Rows[0]["approveState"] = "O";
            Update5Field(dt, sysUser.UserId, viewTitle);
            dbUpdate.Update(dt);
            DeleteDetail(pkValue);
            List<AssetsLease> gridData = JsonHelper.JSONStringToList<AssetsLease>(DataConvert.ToString(objs["gridData"]));
            foreach (AssetsLease AssetsLease in gridData)
            {
                AddDetail(AssetsLease, pkValue, sysUser.UserId, viewTitle);
            }
            if (formMode == "reapply")
                InitFirstApproveTask("AssetsLease", "AssetsLeaseId", AssetsLeaseId, viewTitle, formMode, sysUser.UserId);
            return 1;
        }

        protected override int DeleteData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsLeaseId", pkValue);
            string sql = @"select * from AssetsLease where AssetsLeaseId=@AssetsLeaseId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsLease";
            dt.Rows[0].Delete();
            dbUpdate.Update(dt);
            DeleteDetail(pkValue);
            DeleteApproveData("AssetsLease", pkValue, sysUser.UserId);
            return 1;
        }

        protected int AddDetail(AssetsLease AssetsLease, string AssetsLeaseId, string sysUser, string viewTitle)
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
            dt.Rows.Add(dr);
            return dbUpdate.Update(dt);
        }

        protected int DeleteDetail(string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsLeaseId", pkValue);
            string sql = @"select * from AssetsLeaseDetail where AssetsLeaseId=@AssetsLeaseId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsLeaseDetail";
            foreach (DataRow dr in dt.Rows)
                dr.Delete();
            return dbUpdate.Update(dt);
        }

        public override int DealEndApprove(string approvePkValue, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsLeaseId", approvePkValue);
            string sql = @"select * from AssetsLeaseDetail where assetsLeaseId=@assetsLeaseId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            foreach (DataRow dr in dtAssets.Rows)
            {
                UpdateAssetsState(DataConvert.ToString(dr["assetsId"]), "A", sysUser.UserId, viewTitle);
            }
            return 1;
        }

    }
}

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
    public class AssetsTransfer
    {
        public string AssetsId { get; set; }
        public string AssetsNo { get; set; }
        public string AssetsName { get; set; }
        public string AssetsTypeId { get; set; }
        public string AssetsClassId { get; set; }
        public string OriginalDepartmentId { get; set; }
        public string OriginalStoreSiteId { get; set; }
        public string OriginalKeeper { get; set; }
        public string OriginalUsePeople { get; set; }
        public string Remark { get; set; }
    }

    public class AssetsTransferRepository : ApproveMasterRepository
    {

        public AssetsTransferRepository()
        {
            DefaulteGridSortField = "assetsTransferNo";
        }


        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("assetsTransferNo") && DataConvert.ToString(paras["assetsTransferNo"]) != "")
                whereSql += @" and AssetsTransfer.assetsTransferNo like '%'+@assetsTransferNo+'%'";
            if (paras.ContainsKey("assetsTransferName") && DataConvert.ToString(paras["assetsTransferName"]) != "")
                whereSql += @" and AssetsTransfer.assetsTransferName like '%'+@assetsTransferName+'%'";
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
                     where AppApprove.tableName='AssetsTransfer' and AppApprove.approveState='O'
                      and AssetsTransfer.assetsTransferId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(paras["approveMode"]) == "reapply")
                {
                    lsql = @" where AssetsTransfer.createId=@approver and AssetsTransfer.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select top {0} AssetsTransfer.assetsTransferId assetsTransferId,
                        AssetsTransfer.assetsTransferNo assetsTransferNo,
                        AssetsTransfer.assetsTransferName assetsTransferName,
                        (select userName from AppUser where AssetsTransfer.createId=AppUser.userId) createId,
                        AssetsTransfer.createTime createTime ,
                        (select userName from AppUser where AssetsTransfer.updateId=AppUser.userId) updateId,
                        AssetsTransfer.updateTime updateTime ,
                        AssetsTransfer.updatePro updatePro
                from AssetsTransfer  {2}  {3}", DataConvert.ToString(rowSize), AppMember.AppLanguage.ToString(), lsql, WhereSql(paras));
            return subViewSql;
        }





        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from AssetsTransfer  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetAssetsTransfer(string assetsTransferId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsTransferId", assetsTransferId);
            string sql = @"select * from AssetsTransfer where assetsTransferId=@assetsTransferId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        public DataTable GetAssetsDetailScrap(string assetsTransferId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsTransferId", assetsTransferId);
            string sql = @"select * from AssetsTransferDetail where assetsTransferId=@assetsTransferId";
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
                 Assets.departmentId originalDepartmentId,
                 Assets.storeSiteId originalStoreSiteId,
                 Assets.keeper originalKeeper,
                 Assets.usePeople originalUsePeople,
                (select departmentName from AppDepartment where Assets.departmentId=AppDepartment.departmentId) originalDepartmentName, 
                 (select storeSiteName from StoreSite where Assets.storeSiteId=StoreSite.storeSiteId) originalStoreSiteName,
                 (select userName from AppUser where Assets.keeper=AppUser.userId) originalKeeperName,
                 (select userName from AppUser where Assets.usePeople=AppUser.userId) originalUsePeopleName,
                '' Remark
                from Assets where 1<>1  ");
            }
            else
            {
                if (!paras.ContainsKey("assetsTransferId"))
                    paras.Add("assetsTransferId", primaryKey);
                sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
               AssetsTransferDetail.originalDepartmentId originalDepartmentId,
                     AssetsTransferDetail.originalStoreSiteId originalStoreSiteId,
                     AssetsTransferDetail.originalKeeper originalKeeper,
                     AssetsTransferDetail.originalUsePeople originalUsePeople,
                (select departmentName from AppDepartment where AssetsTransferDetail.originalDepartmentId=AppDepartment.departmentId) originalDepartmentName, 
                 (select storeSiteName from StoreSite where AssetsTransferDetail.originalStoreSiteId=StoreSite.storeSiteId) originalStoreSiteName,
                 (select userName from AppUser where AssetsTransferDetail.originalKeeper=AppUser.userId) originalKeeperName,
                 (select userName from AppUser where AssetsTransferDetail.originalUsePeople=AppUser.userId) originalUsePeopleName,
                    AssetsTransferDetail.remark Remark
                from AssetsTransferDetail,Assets where AssetsTransferDetail.assetsId=Assets.assetsId and AssetsTransferDetail.assetsTransferId=@assetsTransferId  ");
            }
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsTransfer where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsTransfer";
            DataRow dr = dt.NewRow();
            string assetsTransferId = IdGenerator.GetMaxId(dt.TableName);
            int retApprove = InitFirstApproveTask("AssetsTransfer", "assetsTransferId", assetsTransferId, viewTitle);
            foreach (KeyValuePair<string, object> kv in objs)
            {
                if (kv.Key != "gridData")
                    dr[kv.Key] = kv.Value;
            }
            if (retApprove != 0)
                dr["approveState"] = "O";
            dr["assetsTransferId"] = assetsTransferId;
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);
            dbUpdate.Update(dt);
            List<AssetsTransfer> gridData = JsonHelper.JSONStringToList<AssetsTransfer>(DataConvert.ToString(objs["gridData"]));
            foreach (AssetsTransfer assetsTransfer in gridData)
            {
                AddDetail(assetsTransfer, objs, assetsTransferId, sysUser.UserId, viewTitle);
                if (retApprove == 0)
                {
                    UpdateAssets(assetsTransfer.AssetsId, DataConvert.ToString("departmentId"),
                        DataConvert.ToString("storeSiteId"), DataConvert.ToString("keeper"),
                        DataConvert.ToString("usePeople"), sysUser.UserId, viewTitle);
                }
                else
                {
                    UpdateAssetsState(assetsTransfer.AssetsId, "TI", sysUser.UserId, viewTitle);
                }

            }

            return 1;
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsTransferId", pkValue);
            string sql = @"select * from AssetsTransfer where assetsTransferId=@assetsTransferId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsTransfer";
            string assetsTransferId = DataConvert.ToString(dt.Rows[0]["assetsTransferId"]);
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
            List<AssetsTransfer> gridData = JsonHelper.JSONStringToList<AssetsTransfer>(DataConvert.ToString(objs["gridData"]));
            foreach (AssetsTransfer assetsTransfer in gridData)
            {
                AddDetail(assetsTransfer, objs, pkValue, sysUser.UserId, viewTitle);
            }
            if (formMode == "reapply")
                InitFirstApproveTask("AssetsTransfer", "assetsTransferId", assetsTransferId, viewTitle, formMode, sysUser.UserId);
            return 1;
        }

        protected override int DeleteData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsTransferId", pkValue);
            string sql = @"select * from AssetsTransfer where assetsTransferId=@assetsTransferId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsTransfer";
            dt.Rows[0].Delete();
            dbUpdate.Update(dt);
            DeleteDetail(pkValue);
            DeleteApproveData("AssetsTransfer", pkValue, sysUser.UserId);
            return 1;
        }

        protected int AddDetail(AssetsTransfer assetsTransfer, Dictionary<string, object> objs, string assetsTransferId, string sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsTransferDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsTransferDetail";
            DataRow dr = dt.NewRow();
            dr["assetsTransferId"] = assetsTransferId;
            dr["assetsId"] = assetsTransfer.AssetsId;
            dr["originalDepartmentId"] = assetsTransfer.OriginalDepartmentId;
            dr["originalStoreSiteId"] = assetsTransfer.OriginalStoreSiteId;
            dr["originalKeeper"] = assetsTransfer.OriginalKeeper;
            dr["originalUsePeople"] = assetsTransfer.OriginalUsePeople;
            dr["newDepartmentId"] = DataConvert.ToString(objs["departmentId"]);
            dr["newStoreSiteId"] = DataConvert.ToString(objs["storeSiteId"]);
            dr["newKeeper"] = DataConvert.ToString(objs["keeper"]);
            dr["newUsePeople"] = DataConvert.ToString(objs["usePeople"]);
            dr["remark"] = assetsTransfer.Remark;
            dt.Rows.Add(dr);
            return dbUpdate.Update(dt);
        }

        protected int DeleteDetail(string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsTransferId", pkValue);
            string sql = @"select * from AssetsTransferDetail where assetsTransferId=@assetsTransferId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsTransferDetail";
            foreach (DataRow dr in dt.Rows)
                dr.Delete();
            return dbUpdate.Update(dt);
        }

        protected int UpdateAssets(string assetsId, string departmentId, string storeSiteId, string keeper, string usePeople, string sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsId", assetsId);
            string sql = @"select * from Assets where assetsId=@assetsId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dtAssets.TableName = "Assets";
            dtAssets.Rows[0]["departmentId"] = departmentId;
            dtAssets.Rows[0]["storeSiteId"] = storeSiteId;
            dtAssets.Rows[0]["keeper"] = keeper;
            dtAssets.Rows[0]["usePeople"] = usePeople;
            dtAssets.Rows[0]["assetsState"] = "A";
            Update5Field(dtAssets, sysUser, viewTitle);
            return dbUpdate.Update(dtAssets);
        }

        public override int DealEndApprove(string approvePkValue, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsTransferId", approvePkValue);
            string sql = @"select * from AssetsTransferDetail where assetsTransferId=@assetsTransferId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            foreach (DataRow dr in dtAssets.Rows)
            {
                UpdateAssets(DataConvert.ToString(dr["assetsId"]), DataConvert.ToString(dr["newDepartmentId"]),
                    DataConvert.ToString(dr["newStoreSiteId"]), DataConvert.ToString(dr["newKeeper"]),
                    DataConvert.ToString(dr["newUsePeople"]), sysUser.UserId, viewTitle);
            }
            return 1;
        }


    }
}

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
    public class AssetsBorrow
    {
        public string AssetsId { get; set; }
        public string AssetsNo { get; set; }
        public string AssetsName { get; set; }
        public string AssetsTypeId { get; set; }
        public string AssetsClassId { get; set; }
        public string DepartmentId { get; set; }
        public string PlanReturnDate { get; set; }
        public string BorrowPeople { get; set; }
        public string BorrowDepartmentId { get; set; }
        public string BorrowDate { get; set; }
        public string Remark { get; set; }
    }

    public class AssetsBorrowRepository : ApproveMasterRepository
    {

        public AssetsBorrowRepository()
        {
            DefaulteGridSortField = "assetsBorrowNo";
        }


        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("assetsBorrowNo") && DataConvert.ToString(paras["assetsBorrowNo"]) != "")
                whereSql += @" and AssetsBorrow.assetsBorrowNo like '%'+@assetsBorrowNo+'%'";
            if (paras.ContainsKey("assetsBorrowName") && DataConvert.ToString(paras["assetsBorrowName"]) != "")
                whereSql += @" and AssetsBorrow.assetsBorrowName like '%'+@assetsBorrowName+'%'";
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
                     where AppApprove.tableName='AssetsBorrow' and AppApprove.approveState='O'
                      and AssetsBorrow.assetsBorrowId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(paras["approveMode"]) == "reapply")
                {
                    lsql = @" where AssetsBorrow.createId=@approver and AssetsBorrow.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select top {0} AssetsBorrow.assetsBorrowId assetsBorrowId,
                        AssetsBorrow.assetsBorrowNo assetsBorrowNo,
                        AssetsBorrow.assetsBorrowName assetsBorrowName,
                        (select userName from AppUser where AssetsBorrow.createId=AppUser.userId) createId,
                        AssetsBorrow.createTime createTime ,
                        (select userName from AppUser where AssetsBorrow.updateId=AppUser.userId) updateId,
                        AssetsBorrow.updateTime updateTime ,
                        AssetsBorrow.updatePro updatePro
                from AssetsBorrow  {2}  {3}", DataConvert.ToString(rowSize), AppMember.AppLanguage.ToString(), lsql, WhereSql(paras));
            return subViewSql;
        }





        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from AssetsBorrow  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetAssetsBorrow(string assetsBorrowId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsBorrowId", assetsBorrowId);
            string sql = @"select * from AssetsBorrow where assetsBorrowId=@assetsBorrowId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        public DataTable GetAssetsDetailScrap(string assetsBorrowId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsBorrowId", assetsBorrowId);
            string sql = @"select * from AssetsBorrowDetail where assetsBorrowId=@assetsBorrowId";
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
                 (select departmentName from AppDepartment where Assets.departmentId=AppDepartment.departmentId) departmentId,
                  '' PlanReturnDate,
                    '' Remark
                from Assets where 1<>1  ");
            }
            else
            {
                if (!paras.ContainsKey("assetsBorrowId"))
                    paras.Add("assetsBorrowId", primaryKey);
                sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                 (select departmentName from AppDepartment where Assets.departmentId=AppDepartment.departmentId) departmentId,
                  AssetsBorrowDetail.planReturnDate PlanReturnDate,
                    AssetsBorrowDetail.remark Remark
                from AssetsBorrowDetail,Assets where AssetsBorrowDetail.assetsId=Assets.assetsId and AssetsBorrowDetail.assetsBorrowId=@assetsBorrowId  ");
            }
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsBorrow where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsBorrow";
            DataRow dr = dt.NewRow();
            string assetsBorrowId = IdGenerator.GetMaxId(dt.TableName);
            int retApprove = InitFirstApproveTask("AssetsBorrow", "assetsBorrowId", assetsBorrowId, viewTitle);
            foreach (KeyValuePair<string, object> kv in objs)
            {
                if (kv.Key != "gridData")
                    dr[kv.Key] = kv.Value;
            }
            if (retApprove != 0)
                dr["approveState"] = "O";
            dr["assetsBorrowId"] = assetsBorrowId;
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);
            dbUpdate.Update(dt);

            //List<AssetsBorrow> gridData = (List<AssetsBorrow>)objs["gridData"]; 
            List<AssetsBorrow> gridData = JsonHelper.JSONStringToList<AssetsBorrow>(DataConvert.ToString(objs["gridData"]));
            foreach (AssetsBorrow assetsBorrow in gridData)
            {
                assetsBorrow.BorrowPeople = DataConvert.ToString(objs["borrowPeople"]);
                assetsBorrow.BorrowDepartmentId = DataConvert.ToString(objs["departmentId"]);
                assetsBorrow.BorrowDate = DataConvert.ToString(objs["borrowDate"]);
                AddDetail(assetsBorrow, assetsBorrowId, sysUser.UserId, viewTitle);
                if (retApprove != 0)
                {
                    UpdateAssetsState(assetsBorrow.AssetsId, "BI", sysUser.UserId, viewTitle);
                }
            }

            return 1;
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsBorrowId", pkValue);
            string sql = @"select * from AssetsBorrow where assetsBorrowId=@assetsBorrowId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsBorrow";
            string assetsBorrowId = DataConvert.ToString(dt.Rows[0]["assetsBorrowId"]);
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
            List<AssetsBorrow> gridData = JsonHelper.JSONStringToList<AssetsBorrow>(DataConvert.ToString(objs["gridData"]));
            foreach (AssetsBorrow assetsBorrow in gridData)
            {
                AddDetail(assetsBorrow, pkValue, sysUser.UserId, viewTitle);
            }
            if (formMode == "reapply")
                InitFirstApproveTask("AssetsBorrow", "assetsBorrowId", assetsBorrowId, viewTitle, formMode, sysUser.UserId);
            return 1;
        }

        protected override int DeleteData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsBorrowId", pkValue);
            string sql = @"select * from AssetsBorrow where assetsBorrowId=@assetsBorrowId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsBorrow";
            dt.Rows[0].Delete();
            dbUpdate.Update(dt);
            DeleteDetail(pkValue);
            DeleteApproveData("AssetsBorrow", pkValue, sysUser.UserId);
            return 1;
        }

        protected int AddDetail(AssetsBorrow assetsBorrow, string assetsBorrowId, string sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsBorrowDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsBorrowDetail";
            DataRow dr = dt.NewRow();
            dr["assetsBorrowId"] = assetsBorrowId;
            dr["assetsId"] = assetsBorrow.AssetsId;
            dr["planReturnDate"] = assetsBorrow.PlanReturnDate;
            dr["borrowPeople"] = assetsBorrow.BorrowPeople;
            dr["borrowDepartmentId"] = assetsBorrow.BorrowDepartmentId;
            dr["borrowDate"] = assetsBorrow.BorrowDate;
            dr["remark"] = assetsBorrow.Remark;
            dt.Rows.Add(dr);
            return dbUpdate.Update(dt);
        }

        protected int DeleteDetail(string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsBorrowId", pkValue);
            string sql = @"select * from AssetsBorrowDetail where assetsBorrowId=@assetsBorrowId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsBorrowDetail";
            foreach (DataRow dr in dt.Rows)
                dr.Delete();
            return dbUpdate.Update(dt);
        }
        public override int DealEndApprove(string approvePkValue, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsBorrowId", approvePkValue);
            string sql = @"select * from AssetsBorrowDetail where assetsBorrowId=@assetsBorrowId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            foreach (DataRow dr in dtAssets.Rows)
            {
                UpdateAssetsState(DataConvert.ToString(dr["assetsId"]), "A", sysUser.UserId, viewTitle);
            }
            return 1;
        }

    }
}

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
    public class AssetsMerge
    {
        public string AssetsId { get; set; }
        public string AssetsNo { get; set; }
        public string AssetsName { get; set; }
        public string AssetsTypeId { get; set; }
        public string AssetsClassId { get; set; }
        public string AssetsValue { get; set; }
        public string Remark { get; set; }
        public string OriginalAssetsId { get; set; }
    }

    public class AssetsMergeRepository : ApproveMasterRepository
    {

        public AssetsMergeRepository()
        {
            DefaulteGridSortField = "AssetsMergeNo";
        }


        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("AssetsMergeNo") && DataConvert.ToString(paras["AssetsMergeNo"]) != "")
                whereSql += @" and AssetsMerge.AssetsMergeNo like '%'+@AssetsMergeNo+'%'";
            if (paras.ContainsKey("AssetsMergeName") && DataConvert.ToString(paras["AssetsMergeName"]) != "")
                whereSql += @" and AssetsMerge.AssetsMergeName like '%'+@AssetsMergeName+'%'";
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
                     where AppApprove.tableName='AssetsMerge' and AppApprove.approveState='O'
                      and AssetsMerge.AssetsMergeId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(paras["approveMode"]) == "reapply")
                {
                    lsql = @" where AssetsMerge.createId=@approver and AssetsMerge.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select top {0} AssetsMerge.AssetsMergeId AssetsMergeId,
                        AssetsMerge.AssetsMergeNo AssetsMergeNo,
                        AssetsMerge.AssetsMergeName AssetsMergeName,
                        (select userName from AppUser where AssetsMerge.createId=AppUser.userId) createId,
                        AssetsMerge.createTime createTime ,
                        (select userName from AppUser where AssetsMerge.updateId=AppUser.userId) updateId,
                        AssetsMerge.updateTime updateTime ,
                        AssetsMerge.updatePro updatePro
                from AssetsMerge  {2}  {3}", DataConvert.ToString(rowSize), AppMember.AppLanguage.ToString(), lsql, WhereSql(paras));
            return subViewSql;
        }





        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from AssetsMerge  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetAssetsMerge(string AssetsMergeId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsMergeId", AssetsMergeId);
            string sql = @"select * from AssetsMerge where AssetsMergeId=@AssetsMergeId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        public DataTable GetAssetsDetailScrap(string AssetsMergeId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsMergeId", AssetsMergeId);
            string sql = @"select * from AssetsMergeDetail where AssetsMergeId=@AssetsMergeId";
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
                    '' assetsValue,
                    '' Remark,
                    Assets.assetsId OriginalAssetsId
                from Assets where 1<>1  ");
            }
            else
            {
                if (!paras.ContainsKey("AssetsMergeId"))
                    paras.Add("AssetsMergeId", primaryKey);
                sql = string.Format(@"select top 1  AssetsMergeDetail.newAssetsId assetsId,
                        AssetsMergeDetail.assetsNo assetsNo,
                        AssetsMergeDetail.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                    AssetsMergeDetail.assetsValue assetsValue,
                    AssetsMergeDetail.remark Remark,
                    AssetsMergeDetail.originalAssetsId originalAssetsId
                from AssetsMergeDetail left join Assets on AssetsMergeDetail.originalAssetsId=Assets.assetsId where  AssetsMergeDetail.AssetsMergeId=@AssetsMergeId  ");
            }
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

        public DataTable GetUpEntryGridDataTable(Dictionary<string, object> paras, string formMode, string primaryKey)
        {
            string sql = "";
            if (formMode == "new" || formMode == "new2")
            {
                sql = string.Format(@"select Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                    '' assetsValue,
                    '' Remark,
                   Assets.assetsId OriginalAssetsId
                from Assets where 1<>1  ");
            }
            else
            {
                if (!paras.ContainsKey("AssetsMergeId"))
                    paras.Add("AssetsMergeId", primaryKey);
                sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                    Assets.assetsValue assetsValue,
                    Assets.remark Remark,
                    AssetsMergeDetail.originalAssetsId originalAssetsId
                from AssetsMergeDetail,Assets where AssetsMergeDetail.originalAssetsId=Assets.assetsId and AssetsMergeDetail.AssetsMergeId=@AssetsMergeId  ");
            }
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsMerge where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsMerge";
            DataRow dr = dt.NewRow();
            string assetsMergeId = IdGenerator.GetMaxId(dt.TableName);
            int retApprove = InitFirstApproveTask("AssetsMerge", "AssetsMergeId", assetsMergeId, viewTitle);
            foreach (KeyValuePair<string, object> kv in objs)
            {
                if (kv.Key != "gridData" && kv.Key != "upGridData")
                    dr[kv.Key] = kv.Value;
            }
            if (retApprove != 0)
                dr["approveState"] = "O";
            dr["AssetsMergeId"] = assetsMergeId;
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);
            dbUpdate.Update(dt);
            List<AssetsMerge> gridData = JsonHelper.JSONStringToList<AssetsMerge>(DataConvert.ToString(objs["gridData"]));
            AssetsMerge assetsMerge = gridData[0];
            assetsMerge.AssetsId = IdGenerator.GetMaxId("Assets");
            List<AssetsMerge> upGridData = JsonHelper.JSONStringToList<AssetsMerge>(DataConvert.ToString(objs["upGridData"]));

            foreach (AssetsMerge originalAssetsMerge in upGridData)
            {
                AddDetail(assetsMerge, originalAssetsMerge, assetsMergeId, sysUser.UserId, viewTitle);
                if (retApprove == 0)
                {
                    UpdateAssetsState(originalAssetsMerge.AssetsId, "M", sysUser.UserId, viewTitle);
                }
                else
                {
                    UpdateAssetsState(originalAssetsMerge.AssetsId, "MI", sysUser.UserId, viewTitle);
                }
            }
            if (retApprove == 0)
                AddNewAssets(assetsMerge, sysUser.UserId, viewTitle);
            return 1;
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsMergeId", pkValue);
            string sql = @"select * from AssetsMerge where AssetsMergeId=@AssetsMergeId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsMerge";
            string AssetsMergeId = DataConvert.ToString(dt.Rows[0]["AssetsMergeId"]);
            foreach (KeyValuePair<string, object> kv in objs)
            {
                if (kv.Key != "gridData" && kv.Key != "upGridData")
                    dt.Rows[0][kv.Key] = kv.Value;
            }
            if (formMode == "reapply")
                dt.Rows[0]["approveState"] = "O";
            Update5Field(dt, sysUser.UserId, viewTitle);
            dbUpdate.Update(dt);
            DeleteDetail(pkValue);
            List<AssetsMerge> gridData = JsonHelper.JSONStringToList<AssetsMerge>(DataConvert.ToString(objs["gridData"]));
            AssetsMerge AssetsMerge = gridData[0];
            List<AssetsMerge> upGridData = JsonHelper.JSONStringToList<AssetsMerge>(DataConvert.ToString(objs["upGridData"]));
            foreach (AssetsMerge OriginalAssetsMerge in upGridData)
            {
                AddDetail(AssetsMerge, OriginalAssetsMerge, pkValue, sysUser.UserId, viewTitle);
            }
            if (formMode == "reapply")
                InitFirstApproveTask("AssetsMerge", "AssetsMergeId", AssetsMergeId, viewTitle, formMode, sysUser.UserId);
            return 1;
        }

        protected override int DeleteData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsMergeId", pkValue);
            string sql = @"select * from AssetsMerge where AssetsMergeId=@AssetsMergeId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsMerge";
            dt.Rows[0].Delete();
            dbUpdate.Update(dt);
            DeleteDetail(pkValue);
            DeleteApproveData("AssetsMerge", pkValue, sysUser.UserId);
            return 1;
        }

        protected int AddDetail(AssetsMerge AssetsMerge, AssetsMerge OriginalAssetsMerge, string AssetsMergeId, string sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsMergeDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsMergeDetail";
            DataRow dr = dt.NewRow();
            dr["AssetsMergeId"] = AssetsMergeId;
            dr["originalAssetsId"] = OriginalAssetsMerge.AssetsId;
            dr["newAssetsId"] = AssetsMerge.AssetsId;
            dr["assetsNo"] = AssetsMerge.AssetsNo;
            dr["assetsName"] = AssetsMerge.AssetsName;
            dr["assetsValue"] = AssetsMerge.AssetsValue;
            dr["remark"] = AssetsMerge.Remark;
            dt.Rows.Add(dr);
            return dbUpdate.Update(dt);
        }

        protected int DeleteDetail(string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsMergeId", pkValue);
            string sql = @"select * from AssetsMergeDetail where AssetsMergeId=@AssetsMergeId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsMergeDetail";
            foreach (DataRow dr in dt.Rows)
                dr.Delete();
            return dbUpdate.Update(dt);
        }

        //protected int UpdateOriginalAssets(AssetsMerge assetsMerge, string sysUser, string viewTitle)
        //{
        //    Dictionary<string, object> paras = new Dictionary<string, object>();
        //    paras.Add("assetsId", assetsMerge.OriginalAssetsId);
        //    string sql = @"select * from Assets where assetsId=@assetsId";
        //    DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
        //    dtAssets.TableName = "Assets";
        //    dtAssets.Rows[0]["assetsState"] = "M";
        //    Update5Field(dtAssets, sysUser, viewTitle);
        //    return dbUpdate.Update(dtAssets);
        //}

        protected int AddNewAssets(AssetsMerge assetsMerge, string sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsId", assetsMerge.OriginalAssetsId);
            string sql = @"select * from Assets where assetsId=@assetsId ";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dtAssets.TableName = "Assets";
            DataRow drNew = dtAssets.NewRow();
            for (int i = 0; i < dtAssets.Columns.Count; i++)
            {
                drNew[i] = dtAssets.Rows[0][i];
            }
            drNew["assetsId"] = assetsMerge.AssetsId;
            drNew["assetsNo"] = assetsMerge.AssetsNo;
            drNew["assetsValue"] = assetsMerge.AssetsValue;
            drNew["remark"] = assetsMerge.Remark;
            Create5Field(drNew, sysUser, viewTitle);
            dtAssets.Rows.Add(drNew);
            return dbUpdate.Update(dtAssets);
        }

        public override int DealEndApprove(string approvePkValue, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsMergeId", approvePkValue);
            string sql = @"select * from AssetsMergeDetail where assetsMergeId=@assetsMergeId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            int rowIndex = 0;
            foreach (DataRow dr in dtAssets.Rows)
            {
                AssetsMerge assetsMerge = new AssetsMerge();
                assetsMerge.AssetsId = DataConvert.ToString(dr["newAssetsId"]);
                assetsMerge.AssetsNo = DataConvert.ToString(dr["assetsNo"]);
                assetsMerge.AssetsValue = DataConvert.ToString(dr["assetsValue"]);
                assetsMerge.OriginalAssetsId = DataConvert.ToString(dr["originalAssetsId"]);
                if (rowIndex == 0)
                    AddNewAssets(assetsMerge, sysUser.UserId, viewTitle);
                UpdateAssetsState(assetsMerge.OriginalAssetsId, "M", sysUser.UserId, viewTitle);
                rowIndex++;
            }
            return 1;
        }



    }
}

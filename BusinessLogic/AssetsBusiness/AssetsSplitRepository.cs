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
    public class AssetsSplit
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

    public class AssetsSplitRepository : ApproveMasterRepository
    {

        public AssetsSplitRepository()
        {
            DefaulteGridSortField = "AssetsSplitNo";
        }


        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("AssetsSplitNo") && DataConvert.ToString(paras["AssetsSplitNo"]) != "")
                whereSql += @" and AssetsSplit.AssetsSplitNo like '%'+@AssetsSplitNo+'%'";
            if (paras.ContainsKey("AssetsSplitName") && DataConvert.ToString(paras["AssetsSplitName"]) != "")
                whereSql += @" and AssetsSplit.AssetsSplitName like '%'+@AssetsSplitName+'%'";
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
                     where AppApprove.tableName='AssetsSplit' and AppApprove.approveState='O'
                      and AssetsSplit.AssetsSplitId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(paras["approveMode"]) == "reapply")
                {
                    lsql = @" where AssetsSplit.createId=@approver and AssetsSplit.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select top {0} AssetsSplit.AssetsSplitId AssetsSplitId,
                        AssetsSplit.AssetsSplitNo AssetsSplitNo,
                        AssetsSplit.AssetsSplitName AssetsSplitName,
                        (select userName from AppUser where AssetsSplit.createId=AppUser.userId) createId,
                        AssetsSplit.createTime createTime ,
                        (select userName from AppUser where AssetsSplit.updateId=AppUser.userId) updateId,
                        AssetsSplit.updateTime updateTime ,
                        AssetsSplit.updatePro updatePro
                from AssetsSplit  {2}  {3}", DataConvert.ToString(rowSize), AppMember.AppLanguage.ToString(), lsql, WhereSql(paras));
            return subViewSql;
        }





        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from AssetsSplit  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetAssetsSplit(string AssetsSplitId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsSplitId", AssetsSplitId);
            string sql = @"select * from AssetsSplit where AssetsSplitId=@AssetsSplitId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        public DataTable GetAssetsDetailScrap(string AssetsSplitId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsSplitId", AssetsSplitId);
            string sql = @"select * from AssetsSplitDetail where AssetsSplitId=@AssetsSplitId";
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
                        (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                    '' assetsValue,
                    '' Remark,
                     Assets.assetsId OriginalAssetsId
                from Assets where 1<>1  ");
            }
            else
            {
                if (!paras.ContainsKey("AssetsSplitId"))
                    paras.Add("AssetsSplitId", primaryKey);
                sql = string.Format(@"select  AssetsSplitDetail.newAssetsId assetsId,
                        AssetsSplitDetail.assetsNo assetsNo,
                        AssetsSplitDetail.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                    AssetsSplitDetail.assetsValue assetsValue,
                    AssetsSplitDetail.remark Remark,
                    AssetsSplitDetail.originalAssetsId originalAssetsId
                from AssetsSplitDetail left join Assets on AssetsSplitDetail.originalAssetsId=Assets.assetsId where  AssetsSplitDetail.AssetsSplitId=@AssetsSplitId  ");
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
                        (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                    '' assetsValue,
                    '' Remark,
                    Assets.assetsId OriginalAssetsId
                from Assets where 1<>1  ");
            }
            else
            {
                if (!paras.ContainsKey("AssetsSplitId"))
                    paras.Add("AssetsSplitId", primaryKey);
                sql = string.Format(@"select top 1 Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                    AssetsSplitDetail.assetsValue assetsValue,
                    AssetsSplitDetail.remark Remark,
                    Assets.assetsId OriginalAssetsId
                from AssetsSplitDetail,Assets where AssetsSplitDetail.originalAssetsId=Assets.assetsId and AssetsSplitDetail.AssetsSplitId=@AssetsSplitId  ");
            }
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsSplit where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsSplit";
            DataRow dr = dt.NewRow();
            string assetsSplitId = IdGenerator.GetMaxId(dt.TableName);
            int retApprove = InitFirstApproveTask("AssetsSplit", "AssetsSplitId", assetsSplitId, viewTitle);
            foreach (KeyValuePair<string, object> kv in objs)
            {
                if (kv.Key != "gridData")
                    dr[kv.Key] = kv.Value;
            }
            if (retApprove != 0)
                dr["approveState"] = "O";
            dr["AssetsSplitId"] = assetsSplitId;
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);
            dbUpdate.Update(dt);
            List<AssetsSplit> gridData = JsonHelper.JSONStringToList<AssetsSplit>(DataConvert.ToString(objs["gridData"]));
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsId", gridData[0].OriginalAssetsId);
            sql = @"select * from Assets where assetsId=@assetsId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            int rowIndex = 0;
            foreach (AssetsSplit assetsSplit in gridData)
            {
                assetsSplit.AssetsId = IdGenerator.GetMaxId("Assets");
                AddDetail(assetsSplit, assetsSplitId, sysUser.UserId, viewTitle);
                if (retApprove == 0)
                {
                    if (rowIndex == 0)
                        UpdateAssetsState(assetsSplit.OriginalAssetsId, "S", sysUser.UserId, viewTitle);
                    AddNewAssets(assetsSplit, dtAssets, sysUser.UserId, viewTitle);
                }
                else
                {
                    if (rowIndex == 0)
                        UpdateAssetsState(assetsSplit.OriginalAssetsId, "SI", sysUser.UserId, viewTitle);
                }
                rowIndex += 1;
            }

            return 1;
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsSplitId", pkValue);
            string sql = @"select * from AssetsSplit where AssetsSplitId=@AssetsSplitId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsSplit";
            string AssetsSplitId = DataConvert.ToString(dt.Rows[0]["AssetsSplitId"]);
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
            List<AssetsSplit> gridData = JsonHelper.JSONStringToList<AssetsSplit>(DataConvert.ToString(objs["gridData"]));
            foreach (AssetsSplit AssetsSplit in gridData)
            {
                AddDetail(AssetsSplit, pkValue, sysUser.UserId, viewTitle);
            }
            if (formMode == "reapply")
                InitFirstApproveTask("AssetsSplit", "AssetsSplitId", AssetsSplitId, viewTitle, formMode, sysUser.UserId);
            return 1;
        }

        protected override int DeleteData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsSplitId", pkValue);
            string sql = @"select * from AssetsSplit where AssetsSplitId=@AssetsSplitId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsSplit";
            dt.Rows[0].Delete();
            dbUpdate.Update(dt);
            DeleteDetail(pkValue);
            DeleteApproveData("AssetsSplit", pkValue, sysUser.UserId);
            return 1;
        }

        protected int AddDetail(AssetsSplit AssetsSplit, string AssetsSplitId, string sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsSplitDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsSplitDetail";
            DataRow dr = dt.NewRow();
            dr["AssetsSplitId"] = AssetsSplitId;
            dr["newAssetsId"] = AssetsSplit.AssetsId;
            dr["assetsNo"] = AssetsSplit.AssetsNo;
            dr["assetsName"] = AssetsSplit.AssetsName;
            dr["assetsValue"] = AssetsSplit.AssetsValue;
            dr["remark"] = AssetsSplit.Remark;
            dr["originalAssetsId"] = AssetsSplit.OriginalAssetsId;
            dt.Rows.Add(dr);
            return dbUpdate.Update(dt);
        }

        protected int DeleteDetail(string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsSplitId", pkValue);
            string sql = @"select * from AssetsSplitDetail where AssetsSplitId=@AssetsSplitId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsSplitDetail";
            foreach (DataRow dr in dt.Rows)
                dr.Delete();
            return dbUpdate.Update(dt);
        }

        //protected int UpdateOriginalAssets(AssetsSplit AssetsSplit, DataTable dtAssets, string sysUser, string viewTitle)
        //{
        //    DataTable dtNew = dtAssets.Clone();
        //    foreach (DataRow drNew in dtAssets.Rows)
        //    {
        //        dtNew.ImportRow(drNew);
        //    }
        //    dtNew.TableName = "Assets";
        //    dtNew.Rows[0]["assetsState"] = "S";
        //    Update5Field(dtNew, sysUser, viewTitle);
        //    return dbUpdate.Update(dtNew);
        //}

        protected int AddNewAssets(AssetsSplit AssetsSplit, DataTable dtAssets, string sysUser, string viewTitle)
        {
            DataTable dtNew = dtAssets.Clone();
            DataRow drNew = dtNew.NewRow();
            for (int i = 0; i < dtAssets.Columns.Count; i++)
            {
                drNew[i] = dtAssets.Rows[0][i];
            }
            drNew["assetsId"] = AssetsSplit.AssetsId;
            drNew["assetsNo"] = AssetsSplit.AssetsNo;
            drNew["assetsValue"] = AssetsSplit.AssetsValue;
            drNew["remark"] = AssetsSplit.Remark;
            Create5Field(drNew, sysUser, viewTitle);
            dtNew.TableName = "Assets";
            dtNew.Rows.Add(drNew);
            return dbUpdate.Update(dtNew);
        }

        public override int DealEndApprove(string approvePkValue, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsSplitId", approvePkValue);
            string sql = @"select * from AssetsSplitDetail where assetsSplitId=@assetsSplitId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            int rowIndex = 0;
            foreach (DataRow dr in dt.Rows)
            {
                AssetsSplit assetsSplit = new AssetsSplit();
                assetsSplit.AssetsId = DataConvert.ToString(dr["newAssetsId"]);
                assetsSplit.AssetsNo = DataConvert.ToString(dr["assetsNo"]);
                assetsSplit.AssetsValue = DataConvert.ToString(dr["assetsValue"]);
                assetsSplit.OriginalAssetsId = DataConvert.ToString(dr["originalAssetsId"]);
                paras.Clear();
                paras.Add("assetsId", assetsSplit.OriginalAssetsId);
                sql = @"select * from Assets where assetsId=@assetsId";
                DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
                if (rowIndex == 0)
                    UpdateAssetsState(assetsSplit.OriginalAssetsId, "S", sysUser.UserId, viewTitle);
                AddNewAssets(assetsSplit, dtAssets, sysUser.UserId, viewTitle);
                rowIndex++;
            }
            return 1;
        }


    }
}

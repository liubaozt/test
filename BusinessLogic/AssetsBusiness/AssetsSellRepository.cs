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
    public class AssetsSell
    {
        public string AssetsId { get; set; }
        public string AssetsNo { get; set; }
        public string AssetsName { get; set; }
        public string AssetsTypeId { get; set; }
        public string AssetsClassId { get; set; }
        public string SellCompany { get; set; }
        public string SellAmount { get; set; }
        public string Remark { get; set; }
    }

    public class AssetsSellRepository : ApproveMasterRepository
    {

        public AssetsSellRepository()
        {
            DefaulteGridSortField = "AssetsSellNo";
        }


        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("AssetsSellNo") && DataConvert.ToString(paras["AssetsSellNo"]) != "")
                whereSql += @" and AssetsSell.AssetsSellNo like '%'+@AssetsSellNo+'%'";
            if (paras.ContainsKey("AssetsSellName") && DataConvert.ToString(paras["AssetsSellName"]) != "")
                whereSql += @" and AssetsSell.AssetsSellName like '%'+@AssetsSellName+'%'";
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
                     where AppApprove.tableName='AssetsSell' and AppApprove.approveState='O'
                      and AssetsSell.AssetsSellId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(paras["approveMode"]) == "reapply")
                {
                    lsql = @" where AssetsSell.createId=@approver and AssetsSell.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select top {0} AssetsSell.AssetsSellId AssetsSellId,
                        AssetsSell.AssetsSellNo AssetsSellNo,
                        AssetsSell.AssetsSellName AssetsSellName,
                        (select userName from AppUser where AssetsSell.createId=AppUser.userId) createId,
                        AssetsSell.createTime createTime ,
                        (select userName from AppUser where AssetsSell.updateId=AppUser.userId) updateId,
                        AssetsSell.updateTime updateTime ,
                        AssetsSell.updatePro updatePro
                from AssetsSell  {2}  {3}", DataConvert.ToString(rowSize), AppMember.AppLanguage.ToString(), lsql, WhereSql(paras));
            return subViewSql;
        }





        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from AssetsSell  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetAssetsSell(string AssetsSellId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsSellId", AssetsSellId);
            string sql = @"select * from AssetsSell where AssetsSellId=@AssetsSellId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        public DataTable GetAssetsDetailScrap(string AssetsSellId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsSellId", AssetsSellId);
            string sql = @"select * from AssetsSellDetail where AssetsSellId=@AssetsSellId";
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
                if (!paras.ContainsKey("AssetsSellId"))
                    paras.Add("AssetsSellId", primaryKey);
                sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                    AssetsSellDetail.sellCompany sellCompany,
                    AssetsSellDetail.sellAmount sellAmount,
                    AssetsSellDetail.remark Remark
                from AssetsSellDetail,Assets where AssetsSellDetail.assetsId=Assets.assetsId and AssetsSellDetail.AssetsSellId=@AssetsSellId  ");
            }
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsSell where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsSell";
            DataRow dr = dt.NewRow();
            string assetsSellId = IdGenerator.GetMaxId(dt.TableName);
            int retApprove = InitFirstApproveTask("AssetsSell", "AssetsSellId", assetsSellId, viewTitle);
            foreach (KeyValuePair<string, object> kv in objs)
            {
                if (kv.Key != "gridData")
                    dr[kv.Key] = kv.Value;
            }
            if (retApprove != 0)
                dr["approveState"] = "O";
            dr["AssetsSellId"] = assetsSellId;
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);
            dbUpdate.Update(dt);

            List<AssetsSell> gridData = JsonHelper.JSONStringToList<AssetsSell>(DataConvert.ToString(objs["gridData"]));
            foreach (AssetsSell assetsSell in gridData)
            {
                AddDetail(assetsSell, assetsSellId, sysUser.UserId, viewTitle);
                if (retApprove != 0)
                {
                    UpdateAssetsState(assetsSell.AssetsId, "XI", sysUser.UserId, viewTitle);
                }
            }

            return 1;
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsSellId", pkValue);
            string sql = @"select * from AssetsSell where AssetsSellId=@AssetsSellId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsSell";
            string AssetsSellId = DataConvert.ToString(dt.Rows[0]["AssetsSellId"]);
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
            List<AssetsSell> gridData = JsonHelper.JSONStringToList<AssetsSell>(DataConvert.ToString(objs["gridData"]));
            foreach (AssetsSell AssetsSell in gridData)
            {
                AddDetail(AssetsSell, pkValue, sysUser.UserId, viewTitle);
            }
            if (formMode == "reapply")
                InitFirstApproveTask("AssetsSell", "AssetsSellId", AssetsSellId, viewTitle, formMode, sysUser.UserId);
            return 1;
        }

        protected override int DeleteData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsSellId", pkValue);
            string sql = @"select * from AssetsSell where AssetsSellId=@AssetsSellId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsSell";
            dt.Rows[0].Delete();
            dbUpdate.Update(dt);
            DeleteDetail(pkValue);
            DeleteApproveData("AssetsSell", pkValue, sysUser.UserId);
            return 1;
        }

        protected int AddDetail(AssetsSell AssetsSell, string AssetsSellId, string sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsSellDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsSellDetail";
            DataRow dr = dt.NewRow();
            dr["AssetsSellId"] = AssetsSellId;
            dr["assetsId"] = AssetsSell.AssetsId;
            dr["remark"] = AssetsSell.Remark;
            dr["sellCompany"] = AssetsSell.SellCompany;
            dr["sellAmount"] = AssetsSell.SellAmount;
            dt.Rows.Add(dr);
            return dbUpdate.Update(dt);
        }

        protected int DeleteDetail(string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsSellId", pkValue);
            string sql = @"select * from AssetsSellDetail where AssetsSellId=@AssetsSellId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsSellDetail";
            foreach (DataRow dr in dt.Rows)
                dr.Delete();
            return dbUpdate.Update(dt);
        }

        public override int DealEndApprove(string approvePkValue, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsSellId", approvePkValue);
            string sql = @"select * from AssetsSellDetail where assetsSellId=@assetsSellId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            foreach (DataRow dr in dtAssets.Rows)
            {
                UpdateAssetsState(DataConvert.ToString(dr["assetsId"]), "A", sysUser.UserId, viewTitle);
            }
            return 1;
        }


    }
}

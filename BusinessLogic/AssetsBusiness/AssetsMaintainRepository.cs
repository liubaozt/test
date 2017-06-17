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
    public class AssetsMaintain
    {
        public string AssetsId { get; set; }
        public string AssetsNo { get; set; }
        public string AssetsName { get; set; }
        public string AssetsTypeId { get; set; }
        public string AssetsClassId { get; set; }
        public string MaintainCompany { get; set; }
        public string MaintainAmount { get; set; }
        public string Remark { get; set; }
    }

    public class AssetsMaintainRepository : ApproveMasterRepository
    {

        public AssetsMaintainRepository()
        {
            DefaulteGridSortField = "AssetsMaintainNo";
        }


        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("AssetsMaintainNo") && DataConvert.ToString(paras["AssetsMaintainNo"]) != "")
                whereSql += @" and AssetsMaintain.AssetsMaintainNo like '%'+@AssetsMaintainNo+'%'";
            if (paras.ContainsKey("AssetsMaintainName") && DataConvert.ToString(paras["AssetsMaintainName"]) != "")
                whereSql += @" and AssetsMaintain.AssetsMaintainName like '%'+@AssetsMaintainName+'%'";
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
                     where AppApprove.tableName='AssetsMaintain' and AppApprove.approveState='O'
                      and AssetsMaintain.AssetsMaintainId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(paras["approveMode"]) == "reapply")
                {
                    lsql = @" where AssetsMaintain.createId=@approver and AssetsMaintain.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select top {0} AssetsMaintain.AssetsMaintainId AssetsMaintainId,
                        AssetsMaintain.AssetsMaintainNo AssetsMaintainNo,
                        AssetsMaintain.AssetsMaintainName AssetsMaintainName,
                        (select userName from AppUser where AssetsMaintain.createId=AppUser.userId) createId,
                        AssetsMaintain.createTime createTime ,
                        (select userName from AppUser where AssetsMaintain.updateId=AppUser.userId) updateId,
                        AssetsMaintain.updateTime updateTime ,
                        AssetsMaintain.updatePro updatePro
                from AssetsMaintain  {2}  {3}", DataConvert.ToString(rowSize), AppMember.AppLanguage.ToString(), lsql, WhereSql(paras));
            return subViewSql;
        }





        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from AssetsMaintain  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetAssetsMaintain(string AssetsMaintainId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsMaintainId", AssetsMaintainId);
            string sql = @"select * from AssetsMaintain where AssetsMaintainId=@AssetsMaintainId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        public DataTable GetAssetsDetailScrap(string AssetsMaintainId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsMaintainId", AssetsMaintainId);
            string sql = @"select * from AssetsMaintainDetail where AssetsMaintainId=@AssetsMaintainId";
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
                    '' MaintainCompany,
                    '' MaintainAmount,
                    '' Remark
                from Assets where 1<>1  ");
            }
            else
            {
                if (!paras.ContainsKey("AssetsMaintainId"))
                    paras.Add("AssetsMaintainId", primaryKey);
                sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                    AssetsMaintainDetail.maintainCompany MaintainCompany,
                    AssetsMaintainDetail.maintainAmount MaintainAmount,
                    AssetsMaintainDetail.remark Remark
                from AssetsMaintainDetail,Assets where AssetsMaintainDetail.assetsId=Assets.assetsId and AssetsMaintainDetail.AssetsMaintainId=@AssetsMaintainId  ");
            }
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsMaintain where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsMaintain";
            DataRow dr = dt.NewRow();
            string assetsMaintainId = IdGenerator.GetMaxId(dt.TableName);
            int retApprove = InitFirstApproveTask("AssetsMaintain", "AssetsMaintainId", assetsMaintainId, viewTitle);
            foreach (KeyValuePair<string, object> kv in objs)
            {
                if (kv.Key != "gridData")
                    dr[kv.Key] = kv.Value;
            }
            if (retApprove != 0)
                dr["approveState"] = "O";
            dr["AssetsMaintainId"] = assetsMaintainId;
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);
            dbUpdate.Update(dt);
            List<AssetsMaintain> gridData = JsonHelper.JSONStringToList<AssetsMaintain>(DataConvert.ToString(objs["gridData"]));
            foreach (AssetsMaintain assetsMaintain in gridData)
            {
                AddDetail(assetsMaintain, assetsMaintainId, sysUser.UserId, viewTitle);
                if (retApprove != 0)
                {
                    UpdateAssetsState(assetsMaintain.AssetsId, "WI", sysUser.UserId, viewTitle);
                }
            }

            return 1;
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsMaintainId", pkValue);
            string sql = @"select * from AssetsMaintain where AssetsMaintainId=@AssetsMaintainId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsMaintain";
            string AssetsMaintainId = DataConvert.ToString(dt.Rows[0]["AssetsMaintainId"]);
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
            List<AssetsMaintain> gridData = JsonHelper.JSONStringToList<AssetsMaintain>(DataConvert.ToString(objs["gridData"]));
            foreach (AssetsMaintain AssetsMaintain in gridData)
            {
                AddDetail(AssetsMaintain, pkValue, sysUser.UserId, viewTitle);
            }
            if (formMode == "reapply")
                InitFirstApproveTask("AssetsMaintain", "AssetsMaintainId", AssetsMaintainId, viewTitle, formMode, sysUser.UserId);
            return 1;
        }

        protected override int DeleteData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsMaintainId", pkValue);
            string sql = @"select * from AssetsMaintain where AssetsMaintainId=@AssetsMaintainId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsMaintain";
            dt.Rows[0].Delete();
            dbUpdate.Update(dt);
            DeleteDetail(pkValue);
            DeleteApproveData("AssetsMaintain", pkValue, sysUser.UserId);
            return 1;
        }

        protected int AddDetail(AssetsMaintain AssetsMaintain, string AssetsMaintainId, string sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsMaintainDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsMaintainDetail";
            DataRow dr = dt.NewRow();
            dr["AssetsMaintainId"] = AssetsMaintainId;
            dr["assetsId"] = AssetsMaintain.AssetsId;
            dr["remark"] = AssetsMaintain.Remark;
            dr["maintainCompany"] = AssetsMaintain.MaintainCompany;
            dr["maintainAmount"] = AssetsMaintain.MaintainAmount;
            dt.Rows.Add(dr);
            return dbUpdate.Update(dt);
        }

        protected int DeleteDetail(string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsMaintainId", pkValue);
            string sql = @"select * from AssetsMaintainDetail where AssetsMaintainId=@AssetsMaintainId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsMaintainDetail";
            foreach (DataRow dr in dt.Rows)
                dr.Delete();
            return dbUpdate.Update(dt);
        }

        public override int DealEndApprove(string approvePkValue, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsMaintainId", approvePkValue);
            string sql = @"select * from AssetsMaintainDetail where assetsMaintainId=@assetsMaintainId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            foreach (DataRow dr in dtAssets.Rows)
            {
                UpdateAssetsState(DataConvert.ToString(dr["assetsId"]), "A", sysUser.UserId, viewTitle);
            }
            return 1;
        }

    }
}

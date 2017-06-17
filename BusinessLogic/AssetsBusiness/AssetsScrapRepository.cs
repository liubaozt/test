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
    public class AssetsScrap
    {
        public string AssetsId { get; set; }
        public string AssetsNo { get; set; }
        public string AssetsName { get; set; }
        public string AssetsTypeId { get; set; }
        public string AssetsClassId { get; set; }
        public string ScrapTypeId { get; set; }
        public string ScrapTypeName { get; set; }
        public string Remark { get; set; }
    }

    public class AssetsScrapRepository : ApproveMasterRepository
    {

        public AssetsScrapRepository()
        {
            DefaulteGridSortField = "assetsScrapNo";
        }


        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("assetsScrapNo") && DataConvert.ToString(paras["assetsScrapNo"]) != "")
                whereSql += @" and AssetsScrap.assetsScrapNo like '%'+@assetsScrapNo+'%'";
            if (paras.ContainsKey("assetsScrapName") && DataConvert.ToString(paras["assetsScrapName"]) != "")
                whereSql += @" and AssetsScrap.assetsScrapName like '%'+@assetsScrapName+'%'";
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
                     where AppApprove.tableName='AssetsScrap' and AppApprove.approveState='O'
                      and AssetsScrap.assetsScrapId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(paras["approveMode"]) == "reapply")
                {
                    lsql = @" where AssetsScrap.createId=@approver and AssetsScrap.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select top {0} AssetsScrap.assetsScrapId assetsScrapId,
                        AssetsScrap.assetsScrapNo assetsScrapNo,
                        AssetsScrap.assetsScrapName assetsScrapName,
                        (select userName from AppUser where AssetsScrap.createId=AppUser.userId) createId,
                        AssetsScrap.createTime createTime ,
                        (select userName from AppUser where AssetsScrap.updateId=AppUser.userId) updateId,
                        AssetsScrap.updateTime updateTime ,
                        AssetsScrap.updatePro updatePro
                from AssetsScrap  {2}  {3}", DataConvert.ToString(rowSize), AppMember.AppLanguage.ToString(), lsql, WhereSql(paras));
            return subViewSql;
        }





        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from AssetsScrap  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetAssetsScrap(string assetsScrapId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsScrapId", assetsScrapId);
            string sql = @"select * from AssetsScrap where assetsScrapId=@assetsScrapId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        public DataTable GetAssetsDetailScrap(string assetsScrapId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsScrapId", assetsScrapId);
            string sql = @"select * from AssetsScrapDetail where assetsScrapId=@assetsScrapId";
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
                    '' ScrapTypeId,
                    '' ScrapTypeName,
                    '' Remark
                from Assets where 1<>1  ");
            }
            else
            {
                if (!paras.ContainsKey("assetsScrapId"))
                    paras.Add("assetsScrapId", primaryKey);
                sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                (select departmentName from AppDepartment where Assets.departmentId=AppDepartment.departmentId) departmentId,
                    AssetsScrapDetail.scrapTypeId ScrapTypeId,
                    AssetsScrapDetail.scrapTypeId ScrapTypeName,
                    AssetsScrapDetail.remark Remark
                from AssetsScrapDetail,Assets where AssetsScrapDetail.assetsId=Assets.assetsId and AssetsScrapDetail.assetsScrapId=@assetsScrapId  ");
            }
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsScrap where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsScrap";
            DataRow dr = dt.NewRow();
            string assetsScrapId = IdGenerator.GetMaxId(dt.TableName);
            int retApprove = InitFirstApproveTask("AssetsScrap", "assetsScrapId", assetsScrapId, viewTitle);
            foreach (KeyValuePair<string, object> kv in objs)
            {
                if (kv.Key != "gridData")
                    dr[kv.Key] = kv.Value;
            }
            if (retApprove != 0)
                dr["approveState"] = "O";
            dr["assetsScrapId"] = assetsScrapId;
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);
            dbUpdate.Update(dt);

            List<AssetsScrap> gridData = JsonHelper.JSONStringToList<AssetsScrap>(DataConvert.ToString(objs["gridData"]));
            foreach (AssetsScrap assetsScrap in gridData)
            {
                AddDetail(assetsScrap, assetsScrapId, sysUser.UserId, viewTitle);
                if (retApprove != 0)
                {
                    UpdateAssetsState(assetsScrap.AssetsId, "RI", sysUser.UserId, viewTitle);
                }
            }

            return 1;
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsScrapId", pkValue);
            string sql = @"select * from AssetsScrap where assetsScrapId=@assetsScrapId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsScrap";
            string assetsScrapId = DataConvert.ToString(dt.Rows[0]["assetsScrapId"]);
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
            List<AssetsScrap> gridData = JsonHelper.JSONStringToList<AssetsScrap>(DataConvert.ToString(objs["gridData"]));
            foreach (AssetsScrap assetsScrap in gridData)
            {
                AddDetail(assetsScrap, pkValue, sysUser.UserId, viewTitle);
            }
            if (formMode == "reapply")
                InitFirstApproveTask("AssetsScrap", "assetsScrapId", assetsScrapId, viewTitle, formMode, sysUser.UserId);
            return 1;
        }

        protected override int DeleteData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsScrapId", pkValue);
            string sql = @"select * from AssetsScrap where assetsScrapId=@assetsScrapId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsScrap";
            dt.Rows[0].Delete();
            dbUpdate.Update(dt);
            DeleteDetail(pkValue);
            DeleteApproveData("AssetsScrap", pkValue, sysUser.UserId);
            return 1;
        }

        protected int AddDetail(AssetsScrap assetsScrap, string assetsScrapId, string sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsScrapDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsScrapDetail";
            DataRow dr = dt.NewRow();
            dr["assetsScrapId"] = assetsScrapId;
            dr["assetsId"] = assetsScrap.AssetsId;
            dr["remark"] = assetsScrap.Remark;
            dr["scrapTypeId"] = assetsScrap.ScrapTypeId;
            dt.Rows.Add(dr);
            return dbUpdate.Update(dt);
        }

        protected int DeleteDetail(string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsScrapId", pkValue);
            string sql = @"select * from AssetsScrapDetail where assetsScrapId=@assetsScrapId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsScrapDetail";
            foreach (DataRow dr in dt.Rows)
                dr.Delete();
            return dbUpdate.Update(dt);
        }

        public override int DealEndApprove(string approvePkValue, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsScrapId", approvePkValue);
            string sql = @"select * from AssetsScrapDetail where assetsScrapId=@assetsScrapId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            foreach (DataRow dr in dtAssets.Rows)
            {
                UpdateAssetsState(DataConvert.ToString(dr["assetsId"]), "A", sysUser.UserId, viewTitle);
            }
            return 1;
        }

    }
}

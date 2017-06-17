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
    public class AssetsCheck
    {
        public string AssetsId { get; set; }
        public string AssetsNo { get; set; }
        public string AssetsName { get; set; }
        public string AssetsTypeId { get; set; }
        public string AssetsClassId { get; set; }
        public string Remark { get; set; }
    }

    public class AssetsCheckRepository : ApproveMasterRepository
    {

        public AssetsCheckRepository()
        {
            DefaulteGridSortField = "AssetsCheckNo";
        }


        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("AssetsCheckNo") && DataConvert.ToString(paras["AssetsCheckNo"]) != "")
                whereSql += @" and AssetsCheck.AssetsCheckNo like '%'+@AssetsCheckNo+'%'";
            if (paras.ContainsKey("AssetsCheckName") && DataConvert.ToString(paras["AssetsCheckName"]) != "")
                whereSql += @" and AssetsCheck.AssetsCheckName like '%'+@AssetsCheckName+'%'";
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
                     where AppApprove.tableName='AssetsCheck' and AppApprove.approveState='O'
                      and AssetsCheck.AssetsCheckId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(paras["approveMode"]) == "reapply")
                {
                    lsql = @" where AssetsCheck.createId=@approver and AssetsCheck.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select top {0} AssetsCheck.AssetsCheckId AssetsCheckId,
                        AssetsCheck.AssetsCheckNo AssetsCheckNo,
                        AssetsCheck.AssetsCheckName AssetsCheckName,
                        (select userName from AppUser where AssetsCheck.createId=AppUser.userId) createId,
                        AssetsCheck.createTime createTime ,
                        (select userName from AppUser where AssetsCheck.updateId=AppUser.userId) updateId,
                        AssetsCheck.updateTime updateTime ,
                        AssetsCheck.updatePro updatePro
                from AssetsCheck  {2}  {3}", DataConvert.ToString(rowSize), AppMember.AppLanguage.ToString(), lsql, WhereSql(paras));
            return subViewSql;
        }





        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from AssetsCheck  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetAssetsCheck(string AssetsCheckId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsCheckId", AssetsCheckId);
            string sql = @"select * from AssetsCheck where AssetsCheckId=@AssetsCheckId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        public DataTable GetAssetsDetailScrap(string AssetsCheckId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsCheckId", AssetsCheckId);
            string sql = @"select * from AssetsCheckDetail where AssetsCheckId=@AssetsCheckId";
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
                    '' Remark
                from Assets where 1<>1  ");
            }
            else
            {
                if (!paras.ContainsKey("AssetsCheckId"))
                    paras.Add("AssetsCheckId", primaryKey);
                sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                    AssetsCheckDetail.remark Remark
                from AssetsCheckDetail,Assets where AssetsCheckDetail.assetsId=Assets.assetsId and AssetsCheckDetail.AssetsCheckId=@AssetsCheckId  ");
            }
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsCheck where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsCheck";
            DataRow dr = dt.NewRow();
            string assetsCheckId = IdGenerator.GetMaxId(dt.TableName);
            int retApprove = InitFirstApproveTask("AssetsCheck", "AssetsCheckId", assetsCheckId, viewTitle);
            foreach (KeyValuePair<string, object> kv in objs)
            {
                if (kv.Key != "gridData")
                    dr[kv.Key] = kv.Value;
            }
            if (retApprove != 0)
                dr["approveState"] = "O";
            dr["AssetsCheckId"] = assetsCheckId;
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);
            dbUpdate.Update(dt);

            List<AssetsCheck> gridData =JsonHelper.JSONStringToList<AssetsCheck>(DataConvert.ToString(objs["gridData"]));
            foreach (AssetsCheck assetsCheck in gridData)
            {
                AddDetail(assetsCheck, assetsCheckId, sysUser.UserId, viewTitle);
                if (retApprove != 0)
                {
                    UpdateAssetsState(assetsCheck.AssetsId, "CI", sysUser.UserId, viewTitle);
                }
            }

            return 1;
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsCheckId", pkValue);
            string sql = @"select * from AssetsCheck where AssetsCheckId=@AssetsCheckId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsCheck";
            string AssetsCheckId = DataConvert.ToString(dt.Rows[0]["AssetsCheckId"]);
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
            List<AssetsCheck> gridData = JsonHelper.JSONStringToList<AssetsCheck>(DataConvert.ToString(objs["gridData"]));
            foreach (AssetsCheck AssetsCheck in gridData)
            {
                AddDetail(AssetsCheck, pkValue, sysUser.UserId, viewTitle);
            }
            if (formMode == "reapply")
                InitFirstApproveTask("AssetsCheck", "AssetsCheckId", AssetsCheckId, viewTitle, formMode, sysUser.UserId);
            return 1;
        }

        protected override int DeleteData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsCheckId", pkValue);
            string sql = @"select * from AssetsCheck where AssetsCheckId=@AssetsCheckId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsCheck";
            dt.Rows[0].Delete();
            dbUpdate.Update(dt);
            DeleteDetail(pkValue);
            DeleteApproveData("AssetsCheck", pkValue, sysUser.UserId);
            return 1;
        }

        protected int AddDetail(AssetsCheck AssetsCheck, string AssetsCheckId, string sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsCheckDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsCheckDetail";
            DataRow dr = dt.NewRow();
            dr["AssetsCheckId"] = AssetsCheckId;
            dr["assetsId"] = AssetsCheck.AssetsId;
            dr["remark"] = AssetsCheck.Remark;
            dt.Rows.Add(dr);
            return dbUpdate.Update(dt);
        }

        protected int DeleteDetail(string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsCheckId", pkValue);
            string sql = @"select * from AssetsCheckDetail where AssetsCheckId=@AssetsCheckId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsCheckDetail";
            foreach (DataRow dr in dt.Rows)
                dr.Delete();
            return dbUpdate.Update(dt);
        }

        public override int DealEndApprove(string approvePkValue, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsCheckId", approvePkValue);
            string sql = @"select * from AssetsCheckDetail where assetsCheckId=@assetsCheckId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            foreach (DataRow dr in dtAssets.Rows)
            {
                UpdateAssetsState(DataConvert.ToString(dr["assetsId"]), "A", sysUser.UserId, viewTitle);
            }
            return 1;
        }


    }
}

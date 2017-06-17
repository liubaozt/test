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
    public class AssetsReturn
    {
        public string AssetsId { get; set; }
        public string AssetsNo { get; set; }
        public string AssetsName { get; set; }
        public string AssetsTypeId { get; set; }
        public string AssetsClassId { get; set; }
        public string Remark { get; set; }
    }

    public class AssetsReturnRepository : ApproveMasterRepository
    {

        public AssetsReturnRepository()
        {
            DefaulteGridSortField = "assetsReturnNo";
        }


        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("assetsReturnNo") && DataConvert.ToString(paras["assetsReturnNo"]) != "")
                whereSql += @" and AssetsReturn.assetsReturnNo like '%'+@assetsReturnNo+'%'";
            if (paras.ContainsKey("assetsReturnName") && DataConvert.ToString(paras["assetsReturnName"]) != "")
                whereSql += @" and AssetsReturn.assetsReturnName like '%'+@assetsReturnName+'%'";
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
                     where AppApprove.tableName='AssetsReturn' and AppApprove.approveState='O'
                      and AssetsReturn.assetsReturnId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(paras["approveMode"]) == "reapply")
                {
                    lsql = @" where AssetsReturn.createId=@approver and AssetsReturn.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select top {0} AssetsReturn.assetsReturnId assetsReturnId,
                        AssetsReturn.assetsReturnNo assetsReturnNo,
                        AssetsReturn.assetsReturnName assetsReturnName,
                        (select userName from AppUser where AssetsReturn.createId=AppUser.userId) createId,
                        AssetsReturn.createTime createTime ,
                        (select userName from AppUser where AssetsReturn.updateId=AppUser.userId) updateId,
                        AssetsReturn.updateTime updateTime ,
                        AssetsReturn.updatePro updatePro
                from AssetsReturn  {2}  {3}", DataConvert.ToString(rowSize), AppMember.AppLanguage.ToString(), lsql, WhereSql(paras));
            return subViewSql;
        }





        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from AssetsReturn  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetAssetsReturn(string assetsReturnId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsReturnId", assetsReturnId);
            string sql = @"select * from AssetsReturn where assetsReturnId=@assetsReturnId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        public DataTable GetAssetsDetailScrap(string assetsReturnId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsReturnId", assetsReturnId);
            string sql = @"select * from AssetsReturnDetail where assetsReturnId=@assetsReturnId";
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
                if (!paras.ContainsKey("assetsReturnId"))
                    paras.Add("assetsReturnId", primaryKey);
                sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                    AssetsReturnDetail.remark Remark
                from AssetsReturnDetail,Assets where AssetsReturnDetail.assetsId=Assets.assetsId and AssetsReturnDetail.assetsReturnId=@assetsReturnId  ");
            }
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsReturn where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsReturn";
            DataRow dr = dt.NewRow();
            string assetsReturnId = IdGenerator.GetMaxId(dt.TableName);
            int retApprove = InitFirstApproveTask("AssetsReturn", "assetsReturnId", assetsReturnId, viewTitle);
            foreach (KeyValuePair<string, object> kv in objs)
            {
                if (kv.Key != "gridData")
                    dr[kv.Key] = kv.Value;
            }
            if (retApprove != 0)
                dr["approveState"] = "O";
            dr["assetsReturnId"] = assetsReturnId;
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);
            dbUpdate.Update(dt);

            List<AssetsReturn> gridData = JsonHelper.JSONStringToList<AssetsReturn>(DataConvert.ToString(objs["gridData"]));
            foreach (AssetsReturn assetsReturn in gridData)
            {
                AddDetail(assetsReturn, assetsReturnId, sysUser.UserId, viewTitle);
                if (retApprove != 0)
                {
                    UpdateAssetsState(assetsReturn.AssetsId, "HI", sysUser.UserId, viewTitle);
                }
            }
            return 1;
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsReturnId", pkValue);
            string sql = @"select * from AssetsReturn where assetsReturnId=@assetsReturnId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsReturn";
            string assetsReturnId = DataConvert.ToString(dt.Rows[0]["assetsReturnId"]);
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
            List<AssetsReturn> gridData = JsonHelper.JSONStringToList<AssetsReturn>(DataConvert.ToString(objs["gridData"]));
            foreach (AssetsReturn assetsReturn in gridData)
            {
                AddDetail(assetsReturn, pkValue, sysUser.UserId, viewTitle);
            }
            if (formMode == "reapply")
                InitFirstApproveTask("AssetsReturn", "assetsReturnId", assetsReturnId, viewTitle, formMode, sysUser.UserId);
            return 1;
        }

        protected override int DeleteData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsReturnId", pkValue);
            string sql = @"select * from AssetsReturn where assetsReturnId=@assetsReturnId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsReturn";
            dt.Rows[0].Delete();
            dbUpdate.Update(dt);
            DeleteDetail(pkValue);
            DeleteApproveData("AssetsReturn", pkValue, sysUser.UserId);
            return 1;
        }

        protected int AddDetail(AssetsReturn assetsReturn, string assetsReturnId, string sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsReturnDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsReturnDetail";
            DataRow dr = dt.NewRow();
            dr["assetsReturnId"] = assetsReturnId;
            dr["assetsId"] = assetsReturn.AssetsId;
            dr["remark"] = assetsReturn.Remark;

            dt.Rows.Add(dr);
            return dbUpdate.Update(dt);
        }

        protected int DeleteDetail(string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsReturnId", pkValue);
            string sql = @"select * from AssetsReturnDetail where assetsReturnId=@assetsReturnId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsReturnDetail";
            foreach (DataRow dr in dt.Rows)
                dr.Delete();
            return dbUpdate.Update(dt);
        }

        public override int DealEndApprove(string approvePkValue, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsReturnId", approvePkValue);
            string sql = @"select * from AssetsReturnDetail where assetsReturnId=@assetsReturnId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            foreach (DataRow dr in dtAssets.Rows)
            {
                UpdateAssetsState(DataConvert.ToString(dr["assetsId"]), "A", sysUser.UserId, viewTitle);
            }
            return 1;
        }
    }
}

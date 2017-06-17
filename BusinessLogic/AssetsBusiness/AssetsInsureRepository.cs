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
    public class AssetsInsure
    {
        public string AssetsId { get; set; }
        public string AssetsNo { get; set; }
        public string AssetsName { get; set; }
        public string AssetsTypeId { get; set; }
        public string AssetsClassId { get; set; }
        public string InsureCompany { get; set; }
        public string InsureAmount { get; set; }
        public string Remark { get; set; }
    }

    public class AssetsInsureRepository : ApproveMasterRepository
    {

        public AssetsInsureRepository()
        {
            DefaulteGridSortField = "AssetsInsureNo";
        }


        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("AssetsInsureNo") && DataConvert.ToString(paras["AssetsInsureNo"]) != "")
                whereSql += @" and AssetsInsure.AssetsInsureNo like '%'+@AssetsInsureNo+'%'";
            if (paras.ContainsKey("AssetsInsureName") && DataConvert.ToString(paras["AssetsInsureName"]) != "")
                whereSql += @" and AssetsInsure.AssetsInsureName like '%'+@AssetsInsureName+'%'";
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
                     where AppApprove.tableName='AssetsInsure' and AppApprove.approveState='O'
                      and AssetsInsure.AssetsInsureId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(paras["approveMode"]) == "reapply")
                {
                    lsql = @" where AssetsInsure.createId=@approver and AssetsInsure.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select top {0} AssetsInsure.AssetsInsureId AssetsInsureId,
                        AssetsInsure.AssetsInsureNo AssetsInsureNo,
                        AssetsInsure.AssetsInsureName AssetsInsureName,
                        (select userName from AppUser where AssetsInsure.createId=AppUser.userId) createId,
                        AssetsInsure.createTime createTime ,
                        (select userName from AppUser where AssetsInsure.updateId=AppUser.userId) updateId,
                        AssetsInsure.updateTime updateTime ,
                        AssetsInsure.updatePro updatePro
                from AssetsInsure  {2}  {3}", DataConvert.ToString(rowSize), AppMember.AppLanguage.ToString(), lsql, WhereSql(paras));
            return subViewSql;
        }





        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from AssetsInsure  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetAssetsInsure(string AssetsInsureId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsInsureId", AssetsInsureId);
            string sql = @"select * from AssetsInsure where AssetsInsureId=@AssetsInsureId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        public DataTable GetAssetsDetailScrap(string AssetsInsureId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsInsureId", AssetsInsureId);
            string sql = @"select * from AssetsInsureDetail where AssetsInsureId=@AssetsInsureId";
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
                    '' InsureCompany,
                    '' InsureAmount,
                    '' Remark
                from Assets where 1<>1  ");
            }
            else
            {
                if (!paras.ContainsKey("AssetsInsureId"))
                    paras.Add("AssetsInsureId", primaryKey);
                sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                    AssetsInsureDetail.InsureCompany InsureCompany,
                    AssetsInsureDetail.InsureAmount InsureAmount,
                    AssetsInsureDetail.remark Remark
                from AssetsInsureDetail,Assets where AssetsInsureDetail.assetsId=Assets.assetsId and AssetsInsureDetail.AssetsInsureId=@AssetsInsureId  ");
            }
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsInsure where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsInsure";
            DataRow dr = dt.NewRow();
            string assetsInsureId = IdGenerator.GetMaxId(dt.TableName);
            int retApprove = InitFirstApproveTask("AssetsInsure", "AssetsInsureId", assetsInsureId, viewTitle);
            foreach (KeyValuePair<string, object> kv in objs)
            {
                if (kv.Key != "gridData")
                    dr[kv.Key] = kv.Value;
            }
            if (retApprove != 0)
                dr["approveState"] = "O";
            dr["AssetsInsureId"] = assetsInsureId;
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);
            dbUpdate.Update(dt);

            List<AssetsInsure> gridData = JsonHelper.JSONStringToList<AssetsInsure>(DataConvert.ToString(objs["gridData"]));
            foreach (AssetsInsure assetsInsure in gridData)
            {
                AddDetail(assetsInsure, assetsInsureId, sysUser.UserId, viewTitle);
                if (retApprove != 0)
                {
                    UpdateAssetsState(assetsInsure.AssetsId, "II", sysUser.UserId, viewTitle);
                }
            }

            return 1;
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsInsureId", pkValue);
            string sql = @"select * from AssetsInsure where AssetsInsureId=@AssetsInsureId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsInsure";
            string AssetsInsureId = DataConvert.ToString(dt.Rows[0]["AssetsInsureId"]);
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
            List<AssetsInsure> gridData = JsonHelper.JSONStringToList<AssetsInsure>(DataConvert.ToString(objs["gridData"]));
            foreach (AssetsInsure AssetsInsure in gridData)
            {
                AddDetail(AssetsInsure, pkValue, sysUser.UserId, viewTitle);
            }
            if (formMode == "reapply")
                InitFirstApproveTask("AssetsInsure", "AssetsInsureId", AssetsInsureId, viewTitle, formMode, sysUser.UserId);
            return 1;
        }

        protected override int DeleteData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsInsureId", pkValue);
            string sql = @"select * from AssetsInsure where AssetsInsureId=@AssetsInsureId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsInsure";
            dt.Rows[0].Delete();
            dbUpdate.Update(dt);
            DeleteDetail(pkValue);
            DeleteApproveData("AssetsInsure", pkValue, sysUser.UserId);
            return 1;
        }

        protected int AddDetail(AssetsInsure AssetsInsure, string AssetsInsureId, string sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsInsureDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsInsureDetail";
            DataRow dr = dt.NewRow();
            dr["AssetsInsureId"] = AssetsInsureId;
            dr["assetsId"] = AssetsInsure.AssetsId;
            dr["remark"] = AssetsInsure.Remark;
            dr["insureCompany"] = AssetsInsure.InsureCompany;
            dr["insureAmount"] = AssetsInsure.InsureAmount;
            dt.Rows.Add(dr);
            return dbUpdate.Update(dt);
        }

        protected int DeleteDetail(string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsInsureId", pkValue);
            string sql = @"select * from AssetsInsureDetail where AssetsInsureId=@AssetsInsureId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsInsureDetail";
            foreach (DataRow dr in dt.Rows)
                dr.Delete();
            return dbUpdate.Update(dt);
        }

        public override int DealEndApprove(string approvePkValue, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsInsureId", approvePkValue);
            string sql = @"select * from AssetsInsureDetail where assetsInsureId=@assetsInsureId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            foreach (DataRow dr in dtAssets.Rows)
            {
                UpdateAssetsState(DataConvert.ToString(dr["assetsId"]), "A", sysUser.UserId, viewTitle);
            }
            return 1;
        }

    }
}

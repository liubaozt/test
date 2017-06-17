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
    public class AssetsRepository : ApproveMasterRepository
    {

        public AssetsRepository()
        {
            DefaulteGridSortField = "assetsNo";
        }

        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("assetsNo") && DataConvert.ToString(paras["assetsNo"]) != "")
                whereSql += @" and Assets.assetsNo like '%'+@assetsNo+'%'";
            if (paras.ContainsKey("assetsName") && DataConvert.ToString(paras["assetsName"]) != "")
                whereSql += @" and Assets.assetsName like '%'+@assetsName+'%'";
            return whereSql;
        }

        protected override string SubSelectSql(Dictionary<string, object> paras)
        {
            if (paras.ContainsKey("approveMode"))
            {
                return ApproveSubSelectSql(paras);
            }
            int pageIndex = DataConvert.ToInt32(paras["pageIndex"]);
            int rowNum = DataConvert.ToInt32(paras["rowNum"]);
            int rowSize = pageIndex * rowNum; //子查询返回行数的尺寸
            string subViewSql = string.Format(@"select top {0} Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
	                (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
	                (select assetsUsesName from AssetsUses where Assets.assetsUsesId=AssetsUses.assetsUsesId) assetsUsesId,
                    Assets.departmentId departmentId,
	                (select departmentName from AppDepartment where Assets.departmentId=AppDepartment.departmentId) departmentName,
	                Assets.storeSiteId storeSiteId,
                   (select storeSiteName from StoreSite where Assets.storeSiteId=StoreSite.storeSiteId) storeSiteName,
	                (select CodeTable.codeName from CodeTable where Assets.depreciationType=CodeTable.codeNo and CodeTable.codeType='DepreciationType' and CodeTable.languageVer='{1}' ) depreciationType,
	                (select equityOwnerName from EquityOwner where Assets.equityOwnerId=EquityOwner.equityOwnerId) equityOwnerId,
	                (select purchaseTypeName from PurchaseType where Assets.purchaseTypeId=PurchaseType.purchaseTypeId) purchaseTypeId,
	                (select unitName from Unit where Assets.unitId=Unit.unitId) unitId,
	                Assets.assetsBarcode assetsBarcode,
	                Assets.assetsValue assetsValue,
	                Assets.durableYears durableYears,
	                Assets.remainRate remainRate,
	                Assets.purchaseDate purchaseDate,
	                Assets.purchaseNo purchaseNo,
	                Assets.invoiceNo invoiceNo,
                    Assets.usePeople usePeople,
	                (select userName from AppUser where Assets.usePeople=AppUser.userId) usePeopleName,
                    Assets.keeper keeper,
	                (select userName from AppUser where Assets.keeper=AppUser.userId) keeperName,
	                (select CodeTable.codeName from CodeTable where Assets.assetsState=CodeTable.codeNo and CodeTable.codeType='AssetsState' and CodeTable.languageVer='{1}' ) assetsState,
	                Assets.guaranteeDays guaranteeDays,
	                Assets.maintainDays maintainDays,
                              (select userName from AppUser where Assets.createId=AppUser.userId) createId,
                              Assets.createTime createTime ,
                              (select userName from AppUser where Assets.updateId=AppUser.userId) updateId,
                              Assets.updateTime updateTime ,
                              Assets.updatePro updatePro
                from Assets 
                where 1=1  {2}", DataConvert.ToString(rowSize), AppMember.AppLanguage.ToString(), WhereSql(paras));
            return subViewSql;
        }

        protected string ApproveSubSelectSql(Dictionary<string, object> paras)
        {
            int pageIndex = DataConvert.ToInt32(paras["pageIndex"]);
            int rowNum = DataConvert.ToInt32(paras["rowNum"]);
            int rowSize = pageIndex * rowNum; //子查询返回行数的尺寸
            string lsql = "";
            if (paras.ContainsKey("approveMode"))
            {
                if (DataConvert.ToString(paras["approveMode"]) == "approve")
                {
                    lsql = @",AppApprove
                     where AppApprove.tableName='Assets' and AppApprove.approveState='O'
                      and Assets.assetsId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(paras["approveMode"]) == "reapply")
                {
                    lsql = @" where Assets.createId=@approver and Assets.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select top {0} Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                       (select CodeTable.codeName from CodeTable where Assets.approveState=CodeTable.codeNo and CodeTable.codeType='ApproveState' and CodeTable.languageVer='{1}' ) approveState,
                       (select userNo from AppUser where Assets.createId=AppUser.userId) createId,
                        Assets.createTime createTime ,
                        (select userNo from AppUser where Assets.updateId=AppUser.userId) updateId,
                        Assets.updateTime updateTime ,
                        Assets.updatePro updatePro
                from Assets {2} {3}", DataConvert.ToString(rowSize), AppMember.AppLanguage.ToString(), lsql, WhereSql(paras));
            return subViewSql;
        }

        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from Assets  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetAssets(string assetsId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsId", assetsId);
            string sql = @"select * from Assets where assetsId=@assetsId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        public string GetDefaultAssetsId()
        {
            string sql = @"select top 1 * from Assets";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return DataConvert.ToString(dtGrid.Rows[0]["assetsId"]);
        }

        public DataTable GetAssetsImg(string assetsId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsId", assetsId);
            string sql = @"select * from AppImg where refId=@assetsId and tableName='Assets'";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }


        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from Assets where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, dbUpdate.cmd).Tables[0];
            dt.TableName = "Assets";
            DataRow dr = dt.NewRow();
            string assetsId = IdGenerator.GetMaxId(dt.TableName);
            foreach (KeyValuePair<string, object> kv in objs)
            {
                if (kv.Key == "imgFileContainer")
                    AddImg(DataConvert.ToString(kv.Value), assetsId, sysUser, viewTitle);
                else
                    dr[kv.Key] = kv.Value;
            }
            int retApprove = InitFirstApproveTask("Assets", "assetsId", assetsId, viewTitle);
            if (retApprove != 0)
            {
                dr["assetsState"] = "O";
                dr["approveState"] = "O";
            }
            else
                dr["assetsState"] = "A";
            dr["assetsId"] = assetsId;
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);
            dbUpdate.Update(dt);
            return 1;
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsId", pkValue);
            string sql = @"select * from Assets where assetsId=@assetsId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "Assets";
            string assetsId = DataConvert.ToString(dt.Rows[0]["assetsId"]);
            foreach (KeyValuePair<string, object> kv in objs)
            {
                if (kv.Key == "imgFileContainer")
                {
                    DeleteImg(sysUser, assetsId);
                    AddImg(DataConvert.ToString(kv.Value), assetsId, sysUser, viewTitle);
                }
                else
                    dt.Rows[0][kv.Key] = kv.Value;
            }
            if (formMode == "reapply")
                dt.Rows[0]["approveState"] = "O";
            Update5Field(dt, sysUser.UserId, viewTitle);
            dbUpdate.Update(dt);
            if (formMode == "reapply")
                InitFirstApproveTask("Assets", "assetsId", assetsId, viewTitle, formMode, sysUser.UserId);
            return 1;
        }

        protected override int DeleteData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsId", pkValue);
            string sql = @"select * from Assets where assetsId=@assetsId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "Assets";
            DeleteImg(sysUser, pkValue);
            dt.Rows[0].Delete();
            dbUpdate.Update(dt);
            DeleteApproveData("Assets", pkValue, sysUser.UserId);
            return 1;
        }

        protected int AddImg(string imgContainer, string assetsId, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AppImg where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, dbUpdate.cmd).Tables[0];
            dt.TableName = "AppImg";
            string[] imgArray = imgContainer.Split(';');
            foreach (string img in imgArray)
            {
                if (DataConvert.ToString(img) != "")
                {
                    DataRow dr = dt.NewRow();
                    dr["imgName"] = img;
                    dr["imgId"] = IdGenerator.GetMaxId(dt.TableName);
                    dr["tableName"] = "Assets";
                    dr["refId"] = assetsId;
                    Create5Field(dr, sysUser.UserId, viewTitle);
                    dt.Rows.Add(dr);
                }
            }
            return dbUpdate.Update(dt);
        }

        protected int DeleteImg(UserInfo sysUser, string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsId", pkValue);
            string sql = @"select * from AppImg where refId=@assetsId and tableName='Assets'";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, dbUpdate.cmd).Tables[0];
            dt.TableName = "AppImg";
            foreach (DataRow dr in dt.Rows)
            {
                dr.Delete();
            }
            return dbUpdate.Update(dt);
        }

    }
}

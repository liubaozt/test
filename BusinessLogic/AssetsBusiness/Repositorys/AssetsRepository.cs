using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.AssetsBusiness.Models.AssetsManage;
using BaseCommon.Models;
using BusinessLogic.BasicData.Repositorys;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsRepository : ApproveMasterRepository
    {

        public AssetsRepository()
        {
            DefaulteGridSortField = " updateTime ";
            MasterTable = "Assets";
        }

        protected WhereConditon AccessLevelWhereSql(ListCondition condition, WhereConditon wcd)
        {
            if (condition.SysUser.IsSysUser != "Y")
            {
                wcd.Sql += @" and Assets.keeper=@curkeeper";
                wcd.DBPara.Add("curkeeper", condition.SysUser.UserId);
            }
            else
            {
                if (condition.SysUser.AccessLevel == "A")
                {
                    if (condition.SysUser.UserNo!="sa" && condition.SysUser.IsHeaderOffice != "Y")
                    {
                        wcd.Sql += @" and D.companyId=@curcompanyId";
                        wcd.DBPara.Add("curcompanyId", condition.SysUser.CompanyId);
                    }
                }
                else if (condition.SysUser.AccessLevel == "C")
                {
                    wcd.Sql += @" and D.companyId=@curcompanyId";
                    wcd.DBPara.Add("curcompanyId", condition.SysUser.CompanyId);
                }
                else if (condition.SysUser.AccessLevel == "D")
                {
                    //wcd.Sql += @" and D.departmentId=@curdepartmentId";
                    //wcd.DBPara.Add("curdepartmentId", condition.SysUser.DepartmentId);
                    string ins = "";
                    foreach (var department in condition.SysUser.Departments)
                    {
                        ins += "'" + department.DepartmentId+"',";
                    }
                    if (ins.Length > 0)
                        ins = ins.Substring(0, ins.Length - 1);
                    wcd.Sql += string.Format(@" and D.departmentId in({0})", ins);
                }
                else if (condition.SysUser.AccessLevel == "M")
                {
                    wcd.Sql += @" and Assets.keeper=@curkeeper";
                    wcd.DBPara.Add("curkeeper", condition.SysUser.UserId);
                }
                else
                {
                    wcd.Sql += @" and Assets.keeper=@curkeeper";
                    wcd.DBPara.Add("curkeeper", condition.SysUser.UserId);
                }
            }
            return wcd;
        }
        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            wcd = AccessLevelWhereSql(condition, wcd);
            ApproveListCondition acondition = condition as ApproveListCondition;
            if (DataConvert.ToString(acondition.ListMode) != "")
                wcd.DBPara.Add("approver", acondition.Approver);
            ListModel model = JsonHelper.Deserialize<ListModel>(acondition.ListModelString);
            if (DataConvert.ToString(condition.FilterString) != "")
            {
                wcd.Sql += @" and " + DFT.HandleExpress(condition.FilterString);
            }
            if (model == null) return wcd;
            if (DataConvert.ToString(model.AssetsNo) != "")
            {
                wcd.Sql += @" and Assets.assetsNo like '%'+@assetsNo+'%'";
                wcd.DBPara.Add("assetsNo", model.AssetsNo);
            }
            if (DataConvert.ToString(model.AssetsName) != "")
            {
                wcd.Sql += @" and Assets.assetsName like '%'+@assetsName+'%'";
                wcd.DBPara.Add("assetsName", model.AssetsName);
            }
            if (DataConvert.ToString(model.AssetsBarcode) != "")
            {
                wcd.Sql += @" and Assets.assetsBarcode like '%'+@assetsBarcode+'%'";
                wcd.DBPara.Add("assetsBarcode", model.AssetsBarcode);
            }
            if (DataConvert.ToString(model.AssetsClassId) != "")
            {
                wcd.Sql += @" and Assets.assetsClassId=@assetsClassId";
                wcd.DBPara.Add("assetsClassId", model.AssetsClassId);
            }
            if (DataConvert.ToString(model.DepartmentId) != "")
            {
                wcd.Sql += @" and Assets.departmentId=@departmentId";
                wcd.DBPara.Add("departmentId", model.DepartmentId);
            }
            if (DataConvert.ToString(model.StoreSiteId) != "")
            {
                wcd.Sql += @" and Assets.storeSiteId=@storeSiteId";
                wcd.DBPara.Add("storeSiteId", model.StoreSiteId);
            }
            if (DataConvert.ToString(model.AssetsState) != "")
            {
                wcd.Sql += @" and Assets.assetsState=@assetsState";
                wcd.DBPara.Add("assetsState", model.AssetsState);
            }
            if (DataConvert.ToString(model.UsePeople) != "")
            {
                wcd.Sql += @" and  Assets.usePeople like '%'+@usePeople+'%'";
                wcd.DBPara.Add("usePeople", model.UsePeople);
            }
            if (DataConvert.ToString(model.Keeper) != "")
            {
                wcd.Sql += @" and  Assets.keeper like '%'+@keeper+'%'";
                wcd.DBPara.Add("keeper", model.Keeper);
            }
            if (DataConvert.ToString(model.ProjectManageId) != "")
            {
                wcd.Sql += @" and Assets.projectManageId=@projectManageId";
                wcd.DBPara.Add("projectManageId", model.ProjectManageId);
            }
            if (DataConvert.ToString(model.PurchaseDateFrom) != "")
            {
                wcd.Sql += @" and Assets.purchaseDate>=@purchaseDateFrom";
                wcd.DBPara.Add("purchaseDateFrom", DataConvert.ToString(model.PurchaseDateFrom) + " 00:00:00");
            }
            if (DataConvert.ToString(model.PurchaseDateTo) != "")
            {
                wcd.Sql += @" and Assets.purchaseDate<=@purchaseDateTo";
                wcd.DBPara.Add("purchaseDateTo", DataConvert.ToString(model.PurchaseDateTo) + " 23:59:59");
            }
            if (DataConvert.ToString(model.CompanyId) != "")
            {
                wcd.Sql += @" and D.companyId=@companyId";
                wcd.DBPara.Add("companyId", model.CompanyId);
            }
            if (DataConvert.ToString(model.Spec) != "")
            {
                wcd.Sql += @" and Assets.spec like '%'+@spec+'%'";
                wcd.DBPara.Add("spec", model.Spec);
            }
            if (DataConvert.ToString(model.Remark) != "")
            {
                wcd.Sql += @" and Assets.remark like '%'+@remark+'%'";
                wcd.DBPara.Add("remark", model.Remark);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            ApproveListCondition acondition = condition as ApproveListCondition;
            if (DataConvert.ToString(acondition.ListMode) != "")
            {
                return ApproveListSql(acondition);
            }
            else
            {
                if (acondition.SelectMode == "AssetsSelect")
                {
                    return AssetsSelectListSql(acondition);
                }
                else if (acondition.SelectMode == "BorrowAssetsSelect")
                {
                    return BorrowAssetsSelectListSql(acondition);
                }
                else
                {
                    return AssetsListSql(acondition);
                }
            }
        }

        protected string AssetsListSql(ApproveListCondition condition)
        {
            string sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
	                (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
	                (select assetsUsesName from AssetsUses where Assets.assetsUsesId=AssetsUses.assetsUsesId) assetsUsesId,
                   (select departmentName from AppDepartment C where D.companyId=C.departmentId ) companyId,
                    Assets.departmentId departmentId,
	                D.departmentName departmentName,
	                Assets.storeSiteId storeSiteId,
                   (select storeSiteName from StoreSite where Assets.storeSiteId=StoreSite.storeSiteId) storeSiteName,
	                (select CodeTable.codeName from CodeTable where Assets.depreciationType=CodeTable.codeNo and CodeTable.codeType='DepreciationType' and CodeTable.languageVer='{0}' ) depreciationType,
	                (select equityOwnerName from EquityOwner where Assets.equityOwnerId=EquityOwner.equityOwnerId) equityOwnerId,
	                (select purchaseTypeName from PurchaseType where Assets.purchaseTypeId=PurchaseType.purchaseTypeId) purchaseTypeId,
	                (select unitName from Unit where Assets.unitId=Unit.unitId) unitId,
	                Assets.assetsBarcode assetsBarcode,
	                Assets.assetsValue assetsValue,
                    Convert(decimal(18,2), Assets.assetsNetValue)  assetsNetValue,
                    {2} assetsRemainValue,
	                Assets.durableYears durableYears,
                    Assets.assetsQty assetsQty,
	                Assets.remainRate remainRate,
	                convert(nvarchar(100),Assets.purchaseDate,23) purchaseDate,
                    convert(nvarchar(100),{1},23) duetoDate,
                    convert(nvarchar(100),Assets.purchaseDate2,23)  purchaseDate2,
	                Assets.purchaseNo purchaseNo,
                   (select assetsPurchaseNo from AssetsPurchase where Assets.assetsPurchaseId=AssetsPurchase.assetsPurchaseId) assetsPurchaseNo,
	                Assets.invoiceNo invoiceNo,
                    Assets.usePeople usePeople,
	                isnull((select userName from AppUser where Assets.usePeople=AppUser.userId),Assets.usePeople) usePeopleName,
                    Assets.keeper keeper,
	                isnull((select userName from AppUser where Assets.keeper=AppUser.userId),Assets.keeper) keeperName,
	                (select CodeTable.codeName from CodeTable where Assets.assetsState=CodeTable.codeNo and CodeTable.codeType='AssetsState' and CodeTable.languageVer='{0}' ) assetsState,
	                Assets.guaranteeDays guaranteeDays,
	                Assets.maintainDays maintainDays,
                   (select projectManageName from ProjectManage where Assets.projectManageId=ProjectManage.projectManageId) projectManageId,
                     Assets.remark remark,
                     Assets.CEANo CEANo,
                     Assets.TagMaterial TagMaterial,
                   convert(nvarchar(100), Assets.scrapDate,23)  scrapDate,
                     Assets.supplierName supplierName,
                     Assets.markMK markMK,
                     Assets.mujuNo mujuNo,
                     Assets.shengchanhuopinNo shengchanhuopinNo,
                     Assets.mujuxueshu mujuxueshu,
                     Assets.mujushouming mujushouming,
                     Assets.shejichannenng shejichannenng,
                     Assets.hadshejichannneng hadshejichannneng,
                    Assets.shiYongDiDian shiYongDiDian,
                    Assets.panDianHao panDianHao,
                    Assets.spec spec,
                  (select depreciationRuleNo from DepreciationRule where DepreciationRule.depreciationRuleId=Assets.depreciationRule) depreciationRuleId,
                   (select userName from AppUser where Assets.createId=AppUser.userId) createId,
                    Assets.createTime createTime ,
                    (select userName from AppUser where Assets.updateId=AppUser.userId) updateId,
                    Assets.updateTime updateTime ,
                    Assets.updatePro updatePro
                from Assets 
               left join AppDepartment D on Assets.departmentId=D.departmentId 
                where 1=1 ", AppMember.AppLanguage.ToString(), DuetoDateSql(), RemainValueSql());
            return sql;
        }

        private string DuetoDateSql()
        {
            if (AppMember.DepreciationRuleOpen)
            {
                return " (SELECT DATEADD(month,DepreciationRule.totalmonth,purchasedate)  from DepreciationRule where assets.depreciationRule=DepreciationRule.depreciationRuleId ) ";
            }
            else
            {
                return " (SELECT DATEADD(year,durableyears,purchasedate)) ";
            }
        }

        private string RemainValueSql()
        {
            if (AppMember.DepreciationRuleOpen)
            {
                return " isnull((SELECT Assets.assetsValue*DepreciationRule.remainRate  from DepreciationRule where assets.depreciationRule=DepreciationRule.depreciationRuleId ),0) ";
            }
            else
            {
                return " (isnull(Assets.assetsValue*Assets.remainRate,0)) ";
            }
        }

        protected string AssetsSelectListSql(ApproveListCondition condition)
        {
            string sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
	                (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
	                (select assetsUsesName from AssetsUses where Assets.assetsUsesId=AssetsUses.assetsUsesId) assetsUsesId,
                   (select departmentName from AppDepartment C where D.companyId=C.departmentId ) companyId,
                    Assets.departmentId departmentId,
	                 D.departmentName departmentName,
	                Assets.storeSiteId storeSiteId,
                   Assets.assetsQty assetsQty,
                   (select storeSiteName from StoreSite where Assets.storeSiteId=StoreSite.storeSiteId) storeSiteName,
                    Assets.usePeople usePeople,
	                isnull((select userName from AppUser where Assets.usePeople=AppUser.userId),Assets.usePeople) usePeopleName,
                    Assets.keeper keeper,
	                isnull((select userName from AppUser where Assets.keeper=AppUser.userId),Assets.keeper) keeperName,
                    Assets.assetsValue assetsValue,
                    Convert(decimal(18,2), Assets.assetsNetValue)  assetsNetValue,
                    Assets.imgDefault imgDefault,
                    Assets.assetsBarcode assetsBarcode,
                     convert(nvarchar(100),Assets.purchaseDate,23) purchaseDate,
	                (select CodeTable.codeName from CodeTable where Assets.assetsState=CodeTable.codeNo and CodeTable.codeType='AssetsState' and CodeTable.languageVer='{0}' ) assetsState,
                     (select equityOwnerName from EquityOwner where Assets.equityOwnerId=EquityOwner.equityOwnerId) equityOwnerName,
                     (select projectManageName from ProjectManage where Assets.projectManageId=ProjectManage.projectManageId) projectManageName,
                      Assets.spec spec,
                      Assets.remark remark,
                     Assets.createTime createTime ,
                      Assets.updateTime updateTime 
                from Assets 
               left join AppDepartment D on Assets.departmentId=D.departmentId 
                where 1=1  ", AppMember.AppLanguage.ToString());
            return sql;
        }

        protected string BorrowAssetsSelectListSql(ApproveListCondition condition)
        {
            string sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
	                (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
	                (select assetsUsesName from AssetsUses where Assets.assetsUsesId=AssetsUses.assetsUsesId) assetsUsesId,
                   (select departmentName from AppDepartment C where D.companyId=C.departmentId ) companyId,
                    Assets.departmentId departmentId,
	                 D.departmentName departmentName,
	                Assets.storeSiteId storeSiteId,
                   (select storeSiteName from StoreSite where Assets.storeSiteId=StoreSite.storeSiteId) storeSiteName,
                    Assets.usePeople usePeople,
	                isnull((select userName from AppUser where Assets.usePeople=AppUser.userId),Assets.usePeople) usePeopleName,
                    Assets.keeper keeper,
	                isnull((select userName from AppUser where Assets.keeper=AppUser.userId),Assets.keeper ) keeperName,
	                (select CodeTable.codeName from CodeTable where Assets.assetsState=CodeTable.codeNo and CodeTable.codeType='AssetsState' and CodeTable.languageVer='{0}' ) assetsState,
                AssetsBorrowDetail.borrowPeople,
                (select userName from AppUser where AssetsBorrowDetail.borrowPeople=AppUser.userId) borrowPeopleName,
                AssetsBorrowDetail.borrowDepartmentId,
                (select departmentName from AppDepartment where AssetsBorrowDetail.borrowDepartmentId=AppDepartment.departmentId) borrowDepartmentName,
                AssetsBorrowDetail.borrowDate
                from Assets inner join AssetsBorrowDetail on Assets.assetsId=AssetsBorrowDetail.assetsId
               left join AppDepartment D on Assets.departmentId=D.departmentId 
                 where AssetsBorrowDetail.alreadyReturn<>'Y'  ", AppMember.AppLanguage.ToString());
            return sql;
        }

        protected string ApproveListSql(ApproveListCondition condition)
        {
            string lsql = "";
            if (DataConvert.ToString(condition.ListMode) != "")
            {
                if (DataConvert.ToString(condition.ListMode) == "approve")
                {
                    lsql = @",AppApprove
                     where AppApprove.tableName='Assets' and AppApprove.approveState='O'
                      and Assets.assetsId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(condition.ListMode) == "reapply")
                {
                    lsql = @" where Assets.createId=@approver and Assets.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                       (select CodeTable.codeName from CodeTable where Assets.approveState=CodeTable.codeNo and CodeTable.codeType='ApproveState' and CodeTable.languageVer='{0}' ) approveState,
                       (select userNo from AppUser where Assets.createId=AppUser.userId) createId,
                        Assets.createTime createTime ,
                        (select userNo from AppUser where Assets.updateId=AppUser.userId) updateId,
                        Assets.updateTime updateTime ,
                        Assets.updatePro updatePro
                from Assets {1} ", AppMember.AppLanguage.ToString(), lsql);
            return subViewSql;
        }

        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            model.ApproveTableName = "Assets";
            model.ApprovePkField = "assetsId";
            if (formMode != "new" && formMode != "new2")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("assetsId", primaryKey);
                string sql = @"select * from Assets where assetsId=@assetsId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.AssetsId = primaryKey;
                model.AssetsNo = DataConvert.ToString(dr["assetsNo"]);
                model.AssetsName = DataConvert.ToString(dr["assetsName"]);
                model.AssetsClassId = DataConvert.ToString(dr["assetsClassId"]);
                model.AssetsTypeId = DataConvert.ToString(dr["assetsTypeId"]);
                model.AssetsUsesId = DataConvert.ToString(dr["assetsUsesId"]);
                model.DepartmentId = DataConvert.ToString(dr["departmentId"]);
                model.StoreSiteId = DataConvert.ToString(dr["storeSiteId"]);
                model.DepreciationType = DataConvert.ToString(dr["depreciationType"]);
                model.DepreciationRule = DataConvert.ToString(dr["depreciationRule"]);
                model.EquityOwnerId = DataConvert.ToString(dr["equityOwnerId"]);
                model.PurchaseTypeId = DataConvert.ToString(dr["purchaseTypeId"]);
                model.SupplierId = DataConvert.ToString(dr["supplierId"]);
                model.UnitId = DataConvert.ToString(dr["unitId"]);
                model.AssetsBarcode = DataConvert.ToString(dr["assetsBarcode"]);
                model.AssetsValue = DataConvert.ToDouble(dr["assetsValue"]);
                model.DurableYears = DataConvert.ToIntNull(dr["durableYears"]);
                model.RemainRate = DataConvert.ToDoubleNull(dr["remainRate"]);
                model.PurchaseDate = DataConvert.ToDateTime(dr["purchaseDate"]);
                model.PurchaseDate2 = DataConvert.ToDateTimeOrNull(dr["purchaseDate2"]);
                model.AssetsQty = DataConvert.ToIntNull(dr["assetsQty"]);
                model.InvoiceNo = DataConvert.ToString(dr["invoiceNo"]);
                model.PurchaseNo = DataConvert.ToString(dr["purchaseNo"]);
                model.UsePeople = DataConvert.ToString(dr["usePeople"]);
                model.Keeper = DataConvert.ToString(dr["keeper"]);
                model.AssetsState = DataConvert.ToString(dr["assetsState"]);
                model.GuaranteeDays = DataConvert.ToIntNull(dr["guaranteeDays"]);
                model.MaintainDays = DataConvert.ToIntNull(dr["maintainDays"]);
                model.ProjectManageId = DataConvert.ToString(dr["projectManageId"]);
                model.Remark = DataConvert.ToString(dr["remark"]);
                model.CEANo = DataConvert.ToString(dr["CEANo"]);
                model.TagMaterial = DataConvert.ToString(dr["TagMaterial"]);
                model.SupplierName = DataConvert.ToString(dr["supplierName"]);
                model.Remark = DataConvert.ToString(dr["markMK"]);
                model.MarkMK = DataConvert.ToString(dr["mujuNo"]);
                model.ShengchanhuopinNo = DataConvert.ToString(dr["shengchanhuopinNo"]);
                model.Mujuxueshu = DataConvert.ToString(dr["mujuxueshu"]);
                model.Mujushouming = DataConvert.ToString(dr["mujushouming"]);
                model.Shejichannenng = DataConvert.ToString(dr["shejichannenng"]);
                model.Hadshejichannneng = DataConvert.ToString(dr["hadshejichannneng"]);
                model.ShiYongDiDian = DataConvert.ToString(dr["shiYongDiDian"]);
                model.PanDianHao = DataConvert.ToString(dr["panDianHao"]);
                model.Spec = DataConvert.ToString(dr["spec"]);
                model.AssetsPurchaseDetailId = DataConvert.ToString(dr["assetsPurchaseDetailId"]);
                model.AssetsPurchaseId = DataConvert.ToString(dr["assetsPurchaseId"]);
                if (DataConvert.ToString(dr["assetsState"]) == "X")
                    model.IsIdle = true;
                else
                    model.IsIdle = false;
                model.ImgFileDefault = DataConvert.ToString(dr["imgDefault"]);
                DataTable dt = GetAssetsImg(primaryKey);
                foreach (DataRow drImg in dt.Rows)
                {
                    model.ImgFileContainer += DataConvert.ToString(drImg["imgName"]) + ";";
                }

            }
        }

        public string GetDefaultAssetsId()
        {
            string sql = @"select top 1 * from Assets";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            if (dtGrid.Rows.Count > 0)
                return DataConvert.ToString(dtGrid.Rows[0]["assetsId"]);
            else
                return "";
        }

        public DataTable GetAssetsImg(string assetsId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsId", assetsId);
            string sql = @"select * from AppImg where refId=@assetsId and tableName='Assets'";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

        public DataSet GetAssetsCard(string assetsId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsId", assetsId);
            string sql = @"select * from Assets where assetsId=@assetsId ";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dtAssets.TableName = "Assets";

            sql = @"select * from AssetsScrapDetail where assetsId=@assetsId ";
            DataTable dtScrap = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dtScrap.TableName = "AssetsScrapDetail";

            sql = @"select * from AssetsTransferDetail where assetsId=@assetsId ";
            DataTable dtTransferJudge = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            DataTable dtTransfer;
            if (dtTransferJudge.Rows.Count > 0) //存在移动记录
            {
                //最开始使用该资产的记录 union all  移动使用记录
                sql = @"select
                    (select purchaseDate from Assets where AssetsTransferDetail.assetsId=Assets.assetsId)  transferDate,
                    (select userName from AppUser where AssetsTransferDetail.originalUsePeople=AppUser.userId) newUsePeople,
                    '' originalUsePeople,
                    (select departmentName from AppDepartment where AssetsTransferDetail.originalDepartmentId=AppDepartment.departmentId) newDepartmentId, 
                    (select tel from AppUser where AssetsTransferDetail.originalUsePeople=AppUser.userId) tel,
                    (select storeSiteName from StoreSite where AssetsTransferDetail.originalStoreSiteId=StoreSite.storeSiteId) newStoreSiteId,
                    '' quitDate,
                    (select remark from Assets where AssetsTransferDetail.assetsId=Assets.assetsId) remark
                    from AssetsTransferDetail
                    where assetsId=@assetsId
                    and createTime=(select min(createTime)from AssetsTransferDetail where assetsId='T15060700276')
                    union all
                    select transferDate,
                    (select userName from AppUser where AssetsTransferDetail.newUsePeople=AppUser.userId) newUsePeople,
                    (select userName from AppUser where AssetsTransferDetail.originalUsePeople=AppUser.userId) originalUsePeople,
                    (select departmentName from AppDepartment where AssetsTransferDetail.newDepartmentId=AppDepartment.departmentId) newDepartmentId, 
                    (select tel from AppUser where AssetsTransferDetail.newUsePeople=AppUser.userId) tel,
                    (select storeSiteName from StoreSite where AssetsTransferDetail.newStoreSiteId=StoreSite.storeSiteId) newStoreSiteId,
                    '' quitDate,
                    remark
                    from AssetsTransferDetail
                    where assetsId=@assetsId";
                dtTransfer = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            }
            else
            {
                //无移动，直接用assets的使用人记录
                sql = @"select purchaseDate transferDate,
                    (select userName from AppUser where Assets.usePeople=AppUser.userId) newUsePeople,
                    '' originalUsePeople,
                    (select departmentName from AppDepartment where Assets.departmentId=AppDepartment.departmentId) newDepartmentId,
                    (select tel from AppUser where Assets.usePeople=AppUser.userId) tel,
                    (select storeSiteName from StoreSite where Assets.storeSiteId=StoreSite.storeSiteId)newStoreSiteId,
                    '' quitDate,
                    remark 
                    from Assets
                    where assetsId=@assetsId";
                dtTransfer = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            }
            dtTransfer.TableName = "AssetsTransfer";
            DataSet ds = new DataSet();
            ds.Tables.Add(dtAssets.Copy());
            ds.Tables.Add(dtScrap.Copy());
            ds.Tables.Add(dtTransfer.Copy());
            return ds;
        }


        public string GetAssetsBarcode(string assetsClassNo)
        {
            int prelen = assetsClassNo.Length;
            string indexformat = "";
            for (int i = 0; i < 11 - prelen; i++)
            {
                indexformat += "0";
            }

            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsClassNo", assetsClassNo);
            string sql = @"select max(assetsBarcode) assetsBarcode from Assets where assetsBarcode like ''+@assetsClassNo+'%'";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            if (dtGrid.Rows.Count > 0)
            {
                string bindex = DataConvert.ToString(dtGrid.Rows[0]["assetsBarcode"]).Replace(assetsClassNo, "");
                int index = DataConvert.ToInt32(bindex) + 1;
                return assetsClassNo + index.ToString(indexformat);
            }
            else
            {
                int index = 1;
                return assetsClassNo + index.ToString(indexformat);
            }
        }

        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from Assets where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "Assets";
            DataRow dr = dt.NewRow();
            string assetsId = IdGenerator.GetMaxId(dt.TableName);
            EntryModel myModel = model as EntryModel;
            UpdateUser(myModel, sysUser, viewTitle);
            SetDataRow(myModel, dr);
            dr["assetsValue"] = DataConvert.ToDBObject(myModel.AssetsValue);
            dr["assetsNetValue"] = DataConvert.ToDBObject(myModel.AssetsValue);
            dr["remainMonth"] = DataConvert.ToDBObject(myModel.DurableYears * 12);
            AddImg(DataConvert.ToString(myModel.ImgFileContainer), assetsId, sysUser, viewTitle);
            int retApprove = InitFirstApproveTask("Assets", "assetsId", assetsId, viewTitle, "", sysUser.UserId);
            if (myModel.IsIdle)
            {
                dr["assetsState"] = "X";
            }
            else
            {
                if (retApprove != 0)
                {
                    dr["assetsState"] = "O";
                    dr["approveState"] = "O";
                }
                else
                    dr["assetsState"] = "A";
            }
            string depreciationRule = DataConvert.ToString(myModel.DepreciationRule);
            dr["depreciationRule"] = depreciationRule;
            if (depreciationRule != "")
            {
                Dictionary<string, object> paras1 = new Dictionary<string, object>();
                paras1.Add("depreciationRuleId", depreciationRule);
                string sql1 = @"select * from DepreciationRule  where depreciationRuleId=@depreciationRuleId ";
                DataTable dt1 = AppMember.DbHelper.GetDataSet(sql1, paras1).Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    int totalMonth = DataConvert.ToInt32(dt1.Rows[0]["totalMonth"]);
                    dr["remainMonth"] = totalMonth;
                }
            }
            if (myModel.PurchaseDate2 != null)
            {
                dr["purchaseDate2"] = DataConvert.ToDBObject(myModel.PurchaseDate2);
            }
            dr["assetsId"] = assetsId;
            dr["checking"] = "N";
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            if (AppMember.LaunchDepreciation == "true")
                InitDepreciationTrack(sysUser, viewTitle);
            AppLog.WriteLog(sysUser.UserName, LogType.Info, "Assets", string.Format(AppMember.AppText["LogAssetsAdd"], myModel.AssetsNo, myModel.AssetsName, myModel.AssetsBarcode));
            return 1;
        }

        private int UpdateUser(EntryModel model, UserInfo sysUser, string viewTitle)
        {
            if (AppMember.UsePeopleControlLevel == UsePeopleControlLevel.Low)
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("UsePeople", DataConvert.ToString(model.UsePeople));
                paras.Add("Keeper", DataConvert.ToString(model.Keeper));
                string sql = @"select * from AppUser where userName in (@UsePeople,@Keeper) ";
                DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                dt.TableName = "AppUser";

                if (DataConvert.ToString(model.UsePeople) != "" && dt.Select("userName='" + model.UsePeople + "'").Length < 1)
                {
                    DataRow dr = dt.NewRow();
                    string userId = IdGenerator.GetMaxId(dt.TableName);
                    dr["userId"] = userId;
                    dr["userName"] = model.UsePeople;
                    dr["isSysUser"] = "N";
                    dt.Rows.Add(dr);
                }
                if (DataConvert.ToString(model.Keeper) != "" && dt.Select("userName='" + model.Keeper + "'").Length < 1 && DataConvert.ToString(model.Keeper) != DataConvert.ToString(model.UsePeople))
                {
                    DataRow dr = dt.NewRow();
                    string userId = IdGenerator.GetMaxId(dt.TableName);
                    dr["userId"] = userId;
                    dr["userName"] = model.Keeper;
                    dr["isSysUser"] = "N";
                    dt.Rows.Add(dr);
                }
                Create5Field(dt, sysUser.UserId, viewTitle);
                DbUpdate.Update(dt);
                return 1;
            }
            else
                return 0;
        }

        protected virtual int InitDepreciationTrack(UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from DepreciationTrack ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "DepreciationTrack";
            if (dt.Rows.Count < 1)
            {
                DataRow dr = dt.NewRow();
                dr["depreciationTrackId"] = IdGenerator.GetMaxId(dt.TableName);
                DateTime dtTime = IdGenerator.GetServerDate();
                FiscalPeriodRepository rep = new FiscalPeriodRepository();
                DataTable dtfp = rep.GetFiscalPeriod(dtTime);
                if (dtfp.Rows.Count < 1)
                    throw new Exception(AppMember.AppText["FirstPeriodAndYear"]);
                string fiscalYearId = DataConvert.ToString(dtfp.Rows[0]["fiscalYearId"]);
                string fiscalPeriodId = DataConvert.ToString(dtfp.Rows[0]["fiscalPeriodId"]);
                dr["fiscalYearId"] = fiscalYearId;
                dr["fiscalPeriodId"] = fiscalPeriodId;

                sql = @"select FiscalYear.fiscalYearName,FiscalPeriod.fiscalPeriodName ,FiscalPeriod.fiscalYearId, FiscalPeriod.fiscalPeriodId 
                    from FiscalPeriod,FiscalYear
                     where FiscalYear.fiscalYearId=FiscalPeriod.fiscalYearId
                    order by  FiscalYear.fiscalYearName,FiscalPeriod.fiscalPeriodName";
                DataTable dtFiscalPeriod = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
                string fiscalYearIdNext = null;
                string fiscalPeriodIdNext = null;
                for (int i = 0; i < dtFiscalPeriod.Rows.Count; i++)
                {
                    if (DataConvert.ToString(dtFiscalPeriod.Rows[i]["fiscalYearId"]) == fiscalYearId
                        && DataConvert.ToString(dtFiscalPeriod.Rows[i]["fiscalPeriodId"]) == fiscalPeriodId)
                    {
                        fiscalYearIdNext = DataConvert.ToString(dtFiscalPeriod.Rows[i + 1]["fiscalYearId"]);
                        fiscalPeriodIdNext = DataConvert.ToString(dtFiscalPeriod.Rows[i + 1]["fiscalPeriodId"]);
                        break;
                    }
                }
                dr["fiscalYearIdNext"] = fiscalYearIdNext;
                dr["fiscalPeriodIdNext"] = fiscalPeriodIdNext;
                dt.Rows.Add(dr);
                Create5Field(dt, sysUser.UserId, viewTitle);
                DbUpdate.Update(dt);
            }
            return 1;
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsId", pkValue);
            string sql = @"select * from Assets where assetsId=@assetsId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "Assets";
            string assetsId = DataConvert.ToString(dt.Rows[0]["assetsId"]);
            EntryModel myModel = model as EntryModel;
            UpdateUser(myModel, sysUser, viewTitle);
            if (DataConvert.ToString(dt.Rows[0]["depreciationRule"]) == "")
            {
                string depreciationRule = DataConvert.ToString(myModel.DepreciationRule);
                dt.Rows[0]["depreciationRule"] = depreciationRule;
                if (depreciationRule != "")
                {
                    Dictionary<string, object> paras1 = new Dictionary<string, object>();
                    paras1.Add("depreciationRuleId", depreciationRule);
                    string sql1 = @"select * from DepreciationRule  where depreciationRuleId=@depreciationRuleId ";
                    DataTable dt1 = AppMember.DbHelper.GetDataSet(sql1, paras1).Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        int totalMonth = DataConvert.ToInt32(dt1.Rows[0]["totalMonth"]);
                        dt.Rows[0]["remainMonth"] = totalMonth;
                    }
                }
            }
            SetDataRow(myModel, dt.Rows[0]);

            if (myModel.AssetsValue > 0)
            {
                dt.Rows[0]["assetsValue"] = DataConvert.ToDBObject(myModel.AssetsValue);
                if (dt.Rows[0]["assetsNetValue"] == DBNull.Value)
                    dt.Rows[0]["assetsNetValue"] = dt.Rows[0]["assetsValue"];
            }

            if (myModel.PurchaseDate2!=null)
            {
                dt.Rows[0]["purchaseDate2"] = DataConvert.ToDBObject(myModel.PurchaseDate2);
            }

            string state = DataConvert.ToString(dt.Rows[0]["assetsState"]);
            if (myModel.IsIdle && (state == "O" || state == "A" || state == "X"))
            {
                dt.Rows[0]["assetsState"] = "X";
            }
            else if (!myModel.IsIdle && (state == "O" || state == "A" || state == "X"))
            {
                dt.Rows[0]["assetsState"] = "A";
            }
            else
            {
                AppLog.WriteLog(sysUser.UserName, LogType.Info, "Assets", "only assets state is O,A,X can modified idle.");
            }
            DeleteImg(sysUser, assetsId);
            AddImg(DataConvert.ToString(myModel.ImgFileContainer), assetsId, sysUser, viewTitle);
            if (formMode == "reapply")
                dt.Rows[0]["approveState"] = "O";
            Update5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            if (formMode == "reapply")
                InitFirstApproveTask("Assets", "assetsId", assetsId, viewTitle, formMode, sysUser.UserId);
            AppLog.WriteLog(sysUser.UserName, LogType.Info, "Assets", string.Format(AppMember.AppText["LogAssetsModified"], myModel.AssetsNo, myModel.AssetsName, myModel.AssetsBarcode));
            return 1;
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsId", pkValue);
            string sql = @"select * from Assets where assetsId=@assetsId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "Assets";
            DeleteImg(sysUser, pkValue);
            string assetsNo = DataConvert.ToString(dt.Rows[0]["assetsNo"]);
            string assetsName = DataConvert.ToString(dt.Rows[0]["assetsName"]);
            string assetsBarcode = DataConvert.ToString(dt.Rows[0]["assetsBarcode"]);
            dt.Rows[0].Delete();
            DbUpdate.Update(dt);
            DeleteApproveData("Assets", pkValue, sysUser.UserId);
            AppLog.WriteLog(sysUser.UserName, LogType.Info, "Assets", string.Format(AppMember.AppText["LogAssetsDelete"], assetsNo, assetsName, assetsBarcode));
            return 1;
        }

        public int BatchDelete(UserInfo sysUser, string ids, string viewTitle, string isCascadeDelete)
        {
            if (ids.EndsWith(","))
            {
                ids = ids.Substring(0, ids.Length - 1);
            }
            string sql = @"select * from Assets where assetsId in (" + ids + ")";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "Assets";
            DeleteBatchImg(sysUser, ids);
            DeleteBatchApproveData("Assets", ids, sysUser.UserId);
            if (isCascadeDelete == "true")
            {
                CascadeDeleteAssets(ids);
            }
            DataTable dtCopy = dt.Copy();
            foreach (DataRow dr in dt.Rows)
            {
                dr.Delete();
            }
            DbUpdate.Update(dt);
            foreach (DataRow dr in dtCopy.Rows)
            {
                AppLog.WriteLog(sysUser.UserName, LogType.Info, "Assets", string.Format(AppMember.AppText["LogAssetsDelete"], DataConvert.ToString(dr["assetsNo"]),
                DataConvert.ToString(dr["assetsName"]), DataConvert.ToString(dr["assetsBarcode"])));
            }
            return 1;
        }

        protected int CascadeDeleteAssets(string ids)
        {
            string sql = @"select * from AssetsBorrowDetail where assetsId in (" + ids + ")";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsBorrowDetail";
            foreach (DataRow dr in dt.Rows)
                dr.Delete();
            DbUpdate.Update(dt);

            sql = @"select * from AssetsCheckDetail where assetsId in (" + ids + ")";
            dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsCheckDetail";
            foreach (DataRow dr in dt.Rows)
                dr.Delete();
            DbUpdate.Update(dt);

            sql = @"select * from AssetsDepreciation where assetsId in (" + ids + ")";
            dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsDepreciation";
            foreach (DataRow dr in dt.Rows)
                dr.Delete();
            DbUpdate.Update(dt);

            sql = @"select * from AssetsMaintainDetail where assetsId in (" + ids + ")";
            dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsMaintainDetail";
            foreach (DataRow dr in dt.Rows)
                dr.Delete();
            DbUpdate.Update(dt);

            sql = @"select * from AssetsMergeDetail where originalAssetsId in (" + ids + ") or newAssetsId in (" + ids + ")";
            dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsMergeDetail";
            foreach (DataRow dr in dt.Rows)
                dr.Delete();
            DbUpdate.Update(dt);

            sql = @"select * from AssetsReturnDetail where assetsId in (" + ids + ")";
            dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsReturnDetail";
            foreach (DataRow dr in dt.Rows)
                dr.Delete();
            DbUpdate.Update(dt);

            sql = @"select * from AssetsScrapDetail where assetsId in (" + ids + ")";
            dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsScrapDetail";
            foreach (DataRow dr in dt.Rows)
                dr.Delete();
            DbUpdate.Update(dt);

            sql = @"select * from AssetsSplitDetail where originalAssetsId in (" + ids + ") or newAssetsId in (" + ids + ")";
            dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsSplitDetail";
            foreach (DataRow dr in dt.Rows)
                dr.Delete();
            DbUpdate.Update(dt);

            sql = @"select * from AssetsTransferDetail where assetsId in (" + ids + ")";
            dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsTransferDetail";
            foreach (DataRow dr in dt.Rows)
                dr.Delete();
            DbUpdate.Update(dt);

            return 1;
        }

        protected void SetDataRow(EntryModel model, DataRow dr)
        {
            dr["assetsNo"] = model.AssetsNo;
            dr["assetsName"] = model.AssetsName;
            dr["assetsClassId"] = model.AssetsClassId;
            dr["assetsTypeId"] = model.AssetsTypeId;
            dr["assetsUsesId"] = model.AssetsUsesId;
            dr["departmentId"] = model.DepartmentId;
            dr["storeSiteId"] = model.StoreSiteId;
            dr["depreciationType"] = model.DepreciationType;
            dr["equityOwnerId"] = model.EquityOwnerId;
            dr["purchaseTypeId"] = model.PurchaseTypeId;
            dr["supplierId"] = model.SupplierId;
            dr["unitId"] = model.UnitId;
            dr["assetsBarcode"] = model.AssetsBarcode;

            dr["durableYears"] = DataConvert.ToDBObject(model.DurableYears);
            dr["remainRate"] = DataConvert.ToDBObject(model.RemainRate);
            dr["purchaseDate"] = DataConvert.ToDBObject(model.PurchaseDate);

            dr["assetsQty"] = DataConvert.ToDBObject(model.AssetsQty);
            dr["addDate"] = DataConvert.ToDBObject(model.PurchaseDate);
            dr["invoiceNo"] = model.InvoiceNo;
            dr["purchaseNo"] = model.PurchaseNo;
            dr["usePeople"] = model.UsePeople;
            dr["keeper"] = model.Keeper;
            dr["guaranteeDays"] = DataConvert.ToDBObject(model.GuaranteeDays);
            dr["maintainDays"] = DataConvert.ToDBObject(model.MaintainDays);
            dr["projectManageId"] = model.ProjectManageId;
            dr["remark"] = model.Remark;
            dr["CEANo"] = model.CEANo;
            dr["TagMaterial"] = model.TagMaterial;
            dr["supplierName"] = model.SupplierName;
            dr["markMK"] = model.MarkMK;
            dr["mujuNo"] = model.MujuNo;
            dr["shengchanhuopinNo"] = model.ShengchanhuopinNo;
            dr["mujuxueshu"] = model.Mujuxueshu;
            dr["mujushouming"] = model.Mujushouming;
            dr["shejichannenng"] = model.Shejichannenng;
            dr["hadshejichannneng"] = model.Hadshejichannneng;
            dr["shiYongDiDian"] = model.ShiYongDiDian;
            dr["panDianHao"] = model.PanDianHao;
            dr["spec"] = model.Spec;
            dr["imgDefault"] = model.ImgFileDefault;
            dr["assetsPurchaseDetailId"] = model.AssetsPurchaseDetailId;
            dr["assetsPurchaseId"] = model.AssetsPurchaseId;

        }


        protected int AddImg(string imgContainer, string assetsId, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AppImg where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
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
                    dt.Rows.Add(dr);
                    Create5Field(dt, sysUser.UserId, viewTitle);
                }
            }
            return DbUpdate.Update(dt);
        }

        protected int DeleteImg(UserInfo sysUser, string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsId", pkValue);
            string sql = @"select * from AppImg where refId=@assetsId and tableName='Assets'";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AppImg";
            foreach (DataRow dr in dt.Rows)
            {
                dr.Delete();
            }
            return DbUpdate.Update(dt);
        }

        protected int DeleteBatchImg(UserInfo sysUser, string pkValue)
        {
            string sql = @"select * from AppImg where refId in (" + pkValue + ") and tableName='Assets'";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "AppImg";
            foreach (DataRow dr in dt.Rows)
            {
                dr.Delete();
            }
            return DbUpdate.Update(dt);
        }

        public override int DealEndApprove(string approvePkValue, UserInfo sysUser, string viewTitle)
        {
            UpdateAssetsState(approvePkValue, "A", sysUser.UserId, viewTitle);
            return 1;
        }

        public string GetStartDepreciationValue(string assetsId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsId", assetsId);
            string sql = @"select * from dbo.AssetsDepreciation where assetsId=@assetsId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            if (dt.Rows.Count > 0)
                return "Y";
            else
                return "N";
        }


    }
}

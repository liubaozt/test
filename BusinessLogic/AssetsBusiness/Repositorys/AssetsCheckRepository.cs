using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.AssetsBusiness.Models.AssetsCheck;
using BaseCommon.Models;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsCheck
    {
        public string AssetsId { get; set; }
        public string Remark { get; set; }
        public string ApproveState { get; set; }
        public string DepartmentId { get; set; }
        public string StoreSiteId { get; set; }
        public string CheckDate { get; set; }
        public string ActualDepartmentId { get; set; }
        public string ActualStoreSiteId { get; set; }
        public string ActualCheckDate { get; set; }
        public string IsFinished { get; set; }
        public string CheckResultId { get; set; }
        public string CreateId { get; set; }
        public string CreateTime { get; set; }
    }

    public class AssetsCheckRepository : ApproveMasterRepository
    {

        public AssetsCheckRepository()
        {
            DefaulteGridSortField = "AssetsCheckNo";
            MasterTable = "AssetsCheck";
        }


        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            ApproveListCondition acondition = condition as ApproveListCondition;
            if (DataConvert.ToString(acondition.ListMode) != "")
                wcd.DBPara.Add("approver", acondition.Approver);
            ListModel model = JsonHelper.Deserialize<ListModel>(acondition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.AssetsCheckNo) != "")
            {
                wcd.Sql += @" and AssetsCheck.assetsCheckNo like '%'+@assetsCheckNo+'%'";
                wcd.DBPara.Add("assetsCheckNo", model.AssetsCheckNo);
            }
            if (DataConvert.ToString(model.AssetsCheckName) != "")
            {
                wcd.Sql += @" and AssetsCheck.assetsCheckName like '%'+@assetsCheckName+'%'";
                wcd.DBPara.Add("assetsCheckName", model.AssetsCheckName);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            ApproveListCondition acondition = condition as ApproveListCondition;
            int rowSize = acondition.PageIndex * acondition.PageRowNum; //子查询返回行数的尺寸
            string lsql = " where 1=1";
            if (DataConvert.ToString(acondition.ListMode) != "")
            {
                if (DataConvert.ToString(acondition.ListMode) == "approve")
                {
                    lsql = @",AppApprove
                     where AppApprove.tableName='AssetsCheck' and AppApprove.approveState='O'
                      and AssetsCheck.AssetsCheckId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(acondition.ListMode) == "reapply")
                {
                    lsql = @" where AssetsCheck.createId=@approver and AssetsCheck.approveState='R' ";
                }
                else if (DataConvert.ToString(acondition.ListMode) == "actual")
                {
                    lsql = @" where AssetsCheck.createId=@approver and (AssetsCheck.approveState='E' or AssetsCheck.approveState is null) ";
                }
            }
            string subViewSql = string.Format(@"select top {0} AssetsCheck.AssetsCheckId AssetsCheckId,
                        AssetsCheck.AssetsCheckNo AssetsCheckNo,
                        AssetsCheck.AssetsCheckName AssetsCheckName,
                       (select top 1 codename from CodeTable where codetype='ApproveState' and  AssetsCheck.approveState=codeno) approveState,       
                        (select userName from AppUser where AssetsCheck.createId=AppUser.userId) createId,
                        AssetsCheck.createTime createTime ,
                        (select userName from AppUser where AssetsCheck.updateId=AppUser.userId) updateId,
                        AssetsCheck.updateTime updateTime ,
                        AssetsCheck.updatePro updatePro
                from AssetsCheck  {2}  {3}", DataConvert.ToString(rowSize), AppMember.AppLanguage.ToString(), lsql, ListWhereSql(acondition).Sql);
            return subViewSql;
        }

        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            model.ApproveTableName = "AssetsCheck";
            model.ApprovePkField = "assetsCheckId";
            if (formMode != "new" && formMode != "new2")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("AssetsCheckId", primaryKey);
                string sql = @"select AssetsCheck.assetsCheckNo,
                        AssetsCheck.assetsCheckName,
                        AssetsCheck.checkPeople,
                        AssetsCheckDetail.checkDate
                        from AssetsCheck,AssetsCheckDetail 
                          where AssetsCheck.assetsCheckId=AssetsCheckDetail.assetsCheckId 
                          and AssetsCheck.assetsCheckId=@AssetsCheckId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.AssetsCheckId = primaryKey;
                model.AssetsCheckNo = DataConvert.ToString(dr["assetsCheckNo"]);
                model.AssetsCheckName = DataConvert.ToString(dr["assetsCheckName"]);
                model.CheckPeople = DataConvert.ToString(dr["checkPeople"]);
                model.CheckDate = DataConvert.ToDateTime(dr["checkDate"]);
            }
        }

        public virtual int GetEntryGridCount(Dictionary<string, object> paras, string formMode, string primaryKey, string formVar)
        {
            if (formMode == "new" || formMode == "new2")
            {
                string sql = string.Format(@"select *  from Assets    where 1=1  ", AppMember.AppLanguage.ToString());
                WhereConditon wcd = EntryGridWhereSql(formVar);
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql + wcd.Sql, wcd.DBPara).Tables[0];
                return dtGrid.Rows.Count;
            }
            else
            {
                if (!paras.ContainsKey("AssetsCheckId"))
                    paras.Add("AssetsCheckId", primaryKey);
                string sql = string.Format(@"select  AssetsCheckDetail.*   from AssetsCheckDetail 
                inner join Assets on AssetsCheckDetail.assetsId=Assets.assetsId 
               left join AppDepartment D on AssetsCheckDetail.departmentId=D.departmentId 
                where AssetsCheckDetail.AssetsCheckId=@AssetsCheckId  ");
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                return dtGrid.Rows.Count;
            }
        }

        public override DataTable GetEntryGridDataTable(Dictionary<string, object> paras, string formMode, string primaryKey, string formVar)
        {
            int pageIndex = DataConvert.ToInt32(paras["pageIndex"]);
            int pageRowNum = DataConvert.ToInt32(paras["pageRowNum"]);
            int start = 1 + pageRowNum * (pageIndex - 1);
            int end = pageRowNum * pageIndex;
            if (formMode == "new" || formMode == "new2")
            {
                WhereConditon wcd = EntryGridWhereSql(formVar);
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
	                isnull((select userName from AppUser where Assets.keeper=AppUser.userId),Assets.keeper) keeperName,
                    Assets.assetsValue assetsValue,
                    Assets.assetsNetValue assetsNetValue,
                    Assets.imgDefault imgDefault,
                    Assets.assetsBarcode assetsBarcode,
                    convert(nvarchar(100), Assets.purchaseDate,23) purchaseDate,
	                (select CodeTable.codeName from CodeTable where Assets.assetsState=CodeTable.codeNo and CodeTable.codeType='AssetsState' and CodeTable.languageVer='{0}' ) assetsState,
                     (select equityOwnerName from EquityOwner where Assets.equityOwnerId=EquityOwner.equityOwnerId) equityOwnerName,
                     (select projectManageName from ProjectManage where Assets.projectManageId=ProjectManage.projectManageId) projectManageName,
                      Assets.spec spec,
                      Assets.remark remark,
                      '' ApproveState,
                        Assets.CreateId CreateId,
                        Assets.CreateTime CreateTime
                from  ( select * from (select *,Row_Number() OVER ( ORDER BY assetsno ) rnum from Assets where 1=1  {1}) Assets where rnum between {2} and {3}  ) Assets
               left join AppDepartment D on Assets.departmentId=D.departmentId 
                where 1=1  ", AppMember.AppLanguage.ToString(), wcd.Sql, start, end);

                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, wcd.DBPara).Tables[0];
                return dtGrid;
            }
            else
            {
                if (!paras.ContainsKey("AssetsCheckId"))
                    paras.Add("AssetsCheckId", primaryKey);
                string sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
               (select departmentName from AppDepartment C where D.companyId=C.departmentId ) companyId,
                 AssetsCheckDetail.departmentId departmentId,
                D.departmentName  departmentName, 
                 AssetsCheckDetail.storeSiteId storeSiteId,
                 (select storeSiteName from StoreSite where StoreSite.storeSiteId=AssetsCheckDetail.storeSiteId) storeSiteName,  
                AssetsCheckDetail.actualDepartmentId actualDepartmentId, 
                AssetsCheckDetail.actualDepartmentId actualDepartmentName,
               (select departmentName from AppDepartment where AppDepartment.departmentId=AssetsCheckDetail.actualDepartmentId) actualDepartmentText, 
                '' ActualDepartmentBtn,
                AssetsCheckDetail.actualStoreSiteId actualStoreSiteId,   
                AssetsCheckDetail.actualStoreSiteId actualStoreSiteName,    
               (select storeSiteName from StoreSite where StoreSite.storeSiteId=AssetsCheckDetail.actualStoreSiteId) actualStoreSiteText,               
                '' ActualStoreSiteBtn ,               
                Assets.assetsBarcode AssetsBarcode,
                  convert(nvarchar(100),AssetsCheckDetail.actualCheckDate,23) ActualCheckDate,
                AssetsCheckDetail.checkResult CheckResultId,   
                AssetsCheckDetail.checkResult CheckResultName,    
               (select CodeTable.codeName from CodeTable where AssetsCheckDetail.checkResult=CodeTable.codeNo and CodeTable.codeType='CheckResult' and CodeTable.languageVer='{0}' ) CheckResultText,  
                AssetsCheckDetail.isFinished isFinished,
                (select CodeTable.codeName from CodeTable where AssetsCheckDetail.isFinished=CodeTable.codeNo and CodeTable.codeType='BoolVal' and CodeTable.languageVer='{0}' ) isFinishedText,
                AssetsCheckDetail.approveState ApproveState,
                AssetsCheckDetail.createId CreateId,
                AssetsCheckDetail.createTime CreateTime
                from  ( select * from (select *,Row_Number() OVER ( ORDER BY assetsId ) rnum from AssetsCheckDetail where AssetsCheckDetail.AssetsCheckId=@AssetsCheckId ) AssetsCheckDetail where rnum between {1} and {2}    ) AssetsCheckDetail
                left join Assets on AssetsCheckDetail.assetsId=Assets.assetsId 
                left join AppDepartment D on AssetsCheckDetail.departmentId=D.departmentId 
                where 1=1  ", AppMember.AppLanguage.ToString(), start, end);
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                return dtGrid;
            }

        }

        public virtual DataTable GetEntryGridSource(EntryModel model)
        {

            WhereConditon wcd = EntryGridWhereSql(model);
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
	                isnull((select userName from AppUser where Assets.keeper=AppUser.userId),Assets.keeper) keeperName,
                    Assets.assetsValue assetsValue,
                    Assets.assetsNetValue assetsNetValue,
                    Assets.imgDefault imgDefault,
                    Assets.assetsBarcode assetsBarcode,
                    Assets.purchaseDate purchaseDate,
	                (select CodeTable.codeName from CodeTable where Assets.assetsState=CodeTable.codeNo and CodeTable.codeType='AssetsState' and CodeTable.languageVer='{0}' ) assetsState,
                     (select equityOwnerName from EquityOwner where Assets.equityOwnerId=EquityOwner.equityOwnerId) equityOwnerName,
                     (select projectManageName from ProjectManage where Assets.projectManageId=ProjectManage.projectManageId) projectManageName,
                      Assets.spec spec,
                      Assets.remark remark,
                      '' ApproveState,
                        Assets.CreateId CreateId,
                        Assets.CreateTime CreateTime
                from  Assets
               left join AppDepartment D on Assets.departmentId=D.departmentId 
                where 1=1 {1} ", AppMember.AppLanguage.ToString(), wcd.Sql);

            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, wcd.DBPara).Tables[0];
            return dtGrid;


        }


        protected WhereConditon EntryGridWhereSql(string formVar)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            if (formVar == null)
                wcd.Sql += " and 1<>1 ";
            EntryQueryModel model = JsonHelper.Deserialize<EntryQueryModel>(formVar);
            if (model == null) return wcd;

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

            return wcd;
        }

        protected WhereConditon EntryGridWhereSql(EntryModel model)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
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

            return wcd;
        }

        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsCheck where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsCheck";
            DataRow dr = dt.NewRow();
            string assetsCheckId = IdGenerator.GetMaxId(dt.TableName);
            int retApprove = InitFirstApproveTask("AssetsCheck", "AssetsCheckId", assetsCheckId, viewTitle, "", sysUser.UserId);
            EntryModel myModel = model as EntryModel;
            dr["assetsCheckNo"] = myModel.AssetsCheckNo;
            dr["assetsCheckName"] = myModel.AssetsCheckName;
            dr["checkPeople"] = myModel.CheckPeople;
            string updateType = "Add";
            if (retApprove != 0)
            {
                dr["approveState"] = "O";
                updateType = "ApproveAdd";
            }
            dr["AssetsCheckId"] = assetsCheckId;
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);

            DataTable gridData = GetEntryGridSource(myModel);
            //List<AssetsCheck> gridData = JsonHelper.JSONStringToList<AssetsCheck>(DataConvert.ToString(myModel.EntryGridString));
            //if (gridData.Count < 1)
            //    throw new Exception(AppMember.AppText["NeedMoreThanOneAssets"]);
            if (gridData.Rows.Count < 1)
                throw new Exception(AppMember.AppText["NeedMoreThanOneAssets"]);
            foreach (DataRow assetsCheck in gridData.Rows)
            {
                AddDetail(assetsCheck, assetsCheckId, sysUser, viewTitle, updateType,DataConvert.ToString(myModel.CheckDate));
                if (retApprove != 0)
                {
                    UpdateChecking(DataConvert.ToString(assetsCheck["AssetsId"]), "I", sysUser.UserId, viewTitle);
                }
                else
                {
                    UpdateChecking(DataConvert.ToString(assetsCheck["AssetsId"]), "Y", sysUser.UserId, viewTitle);
                }
            }

            return 1;
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsCheckId", pkValue);
            string sql = @"select * from AssetsCheck where AssetsCheckId=@AssetsCheckId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsCheck";
            string AssetsCheckId = DataConvert.ToString(dt.Rows[0]["AssetsCheckId"]);
            EntryModel myModel = model as EntryModel;
            dt.Rows[0]["assetsCheckNo"] = myModel.AssetsCheckNo;
            dt.Rows[0]["assetsCheckName"] = myModel.AssetsCheckName;
            dt.Rows[0]["checkPeople"] = myModel.CheckPeople;
            string updateType = "Modified";
            if (formMode == "reapply")
            {
                dt.Rows[0]["approveState"] = "O";
                updateType = "Reapply";
            }
            Update5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            if (formMode == "rename")
                return 1;

            //DeleteDetail(pkValue, sysUser, viewTitle);
            List<AssetsCheck> gridData = JsonHelper.JSONStringToList<AssetsCheck>(DataConvert.ToString(myModel.EntryGridString));
            if (gridData.Count < 1)
                throw new Exception(AppMember.AppText["NeedMoreThanOneAssets"]);
            foreach (AssetsCheck AssetsCheck in gridData)
            {
                AssetsCheck.CheckDate = DataConvert.ToString(myModel.CheckDate);
                UpdateDetail(AssetsCheck, pkValue, sysUser, viewTitle, updateType);
                if (formMode == "reapply")
                {
                    UpdateChecking(AssetsCheck.AssetsId, "I", sysUser.UserId, viewTitle,AssetsCheck.ActualStoreSiteId);
                }
                if (formMode == "actual")
                {
                    UpdateChecking(AssetsCheck.AssetsId, "N", sysUser.UserId, viewTitle,AssetsCheck.ActualStoreSiteId);
                    if (AssetsCheck.CheckResultId == "L")
                        UpdateAssetsState(AssetsCheck.AssetsId, "L", sysUser.UserId, viewTitle);
                }
            }
            if (formMode == "reapply")
                InitFirstApproveTask("AssetsCheck", "AssetsCheckId", AssetsCheckId, viewTitle, formMode, sysUser.UserId);

            return 1;
        }

 

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsCheckId", pkValue);
            string sql = @"select * from AssetsCheck where AssetsCheckId=@AssetsCheckId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsCheck";
            dt.Rows[0].Delete();
            DeleteDetail(pkValue, sysUser, viewTitle, true);
            DeleteApproveData("AssetsCheck", pkValue, sysUser.UserId);
            DbUpdate.Update(dt);

            return 1;
        }

        protected int AddDetail(DataRow drCheck, string AssetsCheckId, UserInfo sysUser, string viewTitle, string updateType, string checkDate)
        {
            string sql = @"select * from AssetsCheckDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsCheckDetail";
            DataRow dr = dt.NewRow();
            dr["AssetsCheckId"] = AssetsCheckId;
            dr["assetsId"] = DataConvert.ToString(drCheck["AssetsId"]);
            dr["remark"] = DataConvert.ToString(drCheck["Remark"]);
            dr["departmentId"] = DataConvert.ToString(drCheck["DepartmentId"]);
            dr["storeSiteId"] = DataConvert.ToString(drCheck["StoreSiteId"]);
            //dr["actualDepartmentId"] = DataConvert.ToString( drCheck["ActualDepartmentId"]) ;
            //dr["actualStoreSiteId"] = DataConvert.ToString( drCheck["ActualStoreSiteId"]) ;
            //dr["isFinished"] = DataConvert.ToString( drCheck["IsFinished"]);
            //dr["checkResult"] =DataConvert.ToString( drCheck["CheckResultId"]) ;
            dr["checkDate"] = DataConvert.ToDBObject(checkDate);
            //dr["actualCheckDate"] =DataConvert.ToDBObject( drCheck["ActualCheckDate"]) ;
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }


        protected int AddDetail(AssetsCheck AssetsCheck, string AssetsCheckId, UserInfo sysUser, string viewTitle, string updateType)
        {
            string sql = @"select * from AssetsCheckDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsCheckDetail";
            DataRow dr = dt.NewRow();
            dr["AssetsCheckId"] = AssetsCheckId;
            dr["assetsId"] = AssetsCheck.AssetsId;
            dr["remark"] = AssetsCheck.Remark;
            dr["departmentId"] = AssetsCheck.DepartmentId;
            dr["storeSiteId"] = AssetsCheck.StoreSiteId;
            dr["actualDepartmentId"] = AssetsCheck.ActualDepartmentId;
            dr["actualStoreSiteId"] = AssetsCheck.ActualStoreSiteId;
            dr["isFinished"] = AssetsCheck.IsFinished;
            dr["checkResult"] = AssetsCheck.CheckResultId;
            dr["checkDate"] = DataConvert.ToDBObject(AssetsCheck.CheckDate);
            dr["actualCheckDate"] = DataConvert.ToDBObject(AssetsCheck.ActualCheckDate);
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            if (DataConvert.ToString(AssetsCheck.CreateId) != "")
            {
                if (updateType == "Reapply")
                    dr["approveState"] = "O";
                else
                    dr["approveState"] = AssetsCheck.ApproveState;
                dr["createId"] = AssetsCheck.CreateId;
                dr["createTime"] = AssetsCheck.CreateTime;
                Update5Field(dt, sysUser.UserId, viewTitle);
            }
            else
            {
                if (updateType == "ApproveAdd")
                    dr["approveState"] = "O";
                Create5Field(dt, sysUser.UserId, viewTitle);
            }
            return DbUpdate.Update(dt);
        }

        protected int UpdateDetail(AssetsCheck AssetsCheck, string AssetsCheckId, UserInfo sysUser, string viewTitle, string updateType)
        {
            string sql = string.Format(@"select * from AssetsCheckDetail where AssetsCheckId='{0}' and assetsId='{1}' ", AssetsCheckId, AssetsCheck.AssetsId);
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsCheckDetail";
            if (dt.Rows.Count > 0)
            {
                dt.Rows[0]["actualDepartmentId"] = AssetsCheck.ActualDepartmentId;
                dt.Rows[0]["actualStoreSiteId"] = AssetsCheck.ActualStoreSiteId;
                dt.Rows[0]["isFinished"] = AssetsCheck.IsFinished;
                dt.Rows[0]["checkResult"] = AssetsCheck.CheckResultId;
                dt.Rows[0]["checkDate"] = DataConvert.ToDBObject(AssetsCheck.CheckDate);
                dt.Rows[0]["actualCheckDate"] = DataConvert.ToDBObject(AssetsCheck.ActualCheckDate);
            }
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected int DeleteDetail(string pkValue, UserInfo sysUser, string viewTitle, bool needUpdateChecking = false)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsCheckId", pkValue);
            string sql = @"select * from AssetsCheckDetail where AssetsCheckId=@AssetsCheckId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsCheckDetail";
            foreach (DataRow dr in dt.Rows)
            {
                if (needUpdateChecking)
                    UpdateChecking(DataConvert.ToString(dr["assetsId"]), "N", sysUser.UserId, viewTitle);
                dr.Delete();
            }
            return DbUpdate.Update(dt);
        }

        public override int DealEndApprove(string approvePkValue, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsCheckId", approvePkValue);
            string sql = @"select * from AssetsCheckDetail where assetsCheckId=@assetsCheckId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            foreach (DataRow dr in dtAssets.Rows)
            {
                UpdateChecking(DataConvert.ToString(dr["assetsId"]), "Y", sysUser.UserId, viewTitle);
            }
            return 1;
        }

        protected int UpdateChecking(string assetsId, string checking, string sysUser, string viewTitle, string actualStoreSiteId = "")
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsId", assetsId);
            string sql = @"select * from Assets where assetsId=@assetsId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dtAssets.TableName = "Assets";
            dtAssets.Rows[0]["checking"] = checking;
            if (DataConvert.ToString( actualStoreSiteId) != "")
                dtAssets.Rows[0]["storeSiteId"] = actualStoreSiteId;
            Update5Field(dtAssets, sysUser, viewTitle);
            return DbUpdate.Update(dtAssets);
        }


    }
}

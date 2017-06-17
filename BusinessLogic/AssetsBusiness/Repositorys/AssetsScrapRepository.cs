using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.AssetsBusiness.Models.AssetsScrap;
using BaseCommon.Models;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsScrap
    {
        public string AssetsId { get; set; }
        public string ScrapTypeId { get; set; }
        public string ScrapTypeName { get; set; }
        public string ScrapDate { get; set; }
        public string Remark { get; set; }
        public string ApproveState { get; set; }
        public string CreateId { get; set; }
        public string CreateTime { get; set; }
    }

    public class AssetsScrapRepository : ApproveMasterRepository
    {

        public AssetsScrapRepository()
        {
            DefaulteGridSortField = "assetsScrapNo";
            MasterTable = "AssetsScrap";
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
            if (DataConvert.ToString(model.AssetsScrapNo) != "")
            {
                wcd.Sql += @" and AssetsScrap.assetsScrapNo like '%'+@assetsScrapNo+'%'";
                wcd.DBPara.Add("assetsScrapNo", model.AssetsScrapNo);
            }
            if (DataConvert.ToString(model.AssetsScrapName) != "")
            {
                wcd.Sql += @" and AssetsScrap.assetsScrapName like '%'+@assetsScrapName+'%'";
                wcd.DBPara.Add("assetsScrapName", model.AssetsScrapName);
            }
            if (DataConvert.ToString(model.ScrapDate1) != "")
            {
                wcd.Sql += @" and exists(select 1 from AssetsScrapDetail where AssetsScrapDetail.assetsScrapId=AssetsScrap.assetsScrapId
                             and AssetsScrapDetail.scrapDate>=@scrapDate1)";
                wcd.DBPara.Add("scrapDate1", DataConvert.ToString(model.ScrapDate1) + " 00:00:00");
            }
            if (DataConvert.ToString(model.ScrapDate2) != "")
            {
                wcd.Sql += @" and exists(select 1 from AssetsScrapDetail where AssetsScrapDetail.assetsScrapId=AssetsScrap.assetsScrapId
                        and AssetsScrapDetail.scrapDate<=@scrapDate2)";
                wcd.DBPara.Add("scrapDate2", DataConvert.ToString(model.ScrapDate2) + " 23:59:59");
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            ApproveListCondition acondition = condition as ApproveListCondition;
            string lsql = " where 1=1";
            if (DataConvert.ToString(acondition.ListMode) != "")
            {
                if (DataConvert.ToString(acondition.ListMode) == "approve")
                {
                    lsql = @",AppApprove
                     where AppApprove.tableName='AssetsScrap' and AppApprove.approveState='O'
                      and AssetsScrap.assetsScrapId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(acondition.ListMode) == "reapply")
                {
                    lsql = @" where AssetsScrap.createId=@approver and AssetsScrap.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select AssetsScrap.assetsScrapId assetsScrapId,
                        AssetsScrap.assetsScrapNo assetsScrapNo,
                        AssetsScrap.assetsScrapName assetsScrapName,
                           convert(nvarchar(100), (select top 1 scrapDate from AssetsScrapDetail where assetsScrapId=AssetsScrap.assetsScrapId),23)  scrapDate,
                         (select top 1 codename from CodeTable where codetype='ApproveState' and  AssetsScrap.approveState=codeno) approveState,                  
                        (select userName from AppUser where AssetsScrap.createId=AppUser.userId) createId,
                        AssetsScrap.createTime createTime ,
                        (select userName from AppUser where AssetsScrap.updateId=AppUser.userId) updateId,
                        AssetsScrap.updateTime updateTime ,
                        AssetsScrap.updatePro updatePro
                from AssetsScrap  {0} ", lsql);
            return subViewSql;
        }


        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            model.ApproveTableName = "AssetsScrap";
            model.ApprovePkField = "assetsScrapId";
            if (formMode != "new" && formMode != "new2")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("assetsScrapId", primaryKey);
                string sql = @"select AssetsScrap.assetsScrapId,
                        AssetsScrap.assetsScrapNo,
                        AssetsScrap.assetsScrapName,
                        AssetsScrapDetail.scrapDate
                         from AssetsScrap,AssetsScrapDetail 
                        where AssetsScrap.assetsScrapId=AssetsScrapDetail.assetsScrapId 
                        and  AssetsScrap.assetsScrapId=@assetsScrapId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.AssetsScrapId = primaryKey;
                model.AssetsScrapNo = DataConvert.ToString(dr["assetsScrapNo"]);
                model.AssetsScrapName = DataConvert.ToString(dr["assetsScrapName"]);
                model.ScrapDate = DataConvert.ToDateTime(dr["scrapDate"]);
            }
        }


        public override DataTable GetEntryGridDataTable(Dictionary<string, object> paras, string formMode, string primaryKey, string formVar)
        {
            if (formMode == "new" || formMode == "new2")
            {
                return null;
            }
            else
            {
                if (!paras.ContainsKey("assetsScrapId"))
                    paras.Add("assetsScrapId", primaryKey);
                string sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
               (select departmentName from AppDepartment C where D.companyId=C.departmentId ) companyId,               
                D.departmentName departmentId,
                (select storeSiteName from StoreSite where Assets.storeSiteId=StoreSite.storeSiteId) storeSiteId,
                   Assets.assetsValue assetsValue,
                 Assets.assetsNetValue  assetsNetValue,
 Assets.spec spec,
Assets.assetsQty assetsQty,
                AssetsScrapDetail.scrapTypeId ScrapTypeId,
                    {0} ScrapTypeName,
                    AssetsScrapDetail.remark Remark,
                AssetsScrapDetail.approveState ApproveState,
                AssetsScrapDetail.createId CreateId,
                AssetsScrapDetail.createTime CreateTime
                from AssetsScrapDetail inner join Assets on AssetsScrapDetail.assetsId=Assets.assetsId 
                left join AppDepartment D on Assets.departmentId=D.departmentId 
                where  AssetsScrapDetail.assetsScrapId=@assetsScrapId  ",
                SetScrapTypeName(formMode));
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                return dtGrid;
            }

        }

        protected virtual string SetScrapTypeName(string formMode)
        {
            if (formMode.Contains("view") || formMode == "approve")
                return "(select scrapTypeName from ScrapType where AssetsScrapDetail.scrapTypeId=ScrapType.scrapTypeId) ";
            else
                return "AssetsScrapDetail.scrapTypeId ";
        }

        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsScrap where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsScrap";
            DataRow dr = dt.NewRow();
            string assetsScrapId = IdGenerator.GetMaxId(dt.TableName);
            int retApprove = InitFirstApproveTask("AssetsScrap", "assetsScrapId", assetsScrapId, viewTitle, "", sysUser.UserId);
            EntryModel myModel = model as EntryModel;
            dr["assetsScrapNo"] = myModel.AssetsScrapNo;
            dr["assetsScrapName"] = myModel.AssetsScrapName;
            string updateType = "Add";
            if (retApprove != 0)
            {
                dr["approveState"] = "O";
                updateType = "ApproveAdd";
            }
            dr["assetsScrapId"] = assetsScrapId;
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);

            List<AssetsScrap> gridData = JsonHelper.JSONStringToList<AssetsScrap>(DataConvert.ToString(myModel.EntryGridString));
            if (gridData.Count < 1)
                throw new Exception(AppMember.AppText["NeedMoreThanOneAssets"]);
            foreach (AssetsScrap assetsScrap in gridData)
            {
                assetsScrap.ScrapDate = DataConvert.ToString(myModel.ScrapDate);
                AddDetail(assetsScrap, assetsScrapId, sysUser, viewTitle, updateType);
                if (retApprove != 0)
                {
                    UpdateAssetsState(assetsScrap.AssetsId, "RI", sysUser.UserId, viewTitle);
                }
                else
                {
                    UpdateAssetsState(assetsScrap.AssetsId, "R", sysUser.UserId, viewTitle);
                }
            }

            return 1;
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsScrapId", pkValue);
            string sql = @"select * from AssetsScrap where assetsScrapId=@assetsScrapId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsScrap";
            string assetsScrapId = DataConvert.ToString(dt.Rows[0]["assetsScrapId"]);
            EntryModel myModel = model as EntryModel;
            dt.Rows[0]["assetsScrapNo"] = myModel.AssetsScrapNo;
            dt.Rows[0]["assetsScrapName"] = myModel.AssetsScrapName;
            string updateType = "Modified";
            if (formMode == "reapply")
            {
                dt.Rows[0]["approveState"] = "O";
                updateType = "Reapply";
            }
            Update5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            DeleteDetail(pkValue, sysUser, viewTitle);
            List<AssetsScrap> gridData = JsonHelper.JSONStringToList<AssetsScrap>(DataConvert.ToString(myModel.EntryGridString));
            if (gridData.Count < 1)
                throw new Exception(AppMember.AppText["NeedMoreThanOneAssets"]);
            foreach (AssetsScrap assetsScrap in gridData)
            {
                assetsScrap.ScrapDate = DataConvert.ToString(myModel.ScrapDate);
                AddDetail(assetsScrap, pkValue, sysUser, viewTitle, updateType);
            }
            if (formMode == "reapply")
                InitFirstApproveTask("AssetsScrap", "assetsScrapId", assetsScrapId, viewTitle, formMode, sysUser.UserId);
            return 1;
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsScrapId", pkValue);
            string sql = @"select * from AssetsScrap where assetsScrapId=@assetsScrapId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsScrap";
            dt.Rows[0].Delete();
            DbUpdate.Update(dt);
            DeleteDetail(pkValue, sysUser, viewTitle);
            DeleteApproveData("AssetsScrap", pkValue, sysUser.UserId);
            return 1;
        }

        protected int AddDetail(AssetsScrap assetsScrap, string assetsScrapId, UserInfo sysUser, string viewTitle, string updateType)
        {
            string sql = @"select * from AssetsScrapDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsScrapDetail";
            DataRow dr = dt.NewRow();
            dr["assetsScrapId"] = assetsScrapId;
            dr["assetsId"] = assetsScrap.AssetsId;
            dr["remark"] = assetsScrap.Remark;
            dr["scrapTypeId"] =DataConvert.ToDBObject(assetsScrap.ScrapTypeId);
            dr["scrapDate"] = DataConvert.ToDBObject(assetsScrap.ScrapDate);
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            if (DataConvert.ToString(assetsScrap.CreateId) != "")
            {
                if (updateType == "Reapply")
                    dr["approveState"] = "O";
                else
                    dr["approveState"] = assetsScrap.ApproveState;
                dr["createId"] = assetsScrap.CreateId;
                dr["createTime"] = assetsScrap.CreateTime;
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

        protected int DeleteDetail(string pkValue, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsScrapId", pkValue);
            string sql = @"select * from AssetsScrapDetail where assetsScrapId=@assetsScrapId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsScrapDetail";
            foreach (DataRow dr in dt.Rows)
            {
                UpdateAssetsState(DataConvert.ToString(dr["assetsId"]), "A", sysUser.UserId, viewTitle);
                dr.Delete();
            }
            return DbUpdate.Update(dt);
        }

        public override int DealEndApprove(string approvePkValue, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsScrapId", approvePkValue);
            string sql = @"select * from AssetsScrapDetail where assetsScrapId=@assetsScrapId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            foreach (DataRow dr in dtAssets.Rows)
            {
                UpdateAssetsState(DataConvert.ToString(dr["assetsId"]), "R", sysUser.UserId, viewTitle);
            }
            return 1;
        }

    }
}

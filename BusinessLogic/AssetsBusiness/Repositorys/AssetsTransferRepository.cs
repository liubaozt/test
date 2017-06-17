using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.AssetsBusiness.Models.AssetsTransfer;
using BaseCommon.Models;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsTransfer
    {
        public string AssetsId { get; set; }
        public string OriginalDepartmentId { get; set; }
        public string OriginalStoreSiteId { get; set; }
        public string OriginalKeeper { get; set; }
        public string OriginalUsePeople { get; set; }
        public string AssetsStateId { get; set; }
        public string AssetsStateName { get; set; }
        public string Remark { get; set; }
        public string ApproveState { get; set; }
        public string CreateId { get; set; }
        public string CreateTime { get; set; }
    }

    public class AssetsTransferRepository : ApproveMasterRepository
    {

        public AssetsTransferRepository()
        {
            DefaulteGridSortField = "assetsTransferNo";
            MasterTable = "AssetsTransfer";
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
            if (DataConvert.ToString(model.AssetsTransferNo) != "")
            {
                wcd.Sql += @" and AssetsTransfer.assetsTransferNo like '%'+@assetsTransferNo+'%'";
                wcd.DBPara.Add("assetsTransferNo", model.AssetsTransferNo);
            }
            if (DataConvert.ToString(model.AssetsTransferName) != "")
            {
                wcd.Sql += @" and AssetsTransfer.assetsTransferName like '%'+@assetsTransferName+'%'";
                wcd.DBPara.Add("assetsTransferName", model.AssetsTransferName);
            }
            if (DataConvert.ToString(model.TransferDate1) != "")
            {
                wcd.Sql += @" and exists (select 1 from AssetsTransferDetail where AssetsTransferDetail.assetsTransferId=AssetsTransfer.assetsTransferId
                           and AssetsTransferDetail.transferDate>=@transferDate1)";
                wcd.DBPara.Add("transferDate1", DataConvert.ToString(model.TransferDate1) + " 00:00:00");
            }
            if (DataConvert.ToString(model.TransferDate2) != "")
            {
                wcd.Sql += @" and exists(select 1 from AssetsTransferDetail where AssetsTransferDetail.assetsTransferId=AssetsTransfer.assetsTransferId
                          and AssetsTransferDetail.transferDate<=@transferDate2)";
                wcd.DBPara.Add("transferDate2", DataConvert.ToString(model.TransferDate2) + " 23:59:59");
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
                     where AppApprove.tableName='AssetsTransfer' and AppApprove.approveState='O'
                      and AssetsTransfer.assetsTransferId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(acondition.ListMode) == "reapply")
                {
                    lsql = @" where AssetsTransfer.createId=@approver and AssetsTransfer.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select  AssetsTransfer.assetsTransferId assetsTransferId,
                        AssetsTransfer.assetsTransferNo assetsTransferNo,
                        AssetsTransfer.assetsTransferName assetsTransferName,
                         convert(nvarchar(100), (select top 1 transferDate from AssetsTransferDetail where assetsTransferId=AssetsTransfer.assetsTransferId),23)  transferDate,
  (select top 1 codename from CodeTable where codetype='ApproveState' and  AssetsTransfer.approveState=codeno) approveState,                       
(select top 1 AssetsClass.assetsClassName from AssetsTransferDetail,Assets,AssetsClass where assetsTransferId=AssetsTransfer.assetsTransferId and Assets.assetsId=AssetsTransferDetail.assetsId and Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                       (select top 1 Assets.assetsBarcode from AssetsTransferDetail,Assets where assetsTransferId=AssetsTransfer.assetsTransferId and Assets.assetsId=AssetsTransferDetail.assetsId) assetsNo,
                       (select top 1 Assets.assetsName from AssetsTransferDetail,Assets where assetsTransferId=AssetsTransfer.assetsTransferId and Assets.assetsId=AssetsTransferDetail.assetsId) assetsName,
                       (select top 1 Assets.spec from AssetsTransferDetail,Assets where assetsTransferId=AssetsTransfer.assetsTransferId and Assets.assetsId=AssetsTransferDetail.assetsId) spec,
                       (select top 1 Assets.assetsQty from AssetsTransferDetail,Assets where assetsTransferId=AssetsTransfer.assetsTransferId and Assets.assetsId=AssetsTransferDetail.assetsId) assetsQty,
                        (select userName from AppUser where AssetsTransfer.createId=AppUser.userId) createId,
                        AssetsTransfer.createTime createTime ,
                        (select userName from AppUser where AssetsTransfer.updateId=AppUser.userId) updateId,
                        AssetsTransfer.updateTime updateTime ,
                        AssetsTransfer.updatePro updatePro
                from AssetsTransfer  {0}", lsql);


            return subViewSql;
        }

        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            model.ApproveTableName = "AssetsTransfer";
            model.ApprovePkField = "assetsTransferId";
            if (formMode != "new" && formMode != "new2")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("assetsTransferId", primaryKey);
                string sql = @"select AssetsTransfer.assetsTransferId,
                            AssetsTransfer.assetsTransferNo,
                            AssetsTransfer.assetsTransferName,
                            AssetsTransferDetail.transferDate,
                            AssetsTransferDetail.newDepartmentId,
                            AssetsTransferDetail.newStoreSiteId,
                            AssetsTransferDetail.newKeeper,
                            AssetsTransferDetail.newUsePeople
                             from AssetsTransfer,AssetsTransferDetail 
                            where AssetsTransfer.assetsTransferId=AssetsTransferDetail.assetsTransferId 
                            and AssetsTransfer.assetsTransferId=@assetsTransferId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.AssetsTransferId = primaryKey;
                model.AssetsTransferNo = DataConvert.ToString(dr["assetsTransferNo"]);
                model.AssetsTransferName = DataConvert.ToString(dr["assetsTransferName"]);
                model.DepartmentId = DataConvert.ToString(dr["newDepartmentId"]);
                model.StoreSiteId = DataConvert.ToString(dr["newStoreSiteId"]);
                model.UsePeople = DataConvert.ToString(dr["newUsePeople"]);
                model.Keeper = DataConvert.ToString(dr["newKeeper"]);
                model.TransferDate = DataConvert.ToDateTime(dr["transferDate"]);
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
                if (!paras.ContainsKey("assetsTransferId"))
                    paras.Add("assetsTransferId", primaryKey);
                string sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
               AssetsTransferDetail.originalDepartmentId originalDepartmentId,
                     AssetsTransferDetail.originalStoreSiteId originalStoreSiteId,
                     AssetsTransferDetail.originalKeeper originalKeeper,
                     AssetsTransferDetail.originalUsePeople originalUsePeople,
               (select departmentName from AppDepartment C where D.companyId=C.departmentId ) companyId,
                 D.departmentName  originalDepartmentName, 
                 (select storeSiteName from StoreSite where AssetsTransferDetail.originalStoreSiteId=StoreSite.storeSiteId) originalStoreSiteName,
                 isnull((select userName from AppUser where AssetsTransferDetail.originalKeeper=AppUser.userId), AssetsTransferDetail.originalKeeper) originalKeeperName,
                 isnull((select userName from AppUser where AssetsTransferDetail.originalUsePeople=AppUser.userId),AssetsTransferDetail.originalUsePeople) originalUsePeopleName,
     AssetsTransferDetail.assetsState AssetsStateId,
                    {0} AssetsStateName,                 
AssetsTransferDetail.remark Remark,
 Assets.spec spec,
Assets.assetsQty assetsQty,
                AssetsTransferDetail.approveState ApproveState,
                AssetsTransferDetail.createId CreateId,
                AssetsTransferDetail.createTime CreateTime
                from AssetsTransferDetail inner join Assets on AssetsTransferDetail.assetsId=Assets.assetsId
              left join AppDepartment D on AssetsTransferDetail.originalDepartmentId=D.departmentId
                where AssetsTransferDetail.assetsTransferId=@assetsTransferId  ", SetAssetsStateName(formMode));
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                return dtGrid;
            }

        }

        protected virtual string SetAssetsStateName(string formMode)
        {
            if (formMode.Contains("view") || formMode == "approve")
                return string.Format("(select CodeTable.codeName from CodeTable where  AssetsTransferDetail.assetsState=CodeTable.codeNo and CodeTable.codeType='AssetsState' and CodeTable.languageVer='{0}' ) ", AppMember.AppLanguage.ToString());
            else
                return " AssetsTransferDetail.assetsState ";
        }

        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsTransfer where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsTransfer";
            DataRow dr = dt.NewRow();
            string assetsTransferId = IdGenerator.GetMaxId(dt.TableName);
            int retApprove = InitFirstApproveTask("AssetsTransfer", "assetsTransferId", assetsTransferId, viewTitle, "", sysUser.UserId);
            EntryModel myModel = model as EntryModel;
            myModel.AssetsTransferId = assetsTransferId;
            UpdateUser(myModel, sysUser, viewTitle);
            dr["assetsTransferNo"] = myModel.AssetsTransferNo;
            dr["assetsTransferName"] = myModel.AssetsTransferName;
            string updateType = "Add";
            if (retApprove != 0)
            {
                dr["approveState"] = "O";
                updateType = "ApproveAdd";
            }
            dr["assetsTransferId"] = assetsTransferId;
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            List<AssetsTransfer> gridData = JsonHelper.JSONStringToList<AssetsTransfer>(DataConvert.ToString(myModel.EntryGridString));
            if (gridData.Count < 1)
                throw new Exception(AppMember.AppText["NeedMoreThanOneAssets"]);
            foreach (AssetsTransfer assetsTransfer in gridData)
            {
                AddDetail(assetsTransfer, myModel, assetsTransferId, sysUser, viewTitle, updateType);
                if (retApprove == 0)
                {
                    UpdateAssets(assetsTransfer.AssetsId, DataConvert.ToString(myModel.DepartmentId),
                        DataConvert.ToString(myModel.StoreSiteId), DataConvert.ToString(myModel.Keeper),
                        DataConvert.ToString(myModel.UsePeople), sysUser.UserId, viewTitle, assetsTransfer.AssetsStateId);
                }
                else
                {
                    UpdateAssetsState(assetsTransfer.AssetsId, "TI", sysUser.UserId, viewTitle);
                }

            }

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

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsTransferId", pkValue);
            string sql = @"select * from AssetsTransfer where assetsTransferId=@assetsTransferId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsTransfer";
            string assetsTransferId = DataConvert.ToString(dt.Rows[0]["assetsTransferId"]);
            EntryModel myModel = model as EntryModel;
            UpdateUser(myModel, sysUser, viewTitle);
            dt.Rows[0]["assetsTransferNo"] = myModel.AssetsTransferNo;
            dt.Rows[0]["assetsTransferName"] = myModel.AssetsTransferName;
            string updateType = "Modified";
            if (formMode == "reapply")
            {
                dt.Rows[0]["approveState"] = "O";
                updateType = "Reapply";
            }
            Update5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            DeleteDetail(pkValue, sysUser, viewTitle);
            List<AssetsTransfer> gridData = JsonHelper.JSONStringToList<AssetsTransfer>(DataConvert.ToString(myModel.EntryGridString));
            if (gridData.Count < 1)
                throw new Exception(AppMember.AppText["NeedMoreThanOneAssets"]);
            foreach (AssetsTransfer assetsTransfer in gridData)
            {
                AddDetail(assetsTransfer, myModel, pkValue, sysUser, viewTitle, updateType);
            }
            if (formMode == "reapply")
                InitFirstApproveTask("AssetsTransfer", "assetsTransferId", assetsTransferId, viewTitle, formMode, sysUser.UserId);
            return 1;
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsTransferId", pkValue);
            string sql = @"select * from AssetsTransfer where assetsTransferId=@assetsTransferId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsTransfer";
            dt.Rows[0].Delete();
            DbUpdate.Update(dt);
            DeleteDetail(pkValue, sysUser, viewTitle);
            DeleteApproveData("AssetsTransfer", pkValue, sysUser.UserId);
            return 1;
        }

        protected int AddDetail(AssetsTransfer assetsTransfer, EntryModel model, string assetsTransferId, UserInfo sysUser, string viewTitle, string updateType)
        {
            string sql = @"select * from AssetsTransferDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsTransferDetail";
            DataRow dr = dt.NewRow();
            dr["assetsTransferId"] = assetsTransferId;
            dr["assetsId"] = DataConvert.ToDBObject(assetsTransfer.AssetsId);
            dr["originalDepartmentId"] = DataConvert.ToDBObject(assetsTransfer.OriginalDepartmentId);
            dr["originalStoreSiteId"] = DataConvert.ToDBObject(assetsTransfer.OriginalStoreSiteId);
            dr["originalKeeper"] = DataConvert.ToDBObject(assetsTransfer.OriginalKeeper);
            dr["originalUsePeople"] = DataConvert.ToDBObject(assetsTransfer.OriginalUsePeople);
            dr["newDepartmentId"] = DataConvert.ToDBObject(model.DepartmentId);
            dr["newStoreSiteId"] = DataConvert.ToDBObject(model.StoreSiteId);
            dr["newKeeper"] = DataConvert.ToDBObject(model.Keeper);
            dr["newUsePeople"] = DataConvert.ToDBObject(model.UsePeople);
            dr["transferDate"] = DataConvert.ToDBObject(model.TransferDate);
            dr["remark"] = assetsTransfer.Remark;
            dr["assetsState"] = assetsTransfer.AssetsStateId;
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            if (DataConvert.ToString(assetsTransfer.CreateId) != "")
            {
                if (updateType == "Reapply")
                    dr["approveState"] = "O";
                else
                    dr["approveState"] = assetsTransfer.ApproveState;
                dr["createId"] = assetsTransfer.CreateId;
                dr["createTime"] = assetsTransfer.CreateTime;
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
            paras.Add("assetsTransferId", pkValue);
            string sql = @"select * from AssetsTransferDetail where assetsTransferId=@assetsTransferId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsTransferDetail";
            foreach (DataRow dr in dt.Rows)
            {
                UpdateAssetsState(DataConvert.ToString(dr["assetsId"]), "A", sysUser.UserId, viewTitle);
                dr.Delete();
            }
            return DbUpdate.Update(dt);
        }

        protected int UpdateAssets(string assetsId, string departmentId, string storeSiteId, string keeper, string usePeople, string sysUser, string viewTitle, string assetsState)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsId", assetsId);
            string sql = @"select * from Assets where assetsId=@assetsId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dtAssets.TableName = "Assets";
            if (DataConvert.ToString(departmentId) != "")
                dtAssets.Rows[0]["departmentId"] = DataConvert.ToDBObject(departmentId);
            if (DataConvert.ToString(storeSiteId) != "")
                dtAssets.Rows[0]["storeSiteId"] = DataConvert.ToDBObject(storeSiteId);
            if (DataConvert.ToString(keeper) != "")
                dtAssets.Rows[0]["keeper"] = DataConvert.ToDBObject(keeper);
            if (DataConvert.ToString(usePeople) != "")
                dtAssets.Rows[0]["usePeople"] = DataConvert.ToDBObject(usePeople);
            if (DataConvert.ToString(assetsState) != "")
                dtAssets.Rows[0]["assetsState"] = assetsState;
            //else
            //    dtAssets.Rows[0]["assetsState"] = "A";
            Update5Field(dtAssets, sysUser, viewTitle);
            return DbUpdate.Update(dtAssets);
        }

        public override int DealEndApprove(string approvePkValue, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsTransferId", approvePkValue);
            string sql = @"select * from AssetsTransferDetail where assetsTransferId=@assetsTransferId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            foreach (DataRow dr in dtAssets.Rows)
            {
                UpdateAssets(DataConvert.ToString(dr["assetsId"]), DataConvert.ToString(dr["newDepartmentId"]),
                    DataConvert.ToString(dr["newStoreSiteId"]), DataConvert.ToString(dr["newKeeper"]),
                    DataConvert.ToString(dr["newUsePeople"]), sysUser.UserId, viewTitle, DataConvert.ToString(dr["assetsState"]));
            }
            return 1;
        }


    }
}

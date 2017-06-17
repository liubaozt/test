using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.AssetsBusiness.Models.AssetsBorrow;
using BaseCommon.Models;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsBorrow
    {
        public string AssetsId { get; set; }
        public string PlanReturnDate { get; set; }
        public string BorrowPeople { get; set; }
        public string BorrowDepartmentId { get; set; }
        public string BorrowDate { get; set; }
        public string Remark { get; set; }
        public string ApproveState { get; set; }
        public string CreateId { get; set; }
        public string CreateTime { get; set; }
    }

    public class AssetsBorrowRepository : ApproveMasterRepository
    {

        public AssetsBorrowRepository()
        {
            DefaulteGridSortField = "assetsBorrowNo";
            MasterTable = "AssetsBorrow";
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
            if (DataConvert.ToString(model.AssetsBorrowNo) != "")
            {
                wcd.Sql += @" and AssetsBorrow.assetsBorrowNo like '%'+@assetsBorrowNo+'%'";
                wcd.DBPara.Add("assetsBorrowNo", model.AssetsBorrowNo);
            }
            if (DataConvert.ToString(model.AssetsBorrowName) != "")
            {
                wcd.Sql += @" and AssetsBorrow.assetsBorrowName like '%'+@assetsBorrowName+'%'";
                wcd.DBPara.Add("assetsBorrowName", model.AssetsBorrowName);
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
                     where AppApprove.tableName='AssetsBorrow' and AppApprove.approveState='O'
                      and AssetsBorrow.assetsBorrowId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(acondition.ListMode) == "reapply")
                {
                    lsql = @" where AssetsBorrow.createId=@approver and AssetsBorrow.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select  AssetsBorrow.assetsBorrowId assetsBorrowId,
                        AssetsBorrow.assetsBorrowNo assetsBorrowNo,
                        AssetsBorrow.assetsBorrowName assetsBorrowName,
                        (select userName from AppUser where AssetsBorrow.createId=AppUser.userId) createId,
                        AssetsBorrow.createTime createTime ,
                        (select userName from AppUser where AssetsBorrow.updateId=AppUser.userId) updateId,
                        AssetsBorrow.updateTime updateTime ,
                        AssetsBorrow.updatePro updatePro
                from AssetsBorrow  {0} ", lsql);
            return subViewSql;
        }

        public void SetModel(string primaryKey,string formMode, EntryModel model)
        {
            model.ApproveTableName = "AssetsBorrow";
            model.ApprovePkField = "assetsBorrowId";
            if (formMode != "new" && formMode != "new2")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("assetsBorrowId", primaryKey);
                string sql = @"select AssetsBorrow.assetsBorrowId,
                            AssetsBorrow.assetsBorrowNo,
                            AssetsBorrow.assetsBorrowName,
                            AssetsBorrowDetail.borrowPeople,
                            AssetsBorrowDetail.borrowDepartmentId,
                            AssetsBorrowDetail.borrowDate
                            from AssetsBorrow,AssetsBorrowDetail
                            where AssetsBorrow.assetsBorrowId=AssetsBorrowDetail.assetsBorrowId 
                             and AssetsBorrow.assetsBorrowId=@assetsBorrowId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.AssetsBorrowId = primaryKey;
                model.AssetsBorrowNo = DataConvert.ToString(dr["assetsBorrowNo"]);
                model.AssetsBorrowName = DataConvert.ToString(dr["assetsBorrowName"]);
                model.DepartmentId = DataConvert.ToString(dr["borrowDepartmentId"]);
                model.BorrowPeople = DataConvert.ToString(dr["borrowPeople"]);
                model.BorrowDate = DataConvert.ToDateTime(dr["borrowDate"]);
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
                if (!paras.ContainsKey("assetsBorrowId"))
                    paras.Add("assetsBorrowId", primaryKey);
                string sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                 (select departmentName from AppDepartment C where D.companyId=C.departmentId ) companyId,
                 D.departmentName departmentId,
                 (select userName from AppUser where Assets.usePeople=AppUser.userId) UsePeople,
                 (select userName from AppUser where Assets.keeper=AppUser.userId) Keeper,
                  AssetsBorrowDetail.planReturnDate PlanReturnDate,
                    AssetsBorrowDetail.remark Remark,
                AssetsBorrowDetail.approveState ApproveState,
                AssetsBorrowDetail.createId CreateId,
                AssetsBorrowDetail.createTime CreateTime
                from AssetsBorrowDetail  inner join  Assets on AssetsBorrowDetail.assetsId=Assets.assetsId
                left join AppDepartment D on AssetsBorrowDetail.borrowDepartmentId=D.departmentId 
                where AssetsBorrowDetail.assetsBorrowId=@assetsBorrowId  ");
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                return dtGrid;
            }

        }

        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsBorrow where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsBorrow";
            DataRow dr = dt.NewRow();
            string assetsBorrowId = IdGenerator.GetMaxId(dt.TableName);
            int retApprove = InitFirstApproveTask("AssetsBorrow", "assetsBorrowId", assetsBorrowId, viewTitle,"", sysUser.UserId);
            EntryModel myModel = model as EntryModel;
            dr["assetsBorrowNo"] = myModel.AssetsBorrowNo;
            dr["assetsBorrowName"] = myModel.AssetsBorrowName;
            string updateType = "Add";
            if (retApprove != 0)
            {
                dr["approveState"] = "O";
                updateType = "ApproveAdd";
            }
            dr["assetsBorrowId"] = assetsBorrowId;
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            List<AssetsBorrow> gridData = JsonHelper.JSONStringToList<AssetsBorrow>(DataConvert.ToString(myModel.EntryGridString));
            if (gridData.Count < 1)
                throw new Exception(AppMember.AppText["NeedMoreThanOneAssets"]);
            List<string> assetsIdList = new List<string>();
            foreach (AssetsBorrow assetsBorrow in gridData)
            {
                assetsBorrow.BorrowPeople = DataConvert.ToString(myModel.BorrowPeople);
                assetsBorrow.BorrowDepartmentId = DataConvert.ToString(myModel.DepartmentId);
                assetsBorrow.BorrowDate = DataConvert.ToString(myModel.BorrowDate);
                assetsIdList.Add(assetsBorrow.AssetsId);
            }
            AddDetail(gridData, assetsBorrowId, sysUser, viewTitle, updateType);
            if (retApprove != 0)
                UpdateAssetsState(assetsIdList, "BI", sysUser.UserId, viewTitle);
            else
                UpdateAssetsState(assetsIdList, "B", sysUser.UserId, viewTitle);
            //foreach (AssetsBorrow assetsBorrow in gridData)
            //{
            //    assetsBorrow.BorrowPeople = DataConvert.ToString(myModel.BorrowPeople);
            //    assetsBorrow.BorrowDepartmentId = DataConvert.ToString(myModel.DepartmentId);
            //    assetsBorrow.BorrowDate = DataConvert.ToString(myModel.BorrowDate);
            //    AddDetail(assetsBorrow, assetsBorrowId, sysUser.UserId, viewTitle,needApprove);
            //    if (retApprove != 0)
            //    {
            //        UpdateAssetsState(assetsBorrow.AssetsId, "BI", sysUser.UserId, viewTitle);
            //    }
            //}
            return 1;
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsBorrowId", pkValue);
            string sql = @"select * from AssetsBorrow where assetsBorrowId=@assetsBorrowId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsBorrow";
            string assetsBorrowId = DataConvert.ToString(dt.Rows[0]["assetsBorrowId"]);
            EntryModel myModel = model as EntryModel;
            dt.Rows[0]["assetsBorrowNo"] = myModel.AssetsBorrowNo;
            dt.Rows[0]["assetsBorrowName"] = myModel.AssetsBorrowName;
            string updateType = "Modified";
            if (formMode == "reapply")
            {
                dt.Rows[0]["approveState"] = "O";
                updateType = "Reapply";
            }
            Update5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            DeleteDetail(pkValue,sysUser,viewTitle);
            List<AssetsBorrow> gridData = JsonHelper.JSONStringToList<AssetsBorrow>(DataConvert.ToString(myModel.EntryGridString));
            if (gridData.Count < 1)
                throw new Exception(AppMember.AppText["NeedMoreThanOneAssets"]);
            foreach (AssetsBorrow assetsBorrow in gridData)
            {
                assetsBorrow.BorrowPeople = DataConvert.ToString(myModel.BorrowPeople);
                assetsBorrow.BorrowDepartmentId = DataConvert.ToString(myModel.DepartmentId);
                assetsBorrow.BorrowDate = DataConvert.ToString(myModel.BorrowDate);
            }
            AddDetail(gridData, assetsBorrowId, sysUser, viewTitle, updateType);
            if (formMode == "reapply")
                InitFirstApproveTask("AssetsBorrow", "assetsBorrowId", assetsBorrowId, viewTitle, formMode, sysUser.UserId);
            return 1;
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsBorrowId", pkValue);
            string sql = @"select * from AssetsBorrow where assetsBorrowId=@assetsBorrowId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsBorrow";
            dt.Rows[0].Delete();
            DbUpdate.Update(dt);
            DeleteDetail(pkValue, sysUser, viewTitle);
            DeleteApproveData("AssetsBorrow", pkValue, sysUser.UserId);
            return 1;
        }

        protected int AddDetail(AssetsBorrow assetsBorrow, string assetsBorrowId, UserInfo sysUser, string viewTitle, string updateType)
        {
            string sql = @"select * from AssetsBorrowDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsBorrowDetail";
            DataRow dr = dt.NewRow();
            dr["assetsBorrowId"] = assetsBorrowId;
            dr["assetsId"] = assetsBorrow.AssetsId;
            dr["planReturnDate"] = DataConvert.ToDBObject(assetsBorrow.PlanReturnDate);
            dr["borrowPeople"] = assetsBorrow.BorrowPeople;
            dr["borrowDepartmentId"] = assetsBorrow.BorrowDepartmentId;
            dr["borrowDate"] = DataConvert.ToDBObject(assetsBorrow.BorrowDate);
            dr["remark"] = assetsBorrow.Remark;
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            if (DataConvert.ToString(assetsBorrow.CreateId) != "")
            {
                if (updateType == "Reapply")
                    dr["approveState"] = "O";
                else
                    dr["approveState"] = assetsBorrow.ApproveState;
                dr["createId"] = assetsBorrow.CreateId;
                dr["createTime"] = assetsBorrow.CreateTime;
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

        protected int AddDetail(List<AssetsBorrow> gridData, string assetsBorrowId, UserInfo sysUser, string viewTitle, string updateType)
        {
            string sql = @"select * from AssetsBorrowDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsBorrowDetail";
            foreach (AssetsBorrow assetsBorrow in gridData)
            {
                DataRow dr = dt.NewRow();
                dr["assetsBorrowId"] = assetsBorrowId;
                dr["assetsId"] = assetsBorrow.AssetsId;
                dr["planReturnDate"] = DataConvert.ToDBObject(assetsBorrow.PlanReturnDate);
                dr["borrowPeople"] = DataConvert.ToDBObject(assetsBorrow.BorrowPeople);
                dr["borrowDepartmentId"] = DataConvert.ToDBObject(assetsBorrow.BorrowDepartmentId);
                dr["borrowDate"] = DataConvert.ToDBObject(assetsBorrow.BorrowDate);
                dr["remark"] = assetsBorrow.Remark;
                dr["alreadyReturn"] = "N";
                dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
                dt.Rows.Add(dr);
                if (DataConvert.ToString(assetsBorrow.CreateId) != "")
                {
                    if (updateType == "ApproveModified")
                        dr["approveState"] = assetsBorrow.ApproveState;
                    else
                        dr["approveState"] = assetsBorrow.ApproveState;
                    dr["createId"] = assetsBorrow.CreateId;
                    dr["createTime"] = assetsBorrow.CreateTime;
                    Update5Field(dt, sysUser.UserId, viewTitle);
                }
                else
                {
                    if (updateType == "ApproveAdd")
                        dr["approveState"] = "O";
                    Create5Field(dt, sysUser.UserId, viewTitle);
                }
            }
            return DbUpdate.Update(dt);
        }

        protected int DeleteDetail(string pkValue, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsBorrowId", pkValue);
            string sql = @"select * from AssetsBorrowDetail where assetsBorrowId=@assetsBorrowId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsBorrowDetail";
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
            paras.Add("assetsBorrowId", approvePkValue);
            string sql = @"select * from AssetsBorrowDetail where assetsBorrowId=@assetsBorrowId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            foreach (DataRow dr in dtAssets.Rows)
            {
                UpdateAssetsState(DataConvert.ToString(dr["assetsId"]), "B", sysUser.UserId, viewTitle);
            }
            return 1;
        }

    }
}

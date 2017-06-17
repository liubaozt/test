using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.AssetsBusiness.Models.AssetsReturn;
using BaseCommon.Models;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsReturn
    {
        public string AssetsId { get; set; }
        public string Remark { get; set; }
        public string ReturnPeople { get; set; }
        public string ReturnDate { get; set; }
        public string ApproveState { get; set; }
        public string CreateId { get; set; }
        public string CreateTime { get; set; }
    }

    public class AssetsReturnRepository : ApproveMasterRepository
    {

        public AssetsReturnRepository()
        {
            DefaulteGridSortField = "assetsReturnNo";
            MasterTable = "AssetsReturn";
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
            if (DataConvert.ToString(model.AssetsReturnNo) != "")
            {
                wcd.Sql += @" and AssetsReturn.assetsReturnNo like '%'+@assetsReturnNo+'%'";
                wcd.DBPara.Add("assetsReturnNo", model.AssetsReturnNo);
            }
            if (DataConvert.ToString(model.AssetsReturnName) != "")
            {
                wcd.Sql += @" and AssetsReturn.assetsReturnName like '%'+@assetsReturnName+'%'";
                wcd.DBPara.Add("assetsReturnName", model.AssetsReturnName);
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
                     where AppApprove.tableName='AssetsReturn' and AppApprove.approveState='O'
                      and AssetsReturn.assetsReturnId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(acondition.ListMode) == "reapply")
                {
                    lsql = @" where AssetsReturn.createId=@approver and AssetsReturn.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select  AssetsReturn.assetsReturnId assetsReturnId,
                        AssetsReturn.assetsReturnNo assetsReturnNo,
                        AssetsReturn.assetsReturnName assetsReturnName,
                        (select userName from AppUser where AssetsReturn.createId=AppUser.userId) createId,
                        AssetsReturn.createTime createTime ,
                        (select userName from AppUser where AssetsReturn.updateId=AppUser.userId) updateId,
                        AssetsReturn.updateTime updateTime ,
                        AssetsReturn.updatePro updatePro
                from AssetsReturn  {0}  ",  lsql);
            return subViewSql;
        }


        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            model.ApproveTableName = "AssetsReturn";
            model.ApprovePkField = "assetsReturnId";
            if (formMode != "new" && formMode != "new2")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("assetsReturnId", primaryKey);
                string sql = @"select AssetsReturn.assetsReturnNo,AssetsReturn.assetsReturnName,
                    AssetsReturnDetail.returnPeople,AssetsReturnDetail.returnDate
                    from AssetsReturn,AssetsReturnDetail
                    where AssetsReturn.assetsReturnId=@assetsReturnId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.AssetsReturnId = primaryKey;
                model.AssetsReturnNo = DataConvert.ToString(dr["assetsReturnNo"]);
                model.AssetsReturnName = DataConvert.ToString(dr["assetsReturnName"]);
                model.ReturnPeople = DataConvert.ToString(dr["returnPeople"]);
                model.ReturnDate = DataConvert.ToDateTime(dr["returnDate"]);
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
                if (!paras.ContainsKey("assetsReturnId"))
                    paras.Add("assetsReturnId", primaryKey);
                string sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                 (select departmentName from AppDepartment C where D.companyId=C.departmentId ) companyId,
                  D.departmentName  departmentId,
                 (select userName from AppUser where Assets.usePeople=AppUser.userId) UsePeople,
                 (select userName from AppUser where Assets.keeper=AppUser.userId) Keeper,
                (select AppUser.userName from AssetsBorrowDetail,AppUser where AssetsBorrowDetail.borrowPeople=AppUser.userId
                 and AssetsReturnDetail.assetsId=AssetsBorrowDetail.assetsId and AssetsBorrowDetail.alreadyReturn<>'Y') BorrowPeople,
               (select AppDepartment.departmentName from AssetsBorrowDetail,AppDepartment where AssetsBorrowDetail.borrowDepartmentId=AppDepartment.departmentId
               and AssetsReturnDetail.assetsId=AssetsBorrowDetail.assetsId and AssetsBorrowDetail.alreadyReturn<>'Y') BorrowDepartmentId,
                 convert(nvarchar(100), (select AssetsBorrowDetail.borrowDate from AssetsBorrowDetail where AssetsReturnDetail.assetsId=AssetsBorrowDetail.assetsId and AssetsBorrowDetail.alreadyReturn<>'Y' ),23)   BorrowDate,
                    AssetsReturnDetail.remark Remark,
                AssetsReturnDetail.approveState ApproveState,
                AssetsReturnDetail.createId CreateId,
                AssetsReturnDetail.createTime CreateTime
                from AssetsReturnDetail inner join Assets on AssetsReturnDetail.assetsId=Assets.assetsId 
                left join AppDepartment D on Assets.departmentId=D.departmentId 
                where AssetsReturnDetail.assetsReturnId=@assetsReturnId  ");
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                return dtGrid;
            }
            
        }

        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsReturn where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsReturn";
            DataRow dr = dt.NewRow();
            string assetsReturnId = IdGenerator.GetMaxId(dt.TableName);
            int retApprove = InitFirstApproveTask("AssetsReturn", "assetsReturnId", assetsReturnId, viewTitle, "", sysUser.UserId);
            EntryModel myModel = model as EntryModel;
            dr["assetsReturnNo"] = myModel.AssetsReturnNo;
            dr["assetsReturnName"] = myModel.AssetsReturnName;
            string updateType = "Add";
            if (retApprove != 0)
            {
                dr["approveState"] = "O";
                updateType = "ApproveAdd";
            }
            dr["assetsReturnId"] = assetsReturnId;
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);

            List<AssetsReturn> gridData = JsonHelper.JSONStringToList<AssetsReturn>(DataConvert.ToString(myModel.EntryGridString));
            if (gridData.Count < 1)
                throw new Exception(AppMember.AppText["NeedMoreThanOneAssets"]);
            foreach (AssetsReturn assetsReturn in gridData)
            {
                assetsReturn.ReturnPeople = myModel.ReturnPeople;
                assetsReturn.ReturnDate = DataConvert.ToString(myModel.ReturnDate);
                AddDetail(assetsReturn, assetsReturnId, sysUser, viewTitle, updateType);
                if (retApprove != 0)
                {
                    UpdateAssetsState(assetsReturn.AssetsId, "HI", sysUser.UserId, viewTitle);
                }
            }
            return 1;
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsReturnId", pkValue);
            string sql = @"select * from AssetsReturn where assetsReturnId=@assetsReturnId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsReturn";
            string assetsReturnId = DataConvert.ToString(dt.Rows[0]["assetsReturnId"]);
            EntryModel myModel = model as EntryModel;
            dt.Rows[0]["assetsReturnNo"] = myModel.AssetsReturnNo;
            dt.Rows[0]["assetsReturnName"] = myModel.AssetsReturnName;
            string updateType = "Modified";
            if (formMode == "reapply")
            {
                dt.Rows[0]["approveState"] = "O";
                updateType = "Reapply";
            }
            Update5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            DeleteDetail(pkValue,sysUser,viewTitle);
            List<AssetsReturn> gridData = JsonHelper.JSONStringToList<AssetsReturn>(DataConvert.ToString(myModel.EntryGridString));
            if (gridData.Count < 1)
                throw new Exception(AppMember.AppText["NeedMoreThanOneAssets"]);
            foreach (AssetsReturn assetsReturn in gridData)
            {
                AddDetail(assetsReturn, pkValue, sysUser, viewTitle, updateType);
            }
            if (formMode == "reapply")
                InitFirstApproveTask("AssetsReturn", "assetsReturnId", assetsReturnId, viewTitle, formMode, sysUser.UserId);
            return 1;
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsReturnId", pkValue);
            string sql = @"select * from AssetsReturn where assetsReturnId=@assetsReturnId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsReturn";
            dt.Rows[0].Delete();
            DbUpdate.Update(dt);
            DeleteDetail(pkValue, sysUser, viewTitle);
            DeleteApproveData("AssetsReturn", pkValue, sysUser.UserId);
            return 1;
        }

        protected int AddDetail(AssetsReturn assetsReturn, string assetsReturnId, UserInfo sysUser, string viewTitle, string updateType)
        {
            string sql = @"select * from AssetsReturnDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsReturnDetail";
            DataRow dr = dt.NewRow();
            dr["assetsReturnId"] = assetsReturnId;
            dr["assetsId"] = assetsReturn.AssetsId;
            dr["remark"] = assetsReturn.Remark;
            dr["returnPeople"] = assetsReturn.ReturnPeople;
            dr["returnDate"] = DataConvert.ToDBObject(assetsReturn.ReturnDate);
            dr["remark"] = assetsReturn.Remark;
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            if (DataConvert.ToString(assetsReturn.CreateId) != "")
            {
                if (updateType == "Reapply")
                    dr["approveState"] = "O";
                else
                    dr["approveState"] = assetsReturn.ApproveState;
                dr["createId"] = assetsReturn.CreateId;
                dr["createTime"] = assetsReturn.CreateTime;
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
            paras.Add("assetsReturnId", pkValue);
            string sql = @"select * from AssetsReturnDetail where assetsReturnId=@assetsReturnId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsReturnDetail";
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
            paras.Add("assetsReturnId", approvePkValue);
            string sql = @"select * from AssetsReturnDetail where assetsReturnId=@assetsReturnId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            foreach (DataRow dr in dtAssets.Rows)
            {
                UpdateAssetsState(DataConvert.ToString(dr["assetsId"]), "A", sysUser.UserId, viewTitle);
            }
            return 1;
        }
    }
}

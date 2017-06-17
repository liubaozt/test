using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.AssetsBusiness.Models.AssetsPurchase;
using BaseCommon.Models;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsPurchase
    {
        public string assetsPurchaseDetailId { get; set; }
        public string assetsPurchaseId { get; set; }   
        public string assetsName { get; set; }
        public string departmentId { get; set; }
        public string storeSiteId { get; set; }
        public string assetsValue { get; set; }
        public string usePeople { get; set; }
        public string keeper { get; set; }
        public string hasFixed { get; set; }
        public string remark { get; set; } 
    }

    public class AssetsPurchaseRepository : ApproveMasterRepository
    {

        public AssetsPurchaseRepository()
        {
            DefaulteGridSortField = "assetsPurchaseNo";
            MasterTable = "AssetsPurchase";
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
            if (DataConvert.ToString(model.AssetsPurchaseNo) != "")
            {
                wcd.Sql += @" and AssetsPurchase.assetsPurchaseNo like '%'+@assetsPurchaseNo+'%'";
                wcd.DBPara.Add("assetsPurchaseNo", model.AssetsPurchaseNo);
            }
            if (DataConvert.ToString(model.AssetsPurchaseName) != "")
            {
                wcd.Sql += @" and AssetsPurchase.assetsPurchaseName like '%'+@assetsPurchaseName+'%'";
                wcd.DBPara.Add("assetsPurchaseName", model.AssetsPurchaseName);
            }
            if (DataConvert.ToString(model.PurchaseDate1) != "")
            {
                wcd.Sql += @" and AssetsPurchase.purchaseDate>=@purchaseDate1)";
                wcd.DBPara.Add("purchaseDate1", DataConvert.ToString(model.PurchaseDate1) + " 00:00:00");
            }
            if (DataConvert.ToString(model.PurchaseDate2) != "")
            {
                wcd.Sql += @" and AssetsPurchase.purchaseDate<=@purchaseDate2)";
                wcd.DBPara.Add("purchaseDate2", DataConvert.ToString(model.PurchaseDate2) + " 23:59:59");
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
                     where AppApprove.tableName='AssetsPurchase' and AppApprove.approveState='O'
                      and AssetsPurchase.assetsPurchaseId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(acondition.ListMode) == "reapply")
                {
                    lsql = @" where AssetsPurchase.createId=@approver and AssetsPurchase.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select AssetsPurchase.assetsPurchaseId assetsPurchaseId,
                        AssetsPurchase.assetsPurchaseNo assetsPurchaseNo,
                        AssetsPurchase.assetsPurchaseName assetsPurchaseName,
                           convert(nvarchar(100),AssetsPurchase.purchaseDate,23)  purchaseDate,
                         (select top 1 codename from CodeTable where codetype='ApproveState' and  AssetsPurchase.approveState=codeno) approveState,                  
                        (select userName from AppUser where AssetsPurchase.createId=AppUser.userId) createId,
                        AssetsPurchase.createTime createTime ,
                        (select userName from AppUser where AssetsPurchase.updateId=AppUser.userId) updateId,
                        AssetsPurchase.updateTime updateTime ,
                        AssetsPurchase.updatePro updatePro
                from AssetsPurchase  {0} ", lsql);
            return subViewSql;
        }


        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            model.ApproveTableName = "AssetsPurchase";
            model.ApprovePkField = "assetsPurchaseId";
            if (formMode != "new" && formMode != "new2")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("assetsPurchaseId", primaryKey);
                string sql = @"select AssetsPurchase.assetsPurchaseId,
                        AssetsPurchase.assetsPurchaseNo,
                        AssetsPurchase.assetsPurchaseName,
                        AssetsPurchase.purchaseDate
                         from AssetsPurchase 
                        where AssetsPurchase.assetsPurchaseId=@assetsPurchaseId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.AssetsPurchaseId = primaryKey;
                model.AssetsPurchaseNo = DataConvert.ToString(dr["assetsPurchaseNo"]);
                model.AssetsPurchaseName = DataConvert.ToString(dr["assetsPurchaseName"]);
                model.PurchaseDate = DataConvert.ToDateTime(dr["purchaseDate"]);
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
                if (!paras.ContainsKey("assetsPurchaseId"))
                    paras.Add("assetsPurchaseId", primaryKey);
                string sql = string.Format(@"select  AssetsPurchaseDetail.assetsPurchaseDetailId assetsPurchaseDetailId,
                        AssetsPurchaseDetail.assetsPurchaseId assetsPurchaseId,
                        AssetsPurchaseDetail.assetsName assetsName,
 AssetsPurchaseDetail.departmentId departmentId,
(select top 1 departmentName from AppDepartment where AppDepartment.departmentId=AssetsPurchaseDetail.departmentId ) departmentName,
 AssetsPurchaseDetail.storeSiteId storeSiteId,
(select top 1 storeSiteName from StoreSite where StoreSite.storeSiteId=AssetsPurchaseDetail.storeSiteId ) storeSiteName,
 AssetsPurchaseDetail.usePeople usePeople,
 AssetsPurchaseDetail.keeper keeper,
 AssetsPurchaseDetail.assetsValue assetsValue,
 AssetsPurchaseDetail.remark remark,
 AssetsPurchaseDetail.hasFixed hasFixed,
(select codeName from CodeTable where CodeTable.codeType='BoolVal' and CodeTable.codeNo=AssetsPurchaseDetail.hasFixed) hasFixedText
                from AssetsPurchaseDetail 
                where  AssetsPurchaseDetail.assetsPurchaseId=@assetsPurchaseId  ");
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                return dtGrid;
            }

        }

        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsPurchase where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsPurchase";
            DataRow dr = dt.NewRow();
            string assetsPurchaseId = IdGenerator.GetMaxId(dt.TableName);
            int retApprove = InitFirstApproveTask("AssetsPurchase", "assetsPurchaseId", assetsPurchaseId, viewTitle, "", sysUser.UserId);
            EntryModel myModel = model as EntryModel;
            dr["assetsPurchaseNo"] = myModel.AssetsPurchaseNo;
            dr["assetsPurchaseName"] = myModel.AssetsPurchaseName;
            dr["purchaseDate"] =DataConvert.ToDBObject(myModel.PurchaseDate);
            string updateType = "Add";
            if (retApprove != 0)
            {
                dr["approveState"] = "O";
                updateType = "ApproveAdd";
            }
            dr["assetsPurchaseId"] = assetsPurchaseId;
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);

            List<AssetsPurchase> gridData = JsonHelper.JSONStringToList<AssetsPurchase>(DataConvert.ToString(myModel.EntryGridString));
            if (gridData.Count < 1)
                throw new Exception(AppMember.AppText["NeedMoreThanOneAssets"]);
            foreach (AssetsPurchase assetsPurchase in gridData)
            {
                AddDetail(assetsPurchase, assetsPurchaseId, sysUser, viewTitle, updateType);
            }

            return 1;
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsPurchaseId", pkValue);
            string sql = @"select * from AssetsPurchase where assetsPurchaseId=@assetsPurchaseId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsPurchase";
            string assetsPurchaseId = DataConvert.ToString(dt.Rows[0]["assetsPurchaseId"]);
            EntryModel myModel = model as EntryModel;
            dt.Rows[0]["assetsPurchaseNo"] = myModel.AssetsPurchaseNo;
            dt.Rows[0]["assetsPurchaseName"] = myModel.AssetsPurchaseName;
            string updateType = "Modified";
            if (formMode == "reapply")
            {
                dt.Rows[0]["approveState"] = "O";
                updateType = "Reapply";
            }
            Update5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            DeleteDetail(pkValue, sysUser, viewTitle);
            List<AssetsPurchase> gridData = JsonHelper.JSONStringToList<AssetsPurchase>(DataConvert.ToString(myModel.EntryGridString));
            if (gridData.Count < 1)
                throw new Exception(AppMember.AppText["NeedMoreThanOneAssets"]);
            foreach (AssetsPurchase assetsPurchase in gridData)
            {
                AddDetail(assetsPurchase, pkValue, sysUser, viewTitle, updateType);
            }
            if (formMode == "reapply")
                InitFirstApproveTask("AssetsPurchase", "assetsPurchaseId", assetsPurchaseId, viewTitle, formMode, sysUser.UserId);
            return 1;
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsPurchaseId", pkValue);
            string sql = @"select * from AssetsPurchase where assetsPurchaseId=@assetsPurchaseId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsPurchase";
            dt.Rows[0].Delete();
            DbUpdate.Update(dt);
            DeleteDetail(pkValue, sysUser, viewTitle);
            DeleteApproveData("AssetsPurchase", pkValue, sysUser.UserId);
            return 1;
        }

        protected int AddDetail(AssetsPurchase assetsPurchase, string assetsPurchaseId, UserInfo sysUser, string viewTitle, string updateType)
        {
            string sql = @"select * from AssetsPurchaseDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsPurchaseDetail";
            DataRow dr = dt.NewRow();
            string assetsPurchaseDetailId = IdGenerator.GetMaxId(dt.TableName);
            dr["assetsPurchaseDetailId"] = assetsPurchaseDetailId;
            dr["assetsPurchaseId"] = assetsPurchaseId;
            dr["assetsName"] = assetsPurchase.assetsName;
            dr["departmentId"] = assetsPurchase.departmentId;
            dr["storeSiteId"] = assetsPurchase.storeSiteId;
            dr["usePeople"] = assetsPurchase.usePeople;
            dr["keeper"] = assetsPurchase.keeper;
            dr["assetsValue"] = DataConvert.ToDBObject(assetsPurchase.assetsValue);
            dr["hasFixed"] = "N";
            dr["remark"] = assetsPurchase.remark;
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected int DeleteDetail(string pkValue, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsPurchaseId", pkValue);
            string sql = @"select * from AssetsPurchaseDetail where assetsPurchaseId=@assetsPurchaseId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsPurchaseDetail";
            foreach (DataRow dr in dt.Rows)
            {
                dr.Delete();
            }
            return DbUpdate.Update(dt);
        }

        public override int DealEndApprove(string approvePkValue, UserInfo sysUser, string viewTitle)
        {
            //Dictionary<string, object> paras = new Dictionary<string, object>();
            //paras.Add("assetsPurchaseId", approvePkValue);
            //string sql = @"select * from AssetsPurchaseDetail where assetsPurchaseId=@assetsPurchaseId";
            //DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            //foreach (DataRow dr in dtAssets.Rows)
            //{
            //    UpdateAssetsState(DataConvert.ToString(dr["assetsId"]), "R", sysUser.UserId, viewTitle);
            //}
            return 1;
        }

        public bool HadApprove(string assetsPurchaseId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsPurchaseId", assetsPurchaseId);
            string sql = @"select count(1) cnt from AssetsPurchase where assetsPurchaseId=@assetsPurchaseId and approveState='E'";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            if (DataConvert.ToInt32(dt.Rows[0]["cnt"]) > 0)
            {
                return true;
            }
            else
                return false;
        }

    }
}

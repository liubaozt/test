using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.AssetsBusiness.Models.AssetsSell;
using BaseCommon.Models;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsSell
    {
        public string AssetsId { get; set; }
        public string SellCompany { get; set; }
        public string SellAmount { get; set; }
        public string Remark { get; set; }
        public string ApproveState { get; set; }
        public string CreateId { get; set; }
        public string CreateTime { get; set; }
    }

    public class AssetsSellRepository : ApproveMasterRepository
    {

        public AssetsSellRepository()
        {
            DefaulteGridSortField = "AssetsSellNo";
            MasterTable = "AssetsSell";
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
            if (DataConvert.ToString(model.AssetsSellNo) != "")
            {
                wcd.Sql += @" and AssetsSell.assetsSellNo like '%'+@assetsSellNo+'%'";
                wcd.DBPara.Add("assetsSellNo", model.AssetsSellNo);
            }
            if (DataConvert.ToString(model.AssetsSellName) != "")
            {
                wcd.Sql += @" and AssetsSell.assetsSellName like '%'+@assetsSellName+'%'";
                wcd.DBPara.Add("assetsSellName", model.AssetsSellName);
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
                     where AppApprove.tableName='AssetsSell' and AppApprove.approveState='O'
                      and AssetsSell.AssetsSellId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(acondition.ListMode) == "reapply")
                {
                    lsql = @" where AssetsSell.createId=@approver and AssetsSell.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select  AssetsSell.AssetsSellId AssetsSellId,
                        AssetsSell.AssetsSellNo AssetsSellNo,
                        AssetsSell.AssetsSellName AssetsSellName,
                        (select userName from AppUser where AssetsSell.createId=AppUser.userId) createId,
                        AssetsSell.createTime createTime ,
                        (select userName from AppUser where AssetsSell.updateId=AppUser.userId) updateId,
                        AssetsSell.updateTime updateTime ,
                        AssetsSell.updatePro updatePro
                from AssetsSell  {0}  ", lsql);
            return subViewSql;
        }


        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            model.ApproveTableName = "AssetsSell";
            model.ApprovePkField = "assetsSellId";
            if (formMode != "new" && formMode != "new2")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("AssetsSellId", primaryKey);
                string sql = @"select * from AssetsSell where AssetsSellId=@AssetsSellId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.AssetsSellId = primaryKey;
                model.AssetsSellNo = DataConvert.ToString(dr["assetsSellNo"]);
                model.AssetsSellName = DataConvert.ToString(dr["assetsSellName"]);
                model.SellDate = DataConvert.ToDateTime(dr["sellDate"]);
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
                if (!paras.ContainsKey("AssetsSellId"))
                    paras.Add("AssetsSellId", primaryKey);
                string sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                    AssetsSellDetail.sellCompany sellCompany,
                    AssetsSellDetail.sellAmount sellAmount,
                    AssetsSellDetail.remark Remark,
                 AssetsSellDetail.approveState ApproveState,
                AssetsSellDetail.createId CreateId,
                AssetsSellDetail.createTime CreateTime
                from AssetsSellDetail,Assets where AssetsSellDetail.assetsId=Assets.assetsId and AssetsSellDetail.AssetsSellId=@AssetsSellId  ");
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                return dtGrid;
            }
            
        }

        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsSell where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsSell";
            DataRow dr = dt.NewRow();
            string assetsSellId = IdGenerator.GetMaxId(dt.TableName);
            int retApprove = InitFirstApproveTask("AssetsSell", "AssetsSellId", assetsSellId, viewTitle, "", sysUser.UserId);
            EntryModel myModel = model as EntryModel;
            dr["assetsSellNo"] = myModel.AssetsSellNo;
            dr["assetsSellName"] = myModel.AssetsSellName;
            dr["sellDate"] = DataConvert.ToDBObject(myModel.SellDate);
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            string updateType = "Add";
            if (retApprove != 0)
            {
                dr["approveState"] = "O";
                updateType = "ApproveAdd";
            }
            dr["AssetsSellId"] = assetsSellId;
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);

            List<AssetsSell> gridData = JsonHelper.JSONStringToList<AssetsSell>(DataConvert.ToString(myModel.EntryGridString));
            foreach (AssetsSell assetsSell in gridData)
            {
                AddDetail(assetsSell, assetsSellId, sysUser, viewTitle, updateType);
                if (retApprove != 0)
                {
                    UpdateAssetsState(assetsSell.AssetsId, "XI", sysUser.UserId, viewTitle);
                }
            }

            return 1;
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsSellId", pkValue);
            string sql = @"select * from AssetsSell where AssetsSellId=@AssetsSellId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsSell";
            string AssetsSellId = DataConvert.ToString(dt.Rows[0]["AssetsSellId"]);
            EntryModel myModel = model as EntryModel;
            dt.Rows[0]["assetsSellNo"] = myModel.AssetsSellNo;
            dt.Rows[0]["assetsSellName"] = myModel.AssetsSellName;
            dt.Rows[0]["sellDate"] = DataConvert.ToDBObject(myModel.SellDate);
            string updateType = "Modified";
            if (formMode == "reapply")
            {
                dt.Rows[0]["approveState"] = "O";
                updateType = "Reapply";
            }
            Update5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            DeleteDetail(pkValue,sysUser,viewTitle);
            List<AssetsSell> gridData = JsonHelper.JSONStringToList<AssetsSell>(DataConvert.ToString(myModel.EntryGridString));
            foreach (AssetsSell AssetsSell in gridData)
            {
                AddDetail(AssetsSell, pkValue, sysUser, viewTitle, updateType);
            }
            if (formMode == "reapply")
                InitFirstApproveTask("AssetsSell", "AssetsSellId", AssetsSellId, viewTitle, formMode, sysUser.UserId);
            return 1;
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsSellId", pkValue);
            string sql = @"select * from AssetsSell where AssetsSellId=@AssetsSellId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsSell";
            dt.Rows[0].Delete();
            DbUpdate.Update(dt);
            DeleteDetail(pkValue, sysUser, viewTitle);
            DeleteApproveData("AssetsSell", pkValue, sysUser.UserId);
            return 1;
        }

        protected int AddDetail(AssetsSell AssetsSell, string AssetsSellId, UserInfo sysUser, string viewTitle, string updateType)
        {
            string sql = @"select * from AssetsSellDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsSellDetail";
            DataRow dr = dt.NewRow();
            dr["AssetsSellId"] = AssetsSellId;
            dr["assetsId"] = AssetsSell.AssetsId;
            dr["remark"] = AssetsSell.Remark;
            dr["sellCompany"] = AssetsSell.SellCompany;
            dr["sellAmount"] = AssetsSell.SellAmount;
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            if (DataConvert.ToString(AssetsSell.CreateId) != "")
            {
                if (updateType == "Reapply")
                    dr["approveState"] = "O";
                else
                    dr["approveState"] = AssetsSell.ApproveState;
                dr["createId"] = AssetsSell.CreateId;
                dr["createTime"] = AssetsSell.CreateTime;
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
            paras.Add("AssetsSellId", pkValue);
            string sql = @"select * from AssetsSellDetail where AssetsSellId=@AssetsSellId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsSellDetail";
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
            paras.Add("assetsSellId", approvePkValue);
            string sql = @"select * from AssetsSellDetail where assetsSellId=@assetsSellId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            foreach (DataRow dr in dtAssets.Rows)
            {
                UpdateAssetsState(DataConvert.ToString(dr["assetsId"]), "A", sysUser.UserId, viewTitle);
            }
            return 1;
        }


    }
}

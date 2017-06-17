using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.AssetsBusiness.Models.AssetsSplit;
using BaseCommon.Models;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsSplit
    {
        public string AssetsId { get; set; }
        public string AssetsNo { get; set; }
        public string AssetsName { get; set; }
        public string AssetsValue { get; set; }
        public string Remark { get; set; }
        public string OriginalAssetsId { get; set; }
        public string ApproveState { get; set; }
        public string CreateId { get; set; }
        public string CreateTime { get; set; }
    }

    public class AssetsSplitRepository : ApproveMasterRepository
    {

        public AssetsSplitRepository()
        {
            DefaulteGridSortField = "AssetsSplitNo";
            MasterTable = "AssetsSplit";
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
            if (DataConvert.ToString(model.AssetsSplitNo) != "")
            {
                wcd.Sql += @" and AssetsSplit.assetsSplitNo like '%'+@assetsSplitNo+'%'";
                wcd.DBPara.Add("assetsSplitNo", model.AssetsSplitNo);
            }
            if (DataConvert.ToString(model.AssetsSplitName) != "")
            {
                wcd.Sql += @" and AssetsSplit.assetsSplitName like '%'+@assetsSplitName+'%'";
                wcd.DBPara.Add("assetsSplitName", model.AssetsSplitName);
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
                     where AppApprove.tableName='AssetsSplit' and AppApprove.approveState='O'
                      and AssetsSplit.AssetsSplitId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(acondition.ListMode) == "reapply")
                {
                    lsql = @" where AssetsSplit.createId=@approver and AssetsSplit.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select AssetsSplit.AssetsSplitId AssetsSplitId,
                        AssetsSplit.AssetsSplitNo AssetsSplitNo,
                        AssetsSplit.AssetsSplitName AssetsSplitName,
                        (select userName from AppUser where AssetsSplit.createId=AppUser.userId) createId,
                        AssetsSplit.createTime createTime ,
                        (select userName from AppUser where AssetsSplit.updateId=AppUser.userId) updateId,
                        AssetsSplit.updateTime updateTime ,
                        AssetsSplit.updatePro updatePro
                from AssetsSplit  {0}  ",  lsql);
            return subViewSql;
        }

        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            model.ApproveTableName = "AssetsSplit";
            model.ApprovePkField = "assetsSplitId";
            if (formMode != "new" && formMode != "new2")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("AssetsSplitId", primaryKey);
                string sql = @"select * from AssetsSplit where AssetsSplitId=@AssetsSplitId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.AssetsSplitId = primaryKey;
                model.AssetsSplitNo = DataConvert.ToString(dr["assetsSplitNo"]);
                model.AssetsSplitName = DataConvert.ToString(dr["assetsSplitName"]);
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
                if (!paras.ContainsKey("AssetsSplitId"))
                    paras.Add("AssetsSplitId", primaryKey);
                string sql = string.Format(@"select  AssetsSplitDetail.newAssetsId assetsId,
                        AssetsSplitDetail.assetsNo assetsNo,
                        AssetsSplitDetail.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                    AssetsSplitDetail.assetsValue assetsValue,
                    AssetsSplitDetail.remark Remark,
                    AssetsSplitDetail.originalAssetsId originalAssetsId,
                AssetsSplitDetail.approveState ApproveState,
                AssetsSplitDetail.createId CreateId,
                AssetsSplitDetail.createTime CreateTime
                from AssetsSplitDetail left join Assets on AssetsSplitDetail.originalAssetsId=Assets.assetsId where  AssetsSplitDetail.AssetsSplitId=@AssetsSplitId  ");
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                return dtGrid;
            }
            
        }

        public DataTable GetUpEntryGridDataTable(Dictionary<string, object> paras, string formMode, string primaryKey)
        {
            if (formMode == "new" || formMode == "new2")
            {
                return null;
            }
            else
            {
                if (!paras.ContainsKey("AssetsSplitId"))
                    paras.Add("AssetsSplitId", primaryKey);
                string sql = string.Format(@"select top 1 Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                    Assets.assetsValue assetsValue,
                    AssetsSplitDetail.remark Remark,
                    Assets.assetsId OriginalAssetsId,
                AssetsSplitDetail.approveState ApproveState,
                AssetsSplitDetail.createId CreateId,
                AssetsSplitDetail.createTime CreateTime
                from AssetsSplitDetail,Assets where AssetsSplitDetail.originalAssetsId=Assets.assetsId and AssetsSplitDetail.AssetsSplitId=@AssetsSplitId  ");
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                return dtGrid;
            }
           
        }

        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsSplit where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsSplit";
            DataRow dr = dt.NewRow();
            string assetsSplitId = IdGenerator.GetMaxId(dt.TableName);
            int retApprove = InitFirstApproveTask("AssetsSplit", "AssetsSplitId", assetsSplitId, viewTitle, "", sysUser.UserId);
            EntryModel myModel = model as EntryModel;
            dr["assetsSplitNo"] = myModel.AssetsSplitNo;
            dr["assetsSplitName"] = myModel.AssetsSplitName;
            string updateType = "Add";
            if (retApprove != 0)
            {
                dr["approveState"] = "O";
                updateType = "ApproveAdd";
            }
            dr["AssetsSplitId"] = assetsSplitId;
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            List<AssetsSplit> gridData = JsonHelper.JSONStringToList<AssetsSplit>(DataConvert.ToString(myModel.EntryGridString));
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsId", gridData[0].OriginalAssetsId);
            sql = @"select * from Assets where assetsId=@assetsId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            int rowIndex = 0;
            foreach (AssetsSplit assetsSplit in gridData)
            {
                assetsSplit.AssetsId = IdGenerator.GetMaxId("Assets");
                AddDetail(assetsSplit, assetsSplitId, sysUser, viewTitle, updateType);
                if (retApprove == 0)
                {
                    if (rowIndex == 0)
                        UpdateAssetsState(assetsSplit.OriginalAssetsId, "S", sysUser.UserId, viewTitle);
                    AddNewAssets(assetsSplit, dtAssets, sysUser, viewTitle);
                }
                else
                {
                    if (rowIndex == 0)
                        UpdateAssetsState(assetsSplit.OriginalAssetsId, "SI", sysUser.UserId, viewTitle);
                }
                rowIndex += 1;
            }

            return 1;
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsSplitId", pkValue);
            string sql = @"select * from AssetsSplit where AssetsSplitId=@AssetsSplitId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsSplit";
            string AssetsSplitId = DataConvert.ToString(dt.Rows[0]["AssetsSplitId"]);
            EntryModel myModel = model as EntryModel;
            dt.Rows[0]["assetsSplitNo"] = myModel.AssetsSplitNo;
            dt.Rows[0]["assetsSplitName"] = myModel.AssetsSplitName;
            string updateType = "Modified";
            if (formMode == "reapply")
            {
                dt.Rows[0]["approveState"] = "O";
                updateType = "Reapply";
            }
            Update5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            DeleteDetail(pkValue,sysUser,viewTitle);
            List<AssetsSplit> gridData = JsonHelper.JSONStringToList<AssetsSplit>(DataConvert.ToString(myModel.EntryGridString));
            foreach (AssetsSplit AssetsSplit in gridData)
            {
                AddDetail(AssetsSplit, pkValue, sysUser, viewTitle, updateType);
            }
            if (formMode == "reapply")
                InitFirstApproveTask("AssetsSplit", "AssetsSplitId", AssetsSplitId, viewTitle, formMode, sysUser.UserId);
            return 1;
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsSplitId", pkValue);
            string sql = @"select * from AssetsSplit where AssetsSplitId=@AssetsSplitId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsSplit";
            dt.Rows[0].Delete();
            DbUpdate.Update(dt);
            DeleteDetail(pkValue, sysUser, viewTitle);
            DeleteApproveData("AssetsSplit", pkValue, sysUser.UserId);
            return 1;
        }

        protected int AddDetail(AssetsSplit AssetsSplit, string AssetsSplitId, UserInfo sysUser, string viewTitle, string updateType)
        {
            string sql = @"select * from AssetsSplitDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsSplitDetail";
            DataRow dr = dt.NewRow();
            dr["AssetsSplitId"] = AssetsSplitId;
            dr["newAssetsId"] = AssetsSplit.AssetsId;
            dr["assetsNo"] = AssetsSplit.AssetsNo;
            dr["assetsName"] = AssetsSplit.AssetsName;
            dr["assetsValue"] = AssetsSplit.AssetsValue;
            dr["remark"] = AssetsSplit.Remark;
            dr["originalAssetsId"] = AssetsSplit.OriginalAssetsId;
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            if (DataConvert.ToString(AssetsSplit.CreateId) != "")
            {
                if (updateType == "Reapply")
                    dr["approveState"] = "O";
                else
                    dr["approveState"] = AssetsSplit.ApproveState;
                dr["createId"] = AssetsSplit.CreateId;
                dr["createTime"] = AssetsSplit.CreateTime;
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
            paras.Add("AssetsSplitId", pkValue);
            string sql = @"select * from AssetsSplitDetail where AssetsSplitId=@AssetsSplitId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsSplitDetail";
            foreach (DataRow dr in dt.Rows)
            {
                UpdateAssetsState(DataConvert.ToString(dr["assetsId"]), "A", sysUser.UserId, viewTitle);
                dr.Delete();
            }
            return DbUpdate.Update(dt);
        }

        //protected int UpdateOriginalAssets(AssetsSplit AssetsSplit, DataTable dtAssets, string sysUser, string viewTitle)
        //{
        //    DataTable dtNew = dtAssets.Clone();
        //    foreach (DataRow drNew in dtAssets.Rows)
        //    {
        //        dtNew.ImportRow(drNew);
        //    }
        //    dtNew.TableName = "Assets";
        //    dtNew.Rows[0]["assetsState"] = "S";
        //    Update5Field(dtNew, sysUser, viewTitle);
        //    return dbUpdate.Update(dtNew);
        //}

        protected int AddNewAssets(AssetsSplit AssetsSplit, DataTable dtAssets, UserInfo sysUser, string viewTitle)
        {
            DataTable dtNew = dtAssets.Clone();
            DataRow drNew = dtNew.NewRow();
            for (int i = 0; i < dtAssets.Columns.Count; i++)
            {
                drNew[i] = dtAssets.Rows[0][i];
            }
            double assetsValueOri=DataConvert.ToDouble( dtAssets.Rows[0]["assetsValue"]);
            double assetsNetValueOri=DataConvert.ToDouble( dtAssets.Rows[0]["assetsNetValue"]);
            drNew["assetsId"] = AssetsSplit.AssetsId;
            drNew["assetsNo"] = AssetsSplit.AssetsNo;
            drNew["assetsName"] = AssetsSplit.AssetsName;
            drNew["assetsValue"] = AssetsSplit.AssetsValue;
            drNew["assetsNetValue"] = assetsValueOri == 0 ? 0 : DataConvert.ToDouble(AssetsSplit.AssetsValue) * assetsNetValueOri / assetsValueOri;
            drNew["remark"] = AssetsSplit.Remark;
            drNew["assetsState"] = "A";
            drNew["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            drNew["addDate"] = IdGenerator.GetServerDate();
            dtNew.TableName = "Assets";
            dtNew.Rows.Add(drNew);
            Create5Field(dtNew, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dtNew);
        }

        public override int DealEndApprove(string approvePkValue, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsSplitId", approvePkValue);
            string sql = @"select * from AssetsSplitDetail where assetsSplitId=@assetsSplitId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            int rowIndex = 0;
            foreach (DataRow dr in dt.Rows)
            {
                AssetsSplit assetsSplit = new AssetsSplit();
                assetsSplit.AssetsId = DataConvert.ToString(dr["newAssetsId"]);
                assetsSplit.AssetsNo = DataConvert.ToString(dr["assetsNo"]);
                assetsSplit.AssetsName = DataConvert.ToString(dr["assetsName"]);
                assetsSplit.AssetsValue = DataConvert.ToString(dr["assetsValue"]);
                assetsSplit.OriginalAssetsId = DataConvert.ToString(dr["originalAssetsId"]);
                paras.Clear();
                paras.Add("assetsId", assetsSplit.OriginalAssetsId);
                sql = @"select * from Assets where assetsId=@assetsId";
                DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
                if (rowIndex == 0)
                    UpdateAssetsState(assetsSplit.OriginalAssetsId, "S", sysUser.UserId, viewTitle);
                AddNewAssets(assetsSplit, dtAssets, sysUser, viewTitle);
                rowIndex++;
            }
            return 1;
        }


    }
}

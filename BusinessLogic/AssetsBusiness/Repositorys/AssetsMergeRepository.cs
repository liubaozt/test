using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.AssetsBusiness.Models.AssetsMerge;
using BaseCommon.Models;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsMerge
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

    public class AssetsMergeRepository : ApproveMasterRepository
    {

        public AssetsMergeRepository()
        {
            DefaulteGridSortField = "AssetsMergeNo";
            MasterTable = "AssetsMerge";
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
            if (DataConvert.ToString(model.AssetsMergeNo) != "")
            {
                wcd.Sql += @" and AssetsMerge.assetsMergeNo like '%'+@assetsMergeNo+'%'";
                wcd.DBPara.Add("assetsMergeNo", model.AssetsMergeNo);
            }
            if (DataConvert.ToString(model.AssetsMergeName) != "")
            {
                wcd.Sql += @" and AssetsMerge.assetsMergeName like '%'+@assetsMergeName+'%'";
                wcd.DBPara.Add("assetsMergeName", model.AssetsMergeName);
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
                     where AppApprove.tableName='AssetsMerge' and AppApprove.approveState='O'
                      and AssetsMerge.AssetsMergeId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(acondition.ListMode) == "reapply")
                {
                    lsql = @" where AssetsMerge.createId=@approver and AssetsMerge.approveState='R' ";
                }
            }
            string subViewSql = string.Format(@"select  AssetsMerge.AssetsMergeId AssetsMergeId,
                        AssetsMerge.AssetsMergeNo AssetsMergeNo,
                        AssetsMerge.AssetsMergeName AssetsMergeName,
                        (select userName from AppUser where AssetsMerge.createId=AppUser.userId) createId,
                        AssetsMerge.createTime createTime ,
                        (select userName from AppUser where AssetsMerge.updateId=AppUser.userId) updateId,
                        AssetsMerge.updateTime updateTime ,
                        AssetsMerge.updatePro updatePro
                from AssetsMerge {0} ", lsql);
            return subViewSql;
        }

        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            model.ApproveTableName = "AssetsMerge";
            model.ApprovePkField = "assetsMergeId";
            if (formMode != "new" && formMode != "new2")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("AssetsMergeId", primaryKey);
                string sql = @"select * from AssetsMerge where AssetsMergeId=@AssetsMergeId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.AssetsMergeId = primaryKey;
                model.AssetsMergeNo = DataConvert.ToString(dr["assetsMergeNo"]);
                model.AssetsMergeName = DataConvert.ToString(dr["assetsMergeName"]);
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
                if (!paras.ContainsKey("AssetsMergeId"))
                    paras.Add("AssetsMergeId", primaryKey);
                string sql = string.Format(@"select top 1  AssetsMergeDetail.newAssetsId assetsId,
                        AssetsMergeDetail.assetsNo assetsNo,
                        AssetsMergeDetail.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                    AssetsMergeDetail.assetsValue assetsValue,
                    AssetsMergeDetail.remark Remark,
                    AssetsMergeDetail.originalAssetsId originalAssetsId,
                AssetsMergeDetail.approveState ApproveState,
                AssetsMergeDetail.createId CreateId,
                AssetsMergeDetail.createTime CreateTime
                from AssetsMergeDetail left join Assets on AssetsMergeDetail.originalAssetsId=Assets.assetsId where  AssetsMergeDetail.AssetsMergeId=@AssetsMergeId  ");
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
                if (!paras.ContainsKey("AssetsMergeId"))
                    paras.Add("AssetsMergeId", primaryKey);
                string sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                    Assets.assetsValue assetsValue,
                    Assets.remark Remark,
                    AssetsMergeDetail.originalAssetsId originalAssetsId,
                AssetsMergeDetail.approveState ApproveState,
                AssetsMergeDetail.createId CreateId,
                AssetsMergeDetail.createTime CreateTime
                from AssetsMergeDetail,Assets where AssetsMergeDetail.originalAssetsId=Assets.assetsId and AssetsMergeDetail.AssetsMergeId=@AssetsMergeId  ");
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                return dtGrid;
            }
            
        }

        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsMerge where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsMerge";
            DataRow dr = dt.NewRow();
            string assetsMergeId = IdGenerator.GetMaxId(dt.TableName);
            int retApprove = InitFirstApproveTask("AssetsMerge", "AssetsMergeId", assetsMergeId, viewTitle, "", sysUser.UserId);
            EntryModel myModel = model as EntryModel;
            dr["assetsMergeNo"] = myModel.AssetsMergeNo;
            dr["assetsMergeName"] = myModel.AssetsMergeName;
            string updateType = "Add";
            if (retApprove != 0)
            {
                dr["approveState"] = "O";
                updateType = "ApproveAdd";
            }
            dr["AssetsMergeId"] = assetsMergeId;
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            List<AssetsMerge> gridData = JsonHelper.JSONStringToList<AssetsMerge>(DataConvert.ToString(myModel.EntryGridString));
            AssetsMerge assetsMerge = gridData[0];
            assetsMerge.AssetsId = IdGenerator.GetMaxId("Assets");
            List<AssetsMerge> upGridData = JsonHelper.JSONStringToList<AssetsMerge>(DataConvert.ToString(myModel.UpEntryGridString));

            foreach (AssetsMerge originalAssetsMerge in upGridData)
            {
                AddDetail(assetsMerge, originalAssetsMerge, assetsMergeId, sysUser, viewTitle, updateType);
                if (retApprove == 0)
                {
                    UpdateAssetsState(originalAssetsMerge.AssetsId, "M", sysUser.UserId, viewTitle);
                }
                else
                {
                    UpdateAssetsState(originalAssetsMerge.AssetsId, "MI", sysUser.UserId, viewTitle);
                }
            }
            if (retApprove == 0)
                AddNewAssets(assetsMerge, sysUser, viewTitle);
            return 1;
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsMergeId", pkValue);
            string sql = @"select * from AssetsMerge where AssetsMergeId=@AssetsMergeId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsMerge";
            string AssetsMergeId = DataConvert.ToString(dt.Rows[0]["AssetsMergeId"]);
            EntryModel myModel = model as EntryModel;
            dt.Rows[0]["assetsMergeNo"] = myModel.AssetsMergeNo;
            dt.Rows[0]["assetsMergeName"] = myModel.AssetsMergeName;
            string updateType = "Modified";
            if (formMode == "reapply")
            {
                dt.Rows[0]["approveState"] = "O";
                updateType = "Reapply";
            }
            Update5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            DeleteDetail(pkValue,sysUser,viewTitle);
            List<AssetsMerge> gridData = JsonHelper.JSONStringToList<AssetsMerge>(DataConvert.ToString(myModel.EntryGridString));
            AssetsMerge AssetsMerge = gridData[0];
            List<AssetsMerge> upGridData = JsonHelper.JSONStringToList<AssetsMerge>(DataConvert.ToString(myModel.UpEntryGridString));
            foreach (AssetsMerge OriginalAssetsMerge in upGridData)
            {
                AddDetail(AssetsMerge, OriginalAssetsMerge, pkValue, sysUser, viewTitle, updateType);
            }
            if (formMode == "reapply")
                InitFirstApproveTask("AssetsMerge", "AssetsMergeId", AssetsMergeId, viewTitle, formMode, sysUser.UserId);
            return 1;
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsMergeId", pkValue);
            string sql = @"select * from AssetsMerge where AssetsMergeId=@AssetsMergeId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsMerge";
            dt.Rows[0].Delete();
            DbUpdate.Update(dt);
            DeleteDetail(pkValue, sysUser, viewTitle);
            DeleteApproveData("AssetsMerge", pkValue, sysUser.UserId);
            return 1;
        }

        protected int AddDetail(AssetsMerge AssetsMerge, AssetsMerge OriginalAssetsMerge, string AssetsMergeId, UserInfo sysUser, string viewTitle, string updateType)
        {
            string sql = @"select * from AssetsMergeDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsMergeDetail";
            DataRow dr = dt.NewRow();
            dr["AssetsMergeId"] = AssetsMergeId;
            dr["originalAssetsId"] = OriginalAssetsMerge.AssetsId;
            dr["newAssetsId"] = AssetsMerge.AssetsId;
            dr["assetsNo"] = AssetsMerge.AssetsNo;
            dr["assetsName"] = AssetsMerge.AssetsName;
            dr["assetsValue"] = AssetsMerge.AssetsValue;
            dr["remark"] = AssetsMerge.Remark;
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            if (DataConvert.ToString(OriginalAssetsMerge.CreateId) != "")
            {
                if (updateType == "Reapply")
                    dr["approveState"] = "O";
                else
                    dr["approveState"] = OriginalAssetsMerge.ApproveState;
                dr["createId"] = OriginalAssetsMerge.CreateId;
                dr["createTime"] = OriginalAssetsMerge.CreateTime;
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
            paras.Add("AssetsMergeId", pkValue);
            string sql = @"select * from AssetsMergeDetail where AssetsMergeId=@AssetsMergeId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsMergeDetail";
            foreach (DataRow dr in dt.Rows)
            {
                UpdateAssetsState(DataConvert.ToString(dr["assetsId"]), "A", sysUser.UserId, viewTitle);
                dr.Delete();
            }
            return DbUpdate.Update(dt);
        }


        protected int AddNewAssets(AssetsMerge assetsMerge, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsId", assetsMerge.OriginalAssetsId);
            string sql = @"select * from Assets where assetsId=@assetsId ";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dtAssets.TableName = "Assets";
            DataRow drNew = dtAssets.NewRow();
            for (int i = 0; i < dtAssets.Columns.Count; i++)
            {
                drNew[i] = dtAssets.Rows[0][i];
            }
            double assetsValueOri = DataConvert.ToDouble(dtAssets.Rows[0]["assetsValue"]);
            double assetsNetValueOri = DataConvert.ToDouble(dtAssets.Rows[0]["assetsNetValue"]);
            drNew["assetsId"] = assetsMerge.AssetsId;
            drNew["assetsNo"] = assetsMerge.AssetsNo;
            drNew["assetsName"] = assetsMerge.AssetsName;
            drNew["assetsValue"] = assetsMerge.AssetsValue;
            drNew["assetsNetValue"] = assetsValueOri == 0 ? 0 : DataConvert.ToDouble(assetsMerge.AssetsValue) * assetsNetValueOri / assetsValueOri;
            drNew["remark"] = assetsMerge.Remark;
            drNew["assetsState"] = "A";
            drNew["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            drNew["addDate"] = IdGenerator.GetServerDate();
            drNew["smDate"] = DBNull.Value;
            dtAssets.Rows.Add(drNew);
            Create5Field(dtAssets, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dtAssets);
        }

        public override int DealEndApprove(string approvePkValue, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsMergeId", approvePkValue);
            string sql = @"select * from AssetsMergeDetail where assetsMergeId=@assetsMergeId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            int rowIndex = 0;
            foreach (DataRow dr in dtAssets.Rows)
            {
                AssetsMerge assetsMerge = new AssetsMerge();
                assetsMerge.AssetsId = DataConvert.ToString(dr["newAssetsId"]);
                assetsMerge.AssetsNo = DataConvert.ToString(dr["assetsNo"]);
                assetsMerge.AssetsName = DataConvert.ToString(dr["assetsName"]);
                assetsMerge.AssetsValue = DataConvert.ToString(dr["assetsValue"]);
                assetsMerge.OriginalAssetsId = DataConvert.ToString(dr["originalAssetsId"]);
                if (rowIndex == 0)
                    AddNewAssets(assetsMerge, sysUser, viewTitle);
                UpdateAssetsState(assetsMerge.OriginalAssetsId, "M", sysUser.UserId, viewTitle);
                rowIndex++;
            }
            return 1;
        }



    }
}

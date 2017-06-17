using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.AssetsBusiness.Models.AssetsMaintain;
using BaseCommon.Models;


namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsMaintain
    {
        public string AssetsId { get; set; }
        public string MaintainCompany { get; set; }
        public string MaintainAmount { get; set; }
        public string MaintainActualCompany { get; set; }
        public string MaintainActualAmount { get; set; }
        public string MaintainActualDate { get; set; }
        public string IsFinished { get; set; }
        public string Remark { get; set; }
        public string ApproveState { get; set; }
        public string CreateId { get; set; }
        public string CreateTime { get; set; }
    }

    public class AssetsMaintainRepository : ApproveMasterRepository
    {

        public AssetsMaintainRepository()
        {
            DefaulteGridSortField = "AssetsMaintainNo";
            MasterTable = "AssetsMaintain";
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
            if (DataConvert.ToString(model.AssetsMaintainNo) != "")
            {
                wcd.Sql += @" and AssetsMaintain.assetsMaintainNo like '%'+@assetsMaintainNo+'%'";
                wcd.DBPara.Add("assetsMaintainNo", model.AssetsMaintainNo);
            }
            if (DataConvert.ToString(model.AssetsMaintainName) != "")
            {
                wcd.Sql += @" and AssetsMaintain.assetsMaintainName like '%'+@assetsMaintainName+'%'";
                wcd.DBPara.Add("assetsMaintainName", model.AssetsMaintainName);
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
                     where AppApprove.tableName='AssetsMaintain' and AppApprove.approveState='O'
                      and AssetsMaintain.AssetsMaintainId=AppApprove.refId and AppApprove.approver=@approver and isValid='Y'";
                }
                else if (DataConvert.ToString(acondition.ListMode) == "reapply")
                {
                    lsql = @" where AssetsMaintain.createId=@approver and AssetsMaintain.approveState='R' ";
                }
                else if (DataConvert.ToString(acondition.ListMode) == "actual")
                {
                    lsql = @" where AssetsMaintain.createId=@approver and (AssetsMaintain.approveState='E' or AssetsMaintain.approveState is null) ";
                }
            }
            string subViewSql = string.Format(@"select  AssetsMaintain.AssetsMaintainId AssetsMaintainId,
                        AssetsMaintain.AssetsMaintainNo AssetsMaintainNo,
                        AssetsMaintain.AssetsMaintainName AssetsMaintainName,
                        (select userName from AppUser where AssetsMaintain.createId=AppUser.userId) createId,
                        AssetsMaintain.createTime createTime ,
                        (select userName from AppUser where AssetsMaintain.updateId=AppUser.userId) updateId,
                        AssetsMaintain.updateTime updateTime ,
                        AssetsMaintain.updatePro updatePro
                from AssetsMaintain  {0}  ", lsql);
            return subViewSql;
        }


        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            model.ApproveTableName = "AssetsMaintain";
            model.ApprovePkField = "assetsMaintainId";
            if (formMode != "new" && formMode != "new2")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("assetsMaintainId", primaryKey);
                string sql = @"select AssetsMaintain.assetsMaintainNo,
                    AssetsMaintain.assetsMaintainName ,
                    AssetsMaintainDetail.maintainDate 
                    from AssetsMaintain,AssetsMaintainDetail
                       where AssetsMaintain.assetsMaintainId=AssetsMaintainDetail.assetsMaintainId 
                            and AssetsMaintain.assetsMaintainId=@assetsMaintainId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.AssetsMaintainId = primaryKey;
                model.AssetsMaintainNo = DataConvert.ToString(dr["assetsMaintainNo"]);
                model.AssetsMaintainName = DataConvert.ToString(dr["assetsMaintainName"]);
                model.MaintainDate = DataConvert.ToDateTime(dr["maintainDate"]);
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
                if (!paras.ContainsKey("AssetsMaintainId"))
                    paras.Add("AssetsMaintainId", primaryKey);
                string sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                    AssetsMaintainDetail.maintainCompany MaintainCompany,
                    AssetsMaintainDetail.maintainAmount MaintainAmount,
                    AssetsMaintainDetail.maintainActualCompany maintainActualCompany,
                    AssetsMaintainDetail.maintainActualAmount maintainActualAmount,
                   convert(nvarchar(100), AssetsMaintainDetail.maintainActualDate,23) maintainActualDate,
                    AssetsMaintainDetail.remark Remark,
                AssetsMaintainDetail.isFinished isFinished,
                AssetsMaintainDetail.approveState ApproveState,
                AssetsMaintainDetail.createId CreateId,
                AssetsMaintainDetail.createTime CreateTime
                from AssetsMaintainDetail,Assets where AssetsMaintainDetail.assetsId=Assets.assetsId and AssetsMaintainDetail.AssetsMaintainId=@AssetsMaintainId  ");
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                return dtGrid;
            }
        }

        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsMaintain where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsMaintain";
            DataRow dr = dt.NewRow();
            string assetsMaintainId = IdGenerator.GetMaxId(dt.TableName);
            int retApprove = InitFirstApproveTask("AssetsMaintain", "AssetsMaintainId", assetsMaintainId, viewTitle, "", sysUser.UserId);
            EntryModel myModel = model as EntryModel;
            dr["assetsMaintainNo"] = myModel.AssetsMaintainNo;
            dr["assetsMaintainName"] = myModel.AssetsMaintainName;
            string updateType = "Add";
            if (retApprove != 0)
            {
                dr["approveState"] = "O";
                updateType = "ApproveAdd";
            }
            dr["AssetsMaintainId"] = assetsMaintainId;
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            List<AssetsMaintain> gridData = JsonHelper.JSONStringToList<AssetsMaintain>(DataConvert.ToString(myModel.EntryGridString));
            if (gridData.Count < 1)
                throw new Exception(AppMember.AppText["NeedMoreThanOneAssets"]);
            foreach (AssetsMaintain assetsMaintain in gridData)
            {
                AddDetail(assetsMaintain, myModel, assetsMaintainId, sysUser, viewTitle, updateType);
                if (retApprove != 0)
                {
                    UpdateAssetsState(assetsMaintain.AssetsId, "WI", sysUser.UserId, viewTitle);
                }
                else
                {
                    UpdateAssetsState(assetsMaintain.AssetsId, "W", sysUser.UserId, viewTitle);
                }
            }

            return 1;
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsMaintainId", pkValue);
            string sql = @"select * from AssetsMaintain where AssetsMaintainId=@AssetsMaintainId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsMaintain";
            string AssetsMaintainId = DataConvert.ToString(dt.Rows[0]["AssetsMaintainId"]);
            EntryModel myModel = model as EntryModel;
            dt.Rows[0]["assetsMaintainNo"] = myModel.AssetsMaintainNo;
            dt.Rows[0]["assetsMaintainName"] = myModel.AssetsMaintainName;
            string updateType = "Modified";
            if (formMode == "reapply")
            {
                dt.Rows[0]["approveState"] = "O";
                updateType = "Reapply";
            }
            Update5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            DeleteDetail(pkValue, sysUser, viewTitle);
            List<AssetsMaintain> gridData = JsonHelper.JSONStringToList<AssetsMaintain>(DataConvert.ToString(myModel.EntryGridString));
            if (gridData.Count < 1)
                throw new Exception(AppMember.AppText["NeedMoreThanOneAssets"]);
            foreach (AssetsMaintain AssetsMaintain in gridData)
            {
                AddDetail(AssetsMaintain, myModel, pkValue, sysUser, viewTitle, updateType);
                if (formMode == "actual")
                {
                    UpdateAssetsState(AssetsMaintain.AssetsId, "A", sysUser.UserId, viewTitle);
                }
            }
            if (formMode == "reapply")
                InitFirstApproveTask("AssetsMaintain", "AssetsMaintainId", AssetsMaintainId, viewTitle, formMode, sysUser.UserId);
            return 1;
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("AssetsMaintainId", pkValue);
            string sql = @"select * from AssetsMaintain where AssetsMaintainId=@AssetsMaintainId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            dt.TableName = "AssetsMaintain";
            dt.Rows[0].Delete();
            DbUpdate.Update(dt);
            DeleteDetail(pkValue, sysUser, viewTitle);
            DeleteApproveData("AssetsMaintain", pkValue, sysUser.UserId);
            return 1;
        }

        protected int AddDetail(AssetsMaintain AssetsMaintain, EntryModel model, string AssetsMaintainId, UserInfo sysUser, string viewTitle, string updateType)
        {
            string sql = @"select * from AssetsMaintainDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsMaintainDetail";
            DataRow dr = dt.NewRow();
            dr["AssetsMaintainId"] = AssetsMaintainId;
            dr["assetsId"] = AssetsMaintain.AssetsId;
            dr["remark"] = AssetsMaintain.Remark;
            dr["maintainCompany"] = AssetsMaintain.MaintainCompany;
            dr["maintainAmount"] = DataConvert.ToDBObject(AssetsMaintain.MaintainAmount);
            dr["maintainDate"] = DataConvert.ToDBObject(model.MaintainDate);
            dr["maintainActualCompany"] = AssetsMaintain.MaintainActualCompany;
            dr["maintainActualAmount"] = DataConvert.ToDBObject(AssetsMaintain.MaintainActualAmount);
            dr["maintainActualDate"] = DataConvert.ToDBObject(AssetsMaintain.MaintainActualDate);
            dr["isFinished"] = AssetsMaintain.IsFinished;
            dr["setBooksId"] = sysUser.MySetBooks.SetBooksId;
            dt.Rows.Add(dr);
            if (DataConvert.ToString(AssetsMaintain.CreateId) != "")
            {
                if (updateType == "Reapply")
                    dr["approveState"] = "O";
                else
                    dr["approveState"] = AssetsMaintain.ApproveState;
                dr["createId"] = AssetsMaintain.CreateId;
                dr["createTime"] = AssetsMaintain.CreateTime;
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
            paras.Add("AssetsMaintainId", pkValue);
            string sql = @"select * from AssetsMaintainDetail where AssetsMaintainId=@AssetsMaintainId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsMaintainDetail";
            foreach (DataRow dr in dt.Rows)
            {
                //UpdateAssetsState(DataConvert.ToString(dr["assetsId"]), "A", sysUser.UserId, viewTitle);
                dr.Delete();
            }
            return DbUpdate.Update(dt);
        }

        public override int DealEndApprove(string approvePkValue, UserInfo sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsMaintainId", approvePkValue);
            string sql = @"select * from AssetsMaintainDetail where assetsMaintainId=@assetsMaintainId";
            DataTable dtAssets = AppMember.DbHelper.GetDataSet(sql, paras, DbUpdate.cmd).Tables[0];
            foreach (DataRow dr in dtAssets.Rows)
            {
                UpdateAssetsState(DataConvert.ToString(dr["assetsId"]), "A", sysUser.UserId, viewTitle);
            }
            return 1;
        }

    }
}

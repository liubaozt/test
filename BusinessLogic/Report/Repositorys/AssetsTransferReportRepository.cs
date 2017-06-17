using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Report;
using BaseCommon.Basic;
using System.Data;
using BaseCommon.Data;
using BusinessLogic.Report.Models.AssetsTransferReport;

namespace BusinessLogic.Report.Repositorys
{
    public class AssetsTransferReportRepository : IQueryReport
    {


        public virtual DataTable GetReportGridDataTable(ListCondition condition)
        {
            int rowSize = condition.PageIndex * condition.PageRowNum; //子查询返回行数的尺寸
            string sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        AssetsTransfer.assetsTransferNo assetsTransferNo,
                        AssetsTransfer.assetsTransferName assetsTransferName,
                       (select departmentName from AppDepartment where AssetsTransferDetail.originalDepartmentId=AppDepartment.departmentId) originalDepartmentId,
                       (select storeSiteName from StoreSite where AssetsTransferDetail.originalStoreSiteId=StoreSite.storeSiteId) originalStoreSiteId,
                       (select userName from AppUser where AssetsTransferDetail.originalKeeper=AppUser.userId) originalKeeper,
                       (select userName from AppUser where AssetsTransferDetail.originalUsePeople=AppUser.userId) originalUsePeople,
                       (select departmentName from AppDepartment where AssetsTransferDetail.newDepartmentId=AppDepartment.departmentId) newDepartmentId,
                       (select storeSiteName from StoreSite where AssetsTransferDetail.newStoreSiteId=StoreSite.storeSiteId) newStoreSiteId,
                       (select userName from AppUser where AssetsTransferDetail.newKeeper=AppUser.userId) newKeeper,
                       (select userName from AppUser where AssetsTransferDetail.newUsePeople=AppUser.userId) newUsePeople,
                        AssetsTransferDetail.remark Remark
                from AssetsTransferDetail,Assets,AssetsTransfer where AssetsTransferDetail.assetsId=Assets.assetsId  and AssetsTransferDetail.assetsTransferId=AssetsTransfer.assetsTransferId {0} ", ListWhereSql(condition).Sql);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, ListWhereSql(condition).DBPara).Tables[0];
            return dtGrid;
        }

        protected  WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            EntryModel model = JsonHelper.Deserialize<EntryModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.AssetsTransferNo) != "")
            {
                wcd.Sql += @" and AssetsTransfer.assetsTransferNo  like '%'+@assetsTransferNo+'%'";
                wcd.DBPara.Add("assetsTransferNo", model.AssetsTransferNo);
            }
            if (DataConvert.ToString(model.AssetsTransferName) != "")
            {
                wcd.Sql += @" and AssetsTransfer.assetsTransferName  like '%'+@assetsTransferName+'%'";
                wcd.DBPara.Add("assetsTransferName", model.AssetsTransferName);
            }
            if (DataConvert.ToString(model.AssetsNo) != "")
            {
                wcd.Sql += @" and Assets.assetsNo  like '%'+@assetsNo+'%'";
                wcd.DBPara.Add("assetsNo", model.AssetsNo);
            }
            if (DataConvert.ToString(model.AssetsName) != "")
            {
                wcd.Sql += @" and Assets.assetsName  like '%'+@assetsName+'%'";
                wcd.DBPara.Add("assetsName", model.AssetsName);
            }
            if (DataConvert.ToString(model.TransferDate) != "")
            {
                wcd.Sql += @" and AssetsTransfer.transferDate  like '%'+@transferDate+'%'";
                wcd.DBPara.Add("transferDate", model.TransferDate);
            }
            return wcd;
        }



    }
}

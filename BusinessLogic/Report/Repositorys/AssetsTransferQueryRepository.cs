using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;
using System.Data;
using BaseCommon.Data;
using BusinessLogic.Report.Models.AssetsTransferQuery;

namespace BusinessLogic.Report.Repositorys
{
    public class AssetsTransferQueryRepository : IQuery
    {


        public virtual DataTable GetReportGridDataTable(ListCondition condition)
        {
            int rowSize = condition.PageIndex * condition.PageRowNum; //子查询返回行数的尺寸
            string sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        AssetsTransfer.assetsTransferNo assetsTransferNo,
                        AssetsTransfer.assetsTransferName assetsTransferName,
                      convert(nvarchar(100),  AssetsTransferDetail.transferDate,23) transferDate,
                       (select departmentName from AppDepartment where AssetsTransferDetail.originalDepartmentId=AppDepartment.departmentId) originalDepartmentId,
                       (select storeSiteName from StoreSite where AssetsTransferDetail.originalStoreSiteId=StoreSite.storeSiteId) originalStoreSiteId,
                       isnull((select userName from AppUser where AssetsTransferDetail.originalKeeper=AppUser.userId),AssetsTransferDetail.originalKeeper) originalKeeper,
                       isnull((select userName from AppUser where AssetsTransferDetail.originalUsePeople=AppUser.userId),AssetsTransferDetail.originalUsePeople) originalUsePeople,
                       (select departmentName from AppDepartment where AssetsTransferDetail.newDepartmentId=AppDepartment.departmentId) newDepartmentId,
                       (select storeSiteName from StoreSite where AssetsTransferDetail.newStoreSiteId=StoreSite.storeSiteId) newStoreSiteId,
                       isnull((select userName from AppUser where AssetsTransferDetail.newKeeper=AppUser.userId),AssetsTransferDetail.newKeeper) newKeeper,
                       isnull((select userName from AppUser where AssetsTransferDetail.newUsePeople=AppUser.userId),AssetsTransferDetail.newUsePeople) newUsePeople,
                        AssetsTransferDetail.remark Remark
                from AssetsTransferDetail,Assets,AssetsTransfer 
                where AssetsTransferDetail.assetsId=Assets.assetsId  
                and AssetsTransferDetail.assetsTransferId=AssetsTransfer.assetsTransferId 
                and (AssetsTransferDetail.approveState='E' or AssetsTransferDetail.approveState is null) 
                {0} {1} ", ListWhereSql(condition).Sql,
                                                                                                                                                                                                        " order by  AssetsTransfer.assetsTransferNo,Assets.assetsNo");
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
            if (DataConvert.ToString(model.TransferDate1) != "")
            {
                wcd.Sql += @" and AssetsTransferDetail.transferDate>=@transferDate1";
                wcd.DBPara.Add("transferDate1", DataConvert.ToString(model.TransferDate1)+" 00:00:00");
            }
            if (DataConvert.ToString(model.TransferDate2) != "")
            {
                wcd.Sql += @" and AssetsTransferDetail.transferDate<=@transferDate2";
                wcd.DBPara.Add("transferDate2", DataConvert.ToString(model.TransferDate2) + " 23:59:59");
            }
            return wcd;
        }



    }
}

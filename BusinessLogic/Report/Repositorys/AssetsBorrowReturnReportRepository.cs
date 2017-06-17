using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Report;
using BaseCommon.Basic;
using System.Data;
using BaseCommon.Data;
using BusinessLogic.Report.Models.AssetsBorrowReturnReport;

namespace BusinessLogic.Report.Repositorys
{
    public class AssetsBorrowReturnReportRepository : IQueryReport
    {


        public virtual DataTable GetReportGridDataTable(ListCondition condition)
        {
            int rowSize = condition.PageIndex * condition.PageRowNum; //子查询返回行数的尺寸
            string sql = string.Format(@"select T1.*,
                                    T2.assetsReturnNo,
                                    T2.assetsReturnName,
                                    T2.returnPeople,
                                    T2.returnDate
                                    from 
                                    (select AssetsBorrowDetail.assetsId,
                                    Assets.assetsNo,
                                    Assets.assetsName,
                                    AssetsBorrow.assetsBorrowNo,
                                    AssetsBorrow.assetsBorrowName,
                                    AssetsBorrowDetail.borrowPeople,
                                    AssetsBorrowDetail.borrowDepartmentId,
                                    AssetsBorrowDetail.borrowDate
                                    from AssetsBorrow,AssetsBorrowDetail,Assets
                                    where AssetsBorrow.assetsBorrowId=AssetsBorrowDetail.assetsBorrowId
                                    and AssetsBorrowDetail.assetsId=Assets.assetsId) T1
                                    left join 
                                    (select AssetsReturnDetail.assetsId,
                                    AssetsReturn.assetsReturnNo,
                                    AssetsReturn.assetsReturnName,
                                    AssetsReturnDetail.returnPeople,
                                    AssetsReturnDetail.returnDate
                                    from AssetsReturn,AssetsReturnDetail
                                    where AssetsReturn.assetsReturnId=AssetsReturnDetail.assetsReturnId) T2
                                    on T1.assetsId=T2.assetsId {0} ", ListWhereSql(condition).Sql);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, ListWhereSql(condition).DBPara).Tables[0];
            return dtGrid;
        }

        protected  WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            EntryModel model = JsonHelper.Deserialize<EntryModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.AssetsNo) != "")
            {
                wcd.Sql += @" and T1.assetsNo  like '%'+@assetsNo+'%'";
                wcd.DBPara.Add("assetsNo", model.AssetsNo);
            }
            if (DataConvert.ToString(model.AssetsName) != "")
            {
                wcd.Sql += @" and T1.assetsName  like '%'+@assetsName+'%'";
                wcd.DBPara.Add("assetsName", model.AssetsName);
            }
            return wcd;
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;
using System.Data;
using BaseCommon.Data;
using BusinessLogic.Report.Models.AssetsBorrowReturnQuery;

namespace BusinessLogic.Report.Repositorys
{
    public class AssetsBorrowReturnQueryRepository : IQuery
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
                                    (select userName from AppUser where AssetsBorrowDetail.borrowPeople=AppUser.userId) borrowPeople,
                                    (select departmentName from AppDepartment where AssetsBorrowDetail.borrowDepartmentId=AppDepartment.departmentId) borrowDepartmentId,
                                     convert(nvarchar(100), AssetsBorrowDetail.borrowDate,23) borrowDate,
                                      convert(nvarchar(100), AssetsBorrowDetail.planReturnDate ,23) planReturnDate
                                    from AssetsBorrow,AssetsBorrowDetail,Assets
                                    where AssetsBorrow.assetsBorrowId=AssetsBorrowDetail.assetsBorrowId
                                    and AssetsBorrowDetail.assetsId=Assets.assetsId
                                    and (AssetsBorrowDetail.approveState='E' or AssetsBorrowDetail.approveState is null) ) T1
                                    left join 
                                    (select AssetsReturnDetail.assetsId,
                                    AssetsReturn.assetsReturnNo,
                                    AssetsReturn.assetsReturnName,
                                    (select userName from AppUser where AssetsReturnDetail.returnPeople=AppUser.userId) returnPeople,
                                    AssetsReturnDetail.returnDate
                                    from AssetsReturn,AssetsReturnDetail
                                    where AssetsReturn.assetsReturnId=AssetsReturnDetail.assetsReturnId) T2
                                    on T1.assetsId=T2.assetsId where 1=1 {0} {1} ", ListWhereSql(condition).Sql,
                                                                        " order by T1.assetsBorrowNo,T1.assetsNo");
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
            if (DataConvert.ToString(model.BorrowDate1) != "")
            {
                wcd.Sql += @" and T1.borrowDate>=@borrowDate1";
                wcd.DBPara.Add("borrowDate1", DataConvert.ToString(model.BorrowDate1) + " 00:00:00");
            }
            if (DataConvert.ToString(model.BorrowDate2) != "")
            {
                wcd.Sql += @" and T1.borrowDate<=@borrowDate2";
                wcd.DBPara.Add("borrowDate2", DataConvert.ToString(model.BorrowDate2) + " 23:59:59");
            }
            return wcd;
        }



    }
}

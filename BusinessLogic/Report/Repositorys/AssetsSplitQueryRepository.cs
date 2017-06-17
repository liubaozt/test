using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;
using System.Data;
using BaseCommon.Data;
using BusinessLogic.Report.Models.AssetsSplitQuery;

namespace BusinessLogic.Report.Repositorys
{
    public class AssetsSplitQueryRepository : IQuery
    {


        public virtual DataTable GetReportGridDataTable(ListCondition condition)
        {
            int rowSize = condition.PageIndex * condition.PageRowNum; //子查询返回行数的尺寸
            string sql = string.Format(@"select AssetsSplitDetail.newAssetsId,
                                A1.assetsNo newAssetsNo,
                                A1.assetsName newAssetsName,
                                A1.assetsValue newAssetsValue,
                                AssetsSplit.assetsSplitNo,
                                AssetsSplit.assetsSplitName,
                                A2.assetsNo originalAssetsNo,
                                A2.assetsName originalAssetsName,
                                A2.assetsValue originalAssetsValue,
                                AssetsSplitDetail.originalAssetsId,
                                AssetsSplitDetail.remark
                                from AssetsSplitDetail,
                                AssetsSplit ,
                                Assets A1,
                                Assets A2
                                where AssetsSplitDetail.assetsSplitId=AssetsSplit.assetsSplitId 
                                and AssetsSplitDetail.newAssetsId=A1.assetsId
                                and AssetsSplitDetail.originalAssetsId=A2.assetsId  
                                  and (AssetsSplitDetail.approveState='E' or AssetsSplitDetail.approveState is null) 
                                {0} {1} ", ListWhereSql(condition).Sql, "order by AssetsSplit.assetsSplitNo,A1.assetsNo");
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, ListWhereSql(condition).DBPara).Tables[0];
            return dtGrid;
        }

        protected  WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            EntryModel model = JsonHelper.Deserialize<EntryModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.AssetsSplitNo) != "")
            {
                wcd.Sql += @" and AssetsSplit.assetsSplitNo  like '%'+@assetsSplitNo+'%'";
                wcd.DBPara.Add("assetsSplitNo", model.AssetsSplitNo);
            }
            if (DataConvert.ToString(model.AssetsSplitName) != "")
            {
                wcd.Sql += @" and AssetsSplit.assetsSplitName  like '%'+@assetsSplitName+'%'";
                wcd.DBPara.Add("assetsSplitName", model.AssetsSplitName);
            }
            if (DataConvert.ToString(model.AssetsNo) != "")
            {
                wcd.Sql += @" and A1.assetsNo  like '%'+@assetsNo+'%'";
                wcd.DBPara.Add("assetsNo", model.AssetsNo);
            }
            if (DataConvert.ToString(model.AssetsName) != "")
            {
                wcd.Sql += @" and A1.assetsName  like '%'+@assetsName+'%'";
                wcd.DBPara.Add("assetsName", model.AssetsName);
            }
            return wcd;
        }



    }
}

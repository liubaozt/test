using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Report;
using BaseCommon.Basic;
using System.Data;
using BaseCommon.Data;
using BusinessLogic.Report.Models.AssetsMergeReport;

namespace BusinessLogic.Report.Repositorys
{
    public class AssetsMergeReportRepository : IQueryReport
    {


        public virtual DataTable GetReportGridDataTable(ListCondition condition)
        {
            int rowSize = condition.PageIndex * condition.PageRowNum; //子查询返回行数的尺寸
            string sql = string.Format(@"select AssetsMergeDetail.newAssetsId,
                                A1.assetsNo newAssetsNo,
                                A1.assetsName newAssetsName,
                                A1.assetsValue newAssetsValue,
                                AssetsMerge.assetsMergeNo,
                                AssetsMerge.assetsMergeName,
                                A2.assetsNo originalAssetsNo,
                                A2.assetsName originalAssetsName,
                                AssetsMergeDetail.originalAssetsId,
                                AssetsMergeDetail.remark
                                from AssetsMergeDetail,
                                AssetsMerge ,
                                Assets A1,
                                Assets A2
                                where AssetsMergeDetail.assetsMergeId=AssetsMerge.assetsMergeId 
                                and AssetsMergeDetail.newAssetsId=A1.assetsId
                                and AssetsMergeDetail.originalAssetsId=A2.assetsId  {0} ", ListWhereSql(condition).Sql);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, ListWhereSql(condition).DBPara).Tables[0];
            return dtGrid;
        }

        protected  WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            EntryModel model = JsonHelper.Deserialize<EntryModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.AssetsMergeNo) != "")
            {
                wcd.Sql += @" and AssetsMerge.assetsMergeNo  like '%'+@assetsMergeNo+'%'";
                wcd.DBPara.Add("assetsMergeNo", model.AssetsMergeNo);
            }
            if (DataConvert.ToString(model.AssetsMergeName) != "")
            {
                wcd.Sql += @" and AssetsMerge.assetsMergeName  like '%'+@assetsMergeName+'%'";
                wcd.DBPara.Add("assetsMergeName", model.AssetsMergeName);
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

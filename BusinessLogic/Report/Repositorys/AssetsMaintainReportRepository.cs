using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Report;
using BaseCommon.Basic;
using System.Data;
using BaseCommon.Data;
using BusinessLogic.Report.Models.AssetsMaintainReport;

namespace BusinessLogic.Report.Repositorys
{
    public class AssetsMaintainReportRepository : IQueryReport
    {


        public virtual DataTable GetReportGridDataTable(ListCondition condition)
        {
            int rowSize = condition.PageIndex * condition.PageRowNum; //子查询返回行数的尺寸
            string sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        AssetsMaintain.assetsMaintainNo assetsMaintainNo,
                        AssetsMaintain.assetsMaintainName assetsMaintainName,
                        AssetsMaintain.maintainDate maintainDate,
                        AssetsMaintainDetail.maintainCompany maintainCompany,
                        AssetsMaintainDetail.maintainAmount maintainAmount,
                        AssetsMaintainDetail.remark Remark
                from AssetsMaintainDetail,Assets,AssetsMaintain where AssetsMaintainDetail.assetsId=Assets.assetsId  and AssetsMaintainDetail.assetsMaintainId=AssetsMaintain.assetsMaintainId {0} ", ListWhereSql(condition).Sql);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, ListWhereSql(condition).DBPara).Tables[0];
            return dtGrid;
        }

        protected  WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            EntryModel model = JsonHelper.Deserialize<EntryModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.AssetsMaintainNo) != "")
            {
                wcd.Sql += @" and AssetsMaintain.assetsMaintainNo  like '%'+@assetsMaintainNo+'%'";
                wcd.DBPara.Add("assetsMaintainNo", model.AssetsMaintainNo);
            }
            if (DataConvert.ToString(model.AssetsMaintainName) != "")
            {
                wcd.Sql += @" and AssetsMaintain.assetsMaintainName  like '%'+@assetsMaintainName+'%'";
                wcd.DBPara.Add("assetsMaintainName", model.AssetsMaintainName);
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
            if (DataConvert.ToString(model.MaintainDate) != "")
            {
                wcd.Sql += @" and AssetsMaintain.maintainDate  like '%'+@maintainDate+'%'";
                wcd.DBPara.Add("maintainDate", model.MaintainDate);
            }
           
            return wcd;
        }



    }
}

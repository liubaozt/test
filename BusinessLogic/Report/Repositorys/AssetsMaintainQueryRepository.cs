using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;
using System.Data;
using BaseCommon.Data;
using BusinessLogic.Report.Models.AssetsMaintainQuery;

namespace BusinessLogic.Report.Repositorys
{
    public class AssetsMaintainQueryRepository : IQuery
    {


        public virtual DataTable GetReportGridDataTable(ListCondition condition)
        {
            int rowSize = condition.PageIndex * condition.PageRowNum; //子查询返回行数的尺寸
            string sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        AssetsMaintain.assetsMaintainNo assetsMaintainNo,
                        AssetsMaintain.assetsMaintainName assetsMaintainName,
                        convert(nvarchar(100),  AssetsMaintainDetail.maintainDate  ,23) maintainDate,
                        AssetsMaintainDetail.maintainCompany maintainCompany,
                        AssetsMaintainDetail.maintainAmount maintainAmount,
                        AssetsMaintainDetail.maintainActualCompany maintainActualCompany,
                        AssetsMaintainDetail.maintainActualAmount maintainActualAmount,
                       convert(nvarchar(100),AssetsMaintainDetail.maintainActualDate  ,23) maintainActualDate,
                        AssetsMaintainDetail.remark Remark
                       from AssetsMaintainDetail,Assets,AssetsMaintain 
                        where AssetsMaintainDetail.assetsId=Assets.assetsId  
                        and AssetsMaintainDetail.assetsMaintainId=AssetsMaintain.assetsMaintainId 
                        and (AssetsMaintainDetail.approveState='E' or AssetsMaintainDetail.approveState is null or AssetsMaintainDetail.approveState='') 
                        {0} {1} ", ListWhereSql(condition).Sql,
                                                                                                                                                                                                    " order by  AssetsMaintain.assetsMaintainNo,Assets.assetsNo ");
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
            if (DataConvert.ToString(model.MaintainDate1) != "")
            {
                wcd.Sql += @" and AssetsMaintainDetail.maintainDate>=@maintainDate1";
                wcd.DBPara.Add("maintainDate1", DataConvert.ToString(model.MaintainDate1) + " 00:00:00");
            }
            if (DataConvert.ToString(model.MaintainDate2) != "")
            {
                wcd.Sql += @" and AssetsMaintainDetail.maintainDate<=@maintainDate2";
                wcd.DBPara.Add("maintainDate2", DataConvert.ToString(model.MaintainDate2) + " 23:59:59");
            }
            return wcd;
        }



    }
}

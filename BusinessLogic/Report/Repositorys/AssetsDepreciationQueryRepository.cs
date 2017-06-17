using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;
using System.Data;
using BaseCommon.Data;
using BusinessLogic.Report.Models.AssetsDepreciationQuery;

namespace BusinessLogic.Report.Repositorys
{
    public class AssetsDepreciationQueryRepository : IQuery
    {


        public virtual DataTable GetReportGridDataTable(ListCondition condition)
        {
            int rowSize = condition.PageIndex * condition.PageRowNum; //子查询返回行数的尺寸
            string sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        Assets.assetsValue assetsValue,
                        AssetsDepreciation.assetsNetValue assetsNetValue,
                        Assets.durableYears durableYears,
                        AssetsDepreciation.remainMonth remainMonth,
                       (select fiscalYearName from FiscalYear where AssetsDepreciation.fiscalYearId=FiscalYear.fiscalYearId) fiscalYearId,
                       (select fiscalPeriodName from FiscalPeriod where AssetsDepreciation.fiscalPeriodId=FiscalPeriod.fiscalPeriodId) fiscalPeriodId,
                        AssetsDepreciation.depreciationAmount depreciationAmount
                from Assets,AssetsDepreciation where AssetsDepreciation.assetsId=Assets.assetsId  {0} {1} ", ListWhereSql(condition).Sql, " order by  Assets.assetsNo");
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
                wcd.Sql += @" and Assets.assetsNo  like '%'+@assetsNo+'%'";
                wcd.DBPara.Add("assetsNo", model.AssetsNo);
            }
            if (DataConvert.ToString(model.AssetsName) != "")
            {
                wcd.Sql += @" and Assets.assetsName  like '%'+@assetsName+'%'";
                wcd.DBPara.Add("assetsName", model.AssetsName);
            }
            if (DataConvert.ToString(model.FiscalYearId) != "")
            {
                wcd.Sql += @" and AssetsDepreciation.fiscalYearId=@fiscalYearId";
                wcd.DBPara.Add("fiscalYearId", model.FiscalYearId);
            }
            if (DataConvert.ToString(model.FiscalPeriodId) != "")
            {
                wcd.Sql += @" and AssetsDepreciation.fiscalPeriodId=@fiscalPeriodId";
                wcd.DBPara.Add("fiscalPeriodId", model.FiscalPeriodId);
            }
            return wcd;
        }



    }
}

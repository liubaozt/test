using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Report;
using BaseCommon.Basic;
using System.Data;
using BaseCommon.Data;
using BusinessLogic.Report.Models.AssetsDepreciationReport;

namespace BusinessLogic.Report.Repositorys
{
    public class AssetsDepreciationReportRepository : IQueryReport
    {


        public virtual DataTable GetReportGridDataTable(ListCondition condition)
        {
            int rowSize = condition.PageIndex * condition.PageRowNum; //子查询返回行数的尺寸
            string sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        Assets.assetsValue assetsValue,
                        Assets.assetsNetValue assetsNetValue,
                        Assets.durableYears durableYears,
                        Assets.remainMonth remainMonth,
                       (select fiscalYearName from FiscalYear where AssetsDepreciation.fiscalYearId=FiscalYear.fiscalYearId) fiscalYearId,
                       (select fiscalPeriodName from FiscalPeriod where AssetsDepreciation.fiscalPeriodId=FiscalPeriod.fiscalPeriodId) fiscalPeriodId,
                        AssetsDepreciation.depreciationMonth depreciationMonth
                from Assets,AssetsDepreciation where AssetsDepreciation.assetsId=Assets.assetsId  {0} ", ListWhereSql(condition).Sql);
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
            return wcd;
        }



    }
}

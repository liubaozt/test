using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Report;
using BaseCommon.Basic;
using System.Data;
using BaseCommon.Data;
using BusinessLogic.Report.Models.AssetsClassChangeReport;

namespace BusinessLogic.Report.Repositorys
{
    public class AssetsClassChangeReportRepository : IQuery
    {
        public virtual DataTable GetReportGridDataTable(ListCondition condition)
        {
            int rowSize = condition.PageIndex * condition.PageRowNum; //子查询返回行数的尺寸
            string sql = string.Format(@"select MonthAssetsClassRecord.*,AssetsClass.assetsClassPath,AssetsClass.assetsClassName 
            from MonthAssetsClassRecord,AssetsClass 
            where  MonthAssetsClassRecord.assetsClassId=AssetsClass.assetsClassId {0}
            order by AssetsClass.assetsClassNo ", ListWhereSql(condition).Sql);
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, ListWhereSql(condition).DBPara).Tables[0];
            string sqlRpt = string.Format(@"select distinct AssetsClass.assetsClassId, AssetsClass.assetsClassNo,
            AssetsClass.assetsClassName ,AssetsClass.assetsClassPath
            from MonthAssetsClassRecord,AssetsClass 
            where  MonthAssetsClassRecord.assetsClassId=AssetsClass.assetsClassId {0}
            order by AssetsClass.assetsClassNo ", ListWhereSql(condition).Sql);
            DataTable dtRpt = AppMember.DbHelper.GetDataSet(sqlRpt, ListWhereSql(condition).DBPara).Tables[0];
            string sqlView = @"select * from MonthAssetsClassRecord where 1<>1 ";
            DataTable dtView = AppMember.DbHelper.GetDataSet(sqlView).Tables[0];
            foreach (DataRow dr in dtRpt.Rows)
            {
                DataRow drHeader = dtView.NewRow();
                drHeader["assetsClassId"] = DataConvert.ToString(dr["assetsClassId"]);
                drHeader["assetsClassName"] = DataConvert.ToString(dr["assetsClassName"]);
                if (DataConvert.ToString(dr["assetsClassPath"]) == ""
                    || DataConvert.ToString(dr["assetsClassPath"]) == DataConvert.ToString(dr["assetsClassId"]))
                {
                    drHeader["beginPeriodNum"] = DataConvert.ToInt32(dt.Compute("sum(beginPeriodNum)", "assetsClassId ='" + DataConvert.ToString(dr["assetsClassId"]) + "'"));
                    drHeader["beginPeriodAmt"] = DataConvert.ToDouble(dt.Compute("sum(beginPeriodAmt)", "assetsClassId ='" + DataConvert.ToString(dr["assetsClassId"]) + "'"));
                    drHeader["currentPeriodAddNum"] = DataConvert.ToInt32(dt.Compute("sum(currentPeriodAddNum)", "assetsClassId ='" + DataConvert.ToString(dr["assetsClassId"]) + "'"));
                    drHeader["currentPeriodAddAmt"] = DataConvert.ToDouble(dt.Compute("sum(currentPeriodAddAmt)", "assetsClassId ='" + DataConvert.ToString(dr["assetsClassId"]) + "'"));
                    drHeader["currentPeriodReduceNum"] = DataConvert.ToInt32(dt.Compute("sum(currentPeriodReduceNum)", "assetsClassId ='" + DataConvert.ToString(dr["assetsClassId"]) + "'"));
                    drHeader["currentPeriodReduceAmt"] = DataConvert.ToDouble(dt.Compute("sum(currentPeriodReduceAmt)", "assetsClassId ='" + DataConvert.ToString(dr["assetsClassId"]) + "'"));
                    drHeader["endPeriodNum"] = DataConvert.ToInt32(dt.Compute("sum(endPeriodNum)", "assetsClassId ='" + DataConvert.ToString(dr["assetsClassId"]) + "'"));
                    drHeader["endPeriodAmt"] = DataConvert.ToDouble(dt.Compute("sum(endPeriodAmt)", "assetsClassId ='" + DataConvert.ToString(dr["assetsClassId"]) + "'"));
                }
                else
                {
                    drHeader["beginPeriodNum"] = DataConvert.ToInt32(dt.Compute("sum(beginPeriodNum)", "assetsClassPath like '%" + DataConvert.ToString(dr["assetsClassId"]) + "%'"));
                    drHeader["beginPeriodAmt"] = DataConvert.ToDouble(dt.Compute("sum(beginPeriodAmt)", "assetsClassPath like '%" + DataConvert.ToString(dr["assetsClassId"]) + "%'"));
                    drHeader["currentPeriodAddNum"] = DataConvert.ToInt32(dt.Compute("sum(currentPeriodAddNum)", "assetsClassPath like '%" + DataConvert.ToString(dr["assetsClassId"]) + "%'"));
                    drHeader["currentPeriodAddAmt"] = DataConvert.ToDouble(dt.Compute("sum(currentPeriodAddAmt)", "assetsClassPath like '%" + DataConvert.ToString(dr["assetsClassId"]) + "%'"));
                    drHeader["currentPeriodReduceNum"] = DataConvert.ToInt32(dt.Compute("sum(currentPeriodReduceNum)", "assetsClassPath like '%" + DataConvert.ToString(dr["assetsClassId"]) + "%'"));
                    drHeader["currentPeriodReduceAmt"] = DataConvert.ToDouble(dt.Compute("sum(currentPeriodReduceAmt)", "assetsClassPath like '%" + DataConvert.ToString(dr["assetsClassId"]) + "%'"));
                    drHeader["endPeriodNum"] = DataConvert.ToInt32(dt.Compute("sum(endPeriodNum)", "assetsClassPath like '%" + DataConvert.ToString(dr["assetsClassId"]) + "%'"));
                    drHeader["endPeriodAmt"] = DataConvert.ToDouble(dt.Compute("sum(endPeriodAmt)", "assetsClassPath like '%" + DataConvert.ToString(dr["assetsClassId"]) + "%'"));
                }
                dtView.Rows.Add(drHeader);
            }
            return dtView;
        }

     

//        public AssetsClassChangeReportDS GetReportSource(string querystring)
//        {
//            string sql =string.Format( @"select MonthAssetsClassRecord.*,AssetsClass.assetsClassPath,AssetsClass.assetsClassName 
//            from MonthAssetsClassRecord,AssetsClass 
//            where  MonthAssetsClassRecord.assetsClassId=AssetsClass.assetsClassId {0}
//            order by AssetsClass.assetsClassNo ", ListWhereSql(querystring).Sql);
//            DataTable dt = AppMember.DbHelper.GetDataSet(sql, ListWhereSql(querystring).DBPara).Tables[0];
//            string sqlRpt = string.Format(@"select distinct AssetsClass.assetsClassId, AssetsClass.assetsClassNo,
//            AssetsClass.assetsClassName ,AssetsClass.assetsClassPath
//            from MonthAssetsClassRecord,AssetsClass 
//            where  MonthAssetsClassRecord.assetsClassId=AssetsClass.assetsClassId {0}
//            order by AssetsClass.assetsClassNo ", ListWhereSql(querystring).Sql);
//            DataTable dtRpt = AppMember.DbHelper.GetDataSet(sqlRpt, ListWhereSql(querystring).DBPara).Tables[0];
//            AssetsClassChangeReportDS ds = new AssetsClassChangeReportDS();
//            AssetsClassChangeReportDS.AssetsClassChangeReportDataTable dtHeader = ds.AssetsClassChangeReport;
//            foreach (DataRow dr in dtRpt.Rows)
//            {
//                AssetsClassChangeReportDS.AssetsClassChangeReportRow drHeader = dtHeader.NewAssetsClassChangeReportRow();
//                drHeader.assetsClassId = DataConvert.ToString(dr["assetsClassId"]);
//                drHeader.assetsClassName = DataConvert.ToString(dr["assetsClassName"]);
//                if (DataConvert.ToString(dr["assetsClassPath"]) == ""
//                    || DataConvert.ToString(dr["assetsClassPath"]) == DataConvert.ToString(dr["assetsClassId"]))
//                {
//                    drHeader.beginPeriodNum = DataConvert.ToInt32(dt.Compute("sum(beginPeriodNum)", "assetsClassId ='" + DataConvert.ToString(dr["assetsClassId"]) + "'"));
//                    drHeader.beginPeriodAmt = DataConvert.ToDouble(dt.Compute("sum(beginPeriodAmt)", "assetsClassId ='" + DataConvert.ToString(dr["assetsClassId"]) + "'"));
//                    drHeader.currentPeriodAddNum = DataConvert.ToInt32(dt.Compute("sum(currentPeriodAddNum)", "assetsClassId ='" + DataConvert.ToString(dr["assetsClassId"]) + "'"));
//                    drHeader.currentPeriodAddAmt = DataConvert.ToDouble(dt.Compute("sum(currentPeriodAddAmt)", "assetsClassId ='" + DataConvert.ToString(dr["assetsClassId"]) + "'"));
//                    drHeader.currentPeriodReduceNum = DataConvert.ToInt32(dt.Compute("sum(currentPeriodReduceNum)", "assetsClassId ='" + DataConvert.ToString(dr["assetsClassId"]) + "'"));
//                    drHeader.currentPeriodReduceAmt = DataConvert.ToDouble(dt.Compute("sum(currentPeriodReduceAmt)", "assetsClassId ='" + DataConvert.ToString(dr["assetsClassId"]) + "'"));
//                    drHeader.endPeriodNum = DataConvert.ToInt32(dt.Compute("sum(endPeriodNum)", "assetsClassId ='" + DataConvert.ToString(dr["assetsClassId"]) + "'"));
//                    drHeader.endPeriodAmt = DataConvert.ToDouble(dt.Compute("sum(endPeriodAmt)", "assetsClassId ='" + DataConvert.ToString(dr["assetsClassId"]) + "'"));
//                }
//                else
//                {
//                    drHeader.beginPeriodNum = DataConvert.ToInt32(dt.Compute("sum(beginPeriodNum)", "assetsClassPath like '%" + DataConvert.ToString(dr["assetsClassId"]) + "%'"));
//                    drHeader.beginPeriodAmt = DataConvert.ToDouble(dt.Compute("sum(beginPeriodAmt)", "assetsClassPath like '%" + DataConvert.ToString(dr["assetsClassId"]) + "%'"));
//                    drHeader.currentPeriodAddNum = DataConvert.ToInt32(dt.Compute("sum(currentPeriodAddNum)", "assetsClassPath like '%" + DataConvert.ToString(dr["assetsClassId"]) + "%'"));
//                    drHeader.currentPeriodAddAmt = DataConvert.ToDouble(dt.Compute("sum(currentPeriodAddAmt)", "assetsClassPath like '%" + DataConvert.ToString(dr["assetsClassId"]) + "%'"));
//                    drHeader.currentPeriodReduceNum = DataConvert.ToInt32(dt.Compute("sum(currentPeriodReduceNum)", "assetsClassPath like '%" + DataConvert.ToString(dr["assetsClassId"]) + "%'"));
//                    drHeader.currentPeriodReduceAmt = DataConvert.ToDouble(dt.Compute("sum(currentPeriodReduceAmt)", "assetsClassPath like '%" + DataConvert.ToString(dr["assetsClassId"]) + "%'"));
//                    drHeader.endPeriodNum = DataConvert.ToInt32(dt.Compute("sum(endPeriodNum)", "assetsClassPath like '%" + DataConvert.ToString(dr["assetsClassId"]) + "%'"));
//                    drHeader.endPeriodAmt = DataConvert.ToDouble(dt.Compute("sum(endPeriodAmt)", "assetsClassPath like '%" + DataConvert.ToString(dr["assetsClassId"]) + "%'"));
//                }
//                dtHeader.Rows.Add(drHeader);
//            }
//            return ds;
//        }

        protected WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            EntryModel model = JsonHelper.Deserialize<EntryModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.FiscalYearId) != "")
            {
                wcd.Sql += @" and MonthAssetsClassRecord.fiscalYearId=@fiscalYearId";
                wcd.DBPara.Add("fiscalYearId", model.FiscalYearId);
            }
            if (DataConvert.ToString(model.FiscalPeriodId) != "")
            {
                wcd.Sql += @" and MonthAssetsClassRecord.fiscalPeriodId=@fiscalPeriodId";
                wcd.DBPara.Add("fiscalPeriodId", model.FiscalPeriodId);
            }
            if (DataConvert.ToString(model.AssetsClassId) != "")
            {
                wcd.Sql += @" and  AssetsClass.assetsClassId=@assetsClassId";
                wcd.DBPara.Add("assetsClassId", model.AssetsClassId);
            }
            if (DataConvert.ToString(model.DepartmentId) != "")
            {
                wcd.Sql += @" and MonthAssetsClassRecord.departmentId=@departmentId";
                wcd.DBPara.Add("departmentId", model.DepartmentId);
            }
            if (DataConvert.ToString(model.StoreSiteId) != "")
            {
                wcd.Sql += @" and MonthAssetsClassRecord.storeSiteId=@storeSiteId";
                wcd.DBPara.Add("storeSiteId", model.StoreSiteId);
            }
            return wcd;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;
using System.Data;
using BaseCommon.Data;
using BusinessLogic.Report.Models.AssetsChangeReport;

namespace BusinessLogic.Report.Repositorys
{
    public class AssetsChangeReportRepository : IQuery
    {
        public virtual DataTable GetReportGridDataTable(ListCondition condition)
        {
            EntryModel model = JsonHelper.Deserialize<EntryModel>(condition.ListModelString);
            switch (model.FormMode)
            {
                case "AssetsClass":
                    return GetAssetsClassTable(condition);
                case "Department":
                    return GetDepartmentTable(condition);
                case "StoreSite":
                    return GetStoreSiteTable(condition);
                default :
                    return GetAssetsClassTable(condition);
            }
        }

        protected virtual DataTable GetAssetsClassTable(ListCondition condition)
        {
            string sql = string.Format(@"select MonthRecord.*,
            AssetsClass.assetsClassPath,
            AssetsClass.assetsClassName 
            from MonthRecord,AssetsClass 
            where  MonthRecord.assetsClassId=AssetsClass.assetsClassId {0}
            order by AssetsClass.assetsClassNo ", ListWhereSql(condition).Sql);
            DataTable dtAll = AppMember.DbHelper.GetDataSet(sql, ListWhereSql(condition).DBPara).Tables[0];

            string sqlGroup = string.Format(@"select distinct AssetsClass.assetsClassId,
            AssetsClass.assetsClassNo,
            AssetsClass.assetsClassName ,
            AssetsClass.assetsClassPath
            from MonthRecord,AssetsClass 
            where  MonthRecord.assetsClassId=AssetsClass.assetsClassId {0}
            order by AssetsClass.assetsClassNo ", ListWhereSql(condition).Sql);
            DataTable dtGroup = AppMember.DbHelper.GetDataSet(sqlGroup, ListWhereSql(condition).DBPara).Tables[0];

            string sqlView = @"select MonthRecord.* ,
                              AssetsClass.assetsClassName 
                              from MonthRecord,AssetsClass 
                             where MonthRecord.assetsClassId=AssetsClass.assetsClassId and 1<>1 ";
            DataTable dtView = AppMember.DbHelper.GetDataSet(sqlView).Tables[0];

            SetDataView(dtView, dtAll, dtGroup, "assetsClass");
            return dtView;
        }

        protected virtual DataTable GetDepartmentTable(ListCondition condition)
        {
            string sql = string.Format(@"select MonthRecord.*,
            AppDepartment.departmentPath,
            AppDepartment.departmentName 
            from MonthRecord,AppDepartment 
            where  MonthRecord.departmentId=AppDepartment.departmentId {0}
            order by AppDepartment.departmentNo ", ListWhereSql(condition).Sql);
            DataTable dtAll = AppMember.DbHelper.GetDataSet(sql, ListWhereSql(condition).DBPara).Tables[0];

            string sqlGroup = string.Format(@"select distinct AppDepartment.departmentId, 
            AppDepartment.departmentNo,
            AppDepartment.departmentName ,
            AppDepartment.departmentPath
            from MonthRecord,AppDepartment 
            where  MonthRecord.departmentId=AppDepartment.departmentId {0}
            order by AppDepartment.departmentNo ", ListWhereSql(condition).Sql);
            DataTable dtGroup = AppMember.DbHelper.GetDataSet(sqlGroup, ListWhereSql(condition).DBPara).Tables[0];

            string sqlView = @"select MonthRecord.* ,
                              AppDepartment.departmentName 
                              from MonthRecord,AppDepartment 
                             where MonthRecord.departmentId=AppDepartment.departmentId and 1<>1 ";
            DataTable dtView = AppMember.DbHelper.GetDataSet(sqlView).Tables[0];

            SetDataView(dtView, dtAll, dtGroup, "department");
            return dtView;
        }

        protected virtual DataTable GetStoreSiteTable(ListCondition condition)
        {
            string sql = string.Format(@"select MonthRecord.*,
            StoreSite.storeSitePath,
            StoreSite.storeSiteName 
            from MonthRecord,StoreSite 
            where  MonthRecord.storeSiteId=StoreSite.storeSiteId {0}
            order by StoreSite.storeSiteNo ", ListWhereSql(condition).Sql);
            DataTable dtAll = AppMember.DbHelper.GetDataSet(sql, ListWhereSql(condition).DBPara).Tables[0];

            string sqlGroup = string.Format(@"select distinct StoreSite.storeSiteId, 
            StoreSite.storeSiteNo,
            StoreSite.storeSiteName ,
            StoreSite.storeSitePath
            from MonthRecord,StoreSite 
            where  MonthRecord.storeSiteId=StoreSite.storeSiteId {0}
            order by StoreSite.storeSiteNo ", ListWhereSql(condition).Sql);
            DataTable dtGroup = AppMember.DbHelper.GetDataSet(sqlGroup, ListWhereSql(condition).DBPara).Tables[0];

            string sqlView = @"select MonthRecord.* ,
                              StoreSite.storeSiteName 
                              from MonthRecord,StoreSite 
                             where MonthRecord.storeSiteId=StoreSite.storeSiteId and 1<>1 ";
            DataTable dtView = AppMember.DbHelper.GetDataSet(sqlView).Tables[0];

            SetDataView(dtView, dtAll, dtGroup, "storeSite");
            return dtView;
        }

        protected virtual void SetDataView(DataTable dtView, DataTable dtAll, DataTable dtGroup, string tableTag)
        {

            foreach (DataRow dr in dtGroup.Rows)
            {
                DataRow drHeader = dtView.NewRow();
                drHeader[tableTag + "Name"] = DataConvert.ToString(dr[tableTag + "Name"]);
                if (DataConvert.ToString(dr[tableTag + "Path"]) == ""
                    || DataConvert.ToString(dr[tableTag + "Path"]) == DataConvert.ToString(dr[tableTag + "Id"]))
                {
                    drHeader["beginPeriodNum"] = DataConvert.ToInt32(dtAll.Compute("sum(beginPeriodNum)", tableTag + "Id ='" + DataConvert.ToString(dr[tableTag + "Id"]) + "'"));
                    drHeader["beginPeriodAmt"] = DataConvert.ToDouble(dtAll.Compute("sum(beginPeriodAmt)", tableTag + "Id ='" + DataConvert.ToString(dr[tableTag + "Id"]) + "'"));
                    drHeader["currentPeriodAddNum"] = DataConvert.ToInt32(dtAll.Compute("sum(currentPeriodAddNum)", tableTag + "Id ='" + DataConvert.ToString(dr[tableTag + "Id"]) + "'"));
                    drHeader["currentPeriodAddAmt"] = DataConvert.ToDouble(dtAll.Compute("sum(currentPeriodAddAmt)", tableTag + "Id ='" + DataConvert.ToString(dr[tableTag + "Id"]) + "'"));
                    drHeader["currentPeriodReduceNum"] = DataConvert.ToInt32(dtAll.Compute("sum(currentPeriodReduceNum)", tableTag + "Id ='" + DataConvert.ToString(dr[tableTag + "Id"]) + "'"));
                    drHeader["currentPeriodReduceAmt"] = DataConvert.ToDouble(dtAll.Compute("sum(currentPeriodReduceAmt)", tableTag + "Id ='" + DataConvert.ToString(dr[tableTag + "Id"]) + "'"));
                    drHeader["endPeriodNum"] = DataConvert.ToInt32(dtAll.Compute("sum(endPeriodNum)", tableTag + "Id ='" + DataConvert.ToString(dr[tableTag + "Id"]) + "'"));
                    drHeader["endPeriodAmt"] = DataConvert.ToDouble(dtAll.Compute("sum(endPeriodAmt)", tableTag + "Id ='" + DataConvert.ToString(dr[tableTag + "Id"]) + "'"));
                }
                else
                {
                    drHeader["beginPeriodNum"] = DataConvert.ToInt32(dtAll.Compute("sum(beginPeriodNum)", tableTag + "Path like '%" + DataConvert.ToString(dr[tableTag + "Id"]) + "%'"));
                    drHeader["beginPeriodAmt"] = DataConvert.ToDouble(dtAll.Compute("sum(beginPeriodAmt)", tableTag + "Path like '%" + DataConvert.ToString(dr[tableTag + "Id"]) + "%'"));
                    drHeader["currentPeriodAddNum"] = DataConvert.ToInt32(dtAll.Compute("sum(currentPeriodAddNum)", tableTag + "Path like '%" + DataConvert.ToString(dr[tableTag + "Id"]) + "%'"));
                    drHeader["currentPeriodAddAmt"] = DataConvert.ToDouble(dtAll.Compute("sum(currentPeriodAddAmt)", tableTag + "Path like '%" + DataConvert.ToString(dr[tableTag + "Id"]) + "%'"));
                    drHeader["currentPeriodReduceNum"] = DataConvert.ToInt32(dtAll.Compute("sum(currentPeriodReduceNum)", tableTag + "Path like '%" + DataConvert.ToString(dr[tableTag + "Id"]) + "%'"));
                    drHeader["currentPeriodReduceAmt"] = DataConvert.ToDouble(dtAll.Compute("sum(currentPeriodReduceAmt)", tableTag + "Path like '%" + DataConvert.ToString(dr[tableTag + "Id"]) + "%'"));
                    drHeader["endPeriodNum"] = DataConvert.ToInt32(dtAll.Compute("sum(endPeriodNum)", tableTag + "Path like '%" + DataConvert.ToString(dr[tableTag + "Id"]) + "%'"));
                    drHeader["endPeriodAmt"] = DataConvert.ToDouble(dtAll.Compute("sum(endPeriodAmt)", tableTag + "Path like '%" + DataConvert.ToString(dr[tableTag + "Id"]) + "%'"));
                }
                dtView.Rows.Add(drHeader);
            }
        }


        //        public AssetsClassChangeReportDS GetReportSource(string querystring)
        //        {
        //            string sql =string.Format( @"select MonthRecord.*,AssetsClass.assetsClassPath,AssetsClass.assetsClassName 
        //            from MonthRecord,AssetsClass 
        //            where  MonthRecord.assetsClassId=AssetsClass.assetsClassId {0}
        //            order by AssetsClass.assetsClassNo ", ListWhereSql(querystring).Sql);
        //            DataTable dt = AppMember.DbHelper.GetDataSet(sql, ListWhereSql(querystring).DBPara).Tables[0];
        //            string sqlRpt = string.Format(@"select distinct AssetsClass.assetsClassId, AssetsClass.assetsClassNo,
        //            AssetsClass.assetsClassName ,AssetsClass.assetsClassPath
        //            from MonthRecord,AssetsClass 
        //            where  MonthRecord.assetsClassId=AssetsClass.assetsClassId {0}
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
                wcd.Sql += @" and MonthRecord.fiscalYearId=@fiscalYearId";
                wcd.DBPara.Add("fiscalYearId", model.FiscalYearId);
            }
            if (DataConvert.ToString(model.FiscalPeriodId) != "")
            {
                wcd.Sql += @" and MonthRecord.fiscalPeriodId=@fiscalPeriodId";
                wcd.DBPara.Add("fiscalPeriodId", model.FiscalPeriodId);
            }
            if (DataConvert.ToString(model.AssetsClassId) != "")
            {
                wcd.Sql += @" and  AssetsClass.assetsClassId=@assetsClassId";
                wcd.DBPara.Add("assetsClassId", model.AssetsClassId);
            }
            if (DataConvert.ToString(model.DepartmentId) != "")
            {
                wcd.Sql += @" and MonthRecord.departmentId=@departmentId";
                wcd.DBPara.Add("departmentId", model.DepartmentId);
            }
            if (DataConvert.ToString(model.StoreSiteId) != "")
            {
                wcd.Sql += @" and MonthRecord.storeSiteId=@storeSiteId";
                wcd.DBPara.Add("storeSiteId", model.StoreSiteId);
            }
            return wcd;
        }

    }
}

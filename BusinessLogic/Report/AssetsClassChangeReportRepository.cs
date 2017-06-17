using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Report;
using BaseCommon.Basic;
using System.Data;
using BaseCommon.Data;

namespace BusinessLogic.Report
{
    public class AssetsClassChangeReportRepository
    {
        public AssetsClassChangeReportDS GetReportSource()
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            string sql = @"select * from AssetsClass ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            AssetsClassChangeReportDS ds = new AssetsClassChangeReportDS();
            AssetsClassChangeReportDS.AssetsClassChangeReportDataTable dtHeader = ds.AssetsClassChangeReport;
            foreach (DataRow dr in dt.Rows)
            {
                AssetsClassChangeReportDS.AssetsClassChangeReportRow drHeader = dtHeader.NewAssetsClassChangeReportRow();
                drHeader.assetsClassId = DataConvert.ToString(dr["assetsClassId"]);
                drHeader.assetsClassName = DataConvert.ToString(dr["assetsClassName"]);
                dtHeader.Rows.Add(drHeader);
            }
            return ds;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic.Report.Models.StoreSiteAssetsPlot;
using BaseCommon.Data;
using System.Data;
using BaseCommon.Basic;

namespace BusinessLogic.Report.Repositorys
{
    public class StoreSiteAssetsPlotRepository
    {
        public string GetSource(string formVar)
        {
            EntryModel model = JsonHelper.Deserialize<EntryModel>(formVar);
            Dictionary<string, object> paras = new Dictionary<string, object>();
            string whereSql = "";
            if (model != null)
            {
                if (DataConvert.ToString(model.CompanyId) != "")
                {
                    paras.Add("companyId", model.CompanyId);
                    whereSql += " and AppDepartment.companyId=@companyId ";
                }
            }

            string sqlStoreSite = string.Format(@" SELECT  storeSiteName,storeSiteId from StoreSite where parentId=(select storeSiteId from StoreSite where storeSiteNo='LOC_SJZX' )", whereSql);
            DataTable dtStoreSite = AppMember.DbHelper.GetDataSet(sqlStoreSite).Tables[0];

            string sqlAssetsClass = string.Format(@" SELECT assetsClassId,assetsClassName,Row_Number() OVER(ORDER BY assetsClassName) rnum
                           from AssetsClass where assetsClassNo in('C0011','C0004','C0003','C0002')  ORDER BY  assetsClassName", whereSql);
            DataTable dtAssetsClass = AppMember.DbHelper.GetDataSet(sqlAssetsClass).Tables[0];

            string sql = string.Format(@"SELECT AssetsClass.assetsClassName assetsClassName,
                        Assets.assetsClassId assetsClassId,
                        ss.storeSiteName storeSiteName,
                        ss.storeSiteId storeSiteId,
                        COUNT(1) cnt 
                        FROM (select * from StoreSite where parentId=(select storeSiteId from StoreSite where storeSiteNo='LOC_SJZX' ) ) SS LEFT JOIN 
                        Assets ON ss.storeSiteId=Assets.storeSiteId  
                        LEFT JOIN AppDepartment ON  Assets.departmentId=AppDepartment.departmentId 
                        LEFT JOIN AssetsClass ON AssetsClass.assetsClassId=Assets.assetsClassId 
                        WHERE 1=1 {0} 
                        GROUP BY Assets.assetsClassId,AssetsClass.assetsClassName,ss.storeSiteName,ss.storeSiteId
                        Order BY ss.storeSiteName ,AssetsClass.assetsClassName", whereSql);
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];


            string vars = "";

            //刻度值 start
            string ticks = "var ticks = [";
            foreach (DataRow drs in dtStoreSite.Rows)
            {
                ticks += string.Format(@"'{0}',", DataConvert.ToString(drs["storeSiteName"]));
            }
            if (ticks != "var ticks = [")
                ticks = ticks.Substring(0, ticks.Length - 1) + "]; ";
            else
                ticks = "var ticks = []; ";
            //刻度值 end

            //label值 start
            string labels = "var labels = [";
            foreach (DataRow drc in dtAssetsClass.Rows)
            {
                labels += string.Format(@"'{0}',", DataConvert.ToString(drc["assetsClassName"]));
            }
            if (labels != "var labels = [")
                labels = labels.Substring(0, labels.Length - 1) + "]; ";
            else
                labels = "var labels = []; ";
            //label值 end

            //具体数值 start
            string lineCollection = "var lines=[";
            string content = "";
            int i = 0;//存放地点计数
            foreach (DataRow drc in dtAssetsClass.Rows)
            {
                foreach (DataRow drs in dtStoreSite.Rows)
                {

                    DataRow[] drval = dt.Select(string.Format(" assetsClassId='{0}' and storeSiteId='{1}'", DataConvert.ToString(drc["assetsClassId"]), DataConvert.ToString(drs["storeSiteId"])));
                    if (drval.Length > 0)
                    {
                        content += DataConvert.ToString(drval[0]["cnt"]) + ",";
                    }
                    else
                    {
                        content += "0,";
                    }
                }
                if (content.Length > 0)
                    content = content.Substring(0, content.Length - 1);
                vars += string.Format(@"var line{0} = [{1}]; ", i + 1, content);
                lineCollection += string.Format(@"line{0},", i + 1);
                content = "";
                i++;
            }
            if (lineCollection != "var lines=[")
                lineCollection = lineCollection.Substring(0, lineCollection.Length - 1) + "]; ";
            else
                lineCollection = "var lines=[]; ";
            vars += lineCollection;
            //具体数值 end

            vars += ticks;
            vars += labels;

            if (i > 1)
            {
                vars += string.Format("var chartWeight={0}; ", dtStoreSite.Rows.Count * dtAssetsClass.Rows.Count * 60);
            }
            return vars;
        }
    }
}

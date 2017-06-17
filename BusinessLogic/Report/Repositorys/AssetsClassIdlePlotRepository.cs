using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic.Report.Models.AssetsClassIdlePlot;
using BaseCommon.Data;
using System.Data;
using BaseCommon.Basic;

namespace BusinessLogic.Report.Repositorys
{
   public class AssetsClassIdlePlotRepository
    {
       public string GetSource(string formVar)
       {
           EntryModel model = JsonHelper.Deserialize<EntryModel>(formVar);
           Dictionary<string, object> paras = new Dictionary<string, object>();
           string whereSql = "";
           
           if (model != null)
           {
               if (DataConvert.ToString(model.CompanyId)!= "")
               {
                   paras.Add("companyId", model.CompanyId);
                   whereSql += " and AppDepartment.companyId=@companyId ";
               }
               if (DataConvert.ToString(model.DepartmentId) != "")
               {
                   paras.Add("departmentId", model.DepartmentId);
                   whereSql += " and Assets.departmentId=@departmentId ";
               }
               if (DataConvert.ToString(model.AssetsClassId) != "")
               {
                   paras.Add("assetsClassId", model.AssetsClassId);
                   whereSql += " and Assets.assetsClassId=@assetsClassId ";
               }
           }
           string idleWhereSql = whereSql+ " and Assets.assetsState='X' ";

           string sql = @"SELECT (SELECT assetsClassName FROM AssetsClass WHERE assetsClassId=Assets.assetsClassId) assetsClassName,assetsClassId,COUNT(1) cnt 
                        FROM Assets,AppDepartment
                        WHERE  Assets.departmentId=AppDepartment.departmentId  {0} 
                        GROUP BY assetsClassId";
           string sql1 = string.Format(sql,idleWhereSql );
           string sql2 = string.Format(sql, whereSql);
           DataTable dt = AppMember.DbHelper.GetDataSet(sql1, paras).Tables[0];
           DataTable dt2 = AppMember.DbHelper.GetDataSet(sql2, paras).Tables[0]; 
           string content = "";
           string content2 = "";
           foreach (DataRow dr in dt.Rows)
           {
               content += "['" + DataConvert.ToString(dr["assetsClassName"]) + "'," + DataConvert.ToString(dr["cnt"]) + "],";
               DataRow[] drTotal = dt2.Select("assetsClassId='" + DataConvert.ToString(dr["assetsClassId"]) + "'");
               double totalCnt =0;
               if (drTotal.Length > 0)
                   totalCnt =DataConvert.ToDouble( drTotal[0]["cnt"]);
               content2 += "['" + DataConvert.ToString(dr["assetsClassName"]) + "闲置率" + "'," +(totalCnt==0?0: DataConvert.ToDouble(dr["cnt"]) / totalCnt) + "],";
           }
           if (content.Length>0)
              content = content.Substring(0, content.Length -1);
           if (content2.Length > 0)
               content2 = content2.Substring(0, content2.Length - 1);
           string vars = string.Format(@"var line1 = [{0}];", content);
           vars += string.Format(@"var line2 = [{0}];", content2);
           if (dt.Rows.Count > 0)
           {
               vars += string.Format("var chartWeight={0};", dt.Rows.Count * 150);
           }
           return vars;
       }
    }
}

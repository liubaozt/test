using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic.Report.Models.AssetsClassPlot;
using BaseCommon.Data;
using System.Data;
using BaseCommon.Basic;

namespace BusinessLogic.Report.Repositorys
{
   public class AssetsClassPlotRepository
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
           string sql =string.Format( @"SELECT (SELECT assetsClassName FROM AssetsClass WHERE assetsClassId=Assets.assetsClassId) assetsClassName,COUNT(1) cnt 
                        FROM Assets,AppDepartment
                        WHERE  Assets.departmentId=AppDepartment.departmentId  {0} 
                        GROUP BY assetsClassId", whereSql);
           DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
           string content = "";
           foreach (DataRow dr in dt.Rows)
           {
               content += "[" + DataConvert.ToString(dr["cnt"]) + ",'" + DataConvert.ToString(dr["assetsClassName"]) + "'],";
           }
           if (content.Length>0)
              content = content.Substring(0, content.Length -1);
           string vars = string.Format(@"var line1 = [{0}];", content);
           if (dt.Rows.Count > 0)
           {
               vars +=string.Format( "var chartHeight={0};",dt.Rows.Count*50);
           }
           return vars;
       }
    }
}

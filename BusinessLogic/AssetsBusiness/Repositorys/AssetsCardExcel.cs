using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Data;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class AssetsCardExcel
    {
        public static StringBuilder CreateCardExcel(DataSet ds)
        {
            DataRow dr = ds.Tables["Assets"].Rows[0];

            string scrapDate = "";
            if (ds.Tables["AssetsScrapDetail"].Rows.Count > 0)
            {
                scrapDate =DataConvert.ToDateTime( ds.Tables["AssetsScrapDetail"].Rows[0]["scrapDate"]).ToString("yyyy-MM-dd");
            }
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0' style='width:800px'>");
            sbHtml.Append("<tr>");
            sbHtml.AppendFormat("<td style='font-size:12px;text-align:center;height:40px;' colspan=8><span style='font-weight:bold;font-size:20px;'>{0}</span>&#12288;&#12288;&#12288;&#12288;{1}</td>", "固定资产存档表", "存档表编号：" + AutoNoGenerator.GetMaxNo("AssetsCunDang",false,5));
            sbHtml.Append("</tr>");
            sbHtml.Append("<tr>");
            sbHtml.AppendFormat("<td style='font-size:12px;text-align:center;height:20px;'  colspan=8><span style='font-weight:bold;'>{0}</span></td>", "基本情况");
            sbHtml.Append("</tr>");
            sbHtml.Append("<tr>");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "入库日期");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:280px;' colspan=3>{0}</td>", DataConvert.ToDateTime(dr["purchaseDate"]).ToString("yyyy-MM-dd"));
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "报废日期");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:280px;' colspan=3>{0}</td>", scrapDate);
            sbHtml.Append("</tr>");
            sbHtml.Append("<tr>");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "资产名称");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:280px;'  colspan=3>{0}</td>", DataConvert.ToString(dr["assetsName"]));
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "规格/型号");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:280px;'  colspan=3>{0}</td>", DataConvert.ToString(dr["spec"]));
            sbHtml.Append("</tr>");
            sbHtml.Append("<tr>");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "资产金额");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:280px;'  colspan=3>{0}</td>", DataConvert.ToString(dr["assetsValue"]));
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "原单位资产编号");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:280px;'  colspan=3>{0}</td>", DataConvert.ToString(dr["assetsNo"]));
            sbHtml.Append("</tr>");
            sbHtml.Append("<tr>");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "相关附件");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:280px;'  colspan=3>{0}</td>", "");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "出厂编号");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:280px;'  colspan=3>{0}</td>", "");
            sbHtml.Append("</tr>");
            sbHtml.Append("<tr>");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "供应厂家/资产来源");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:680px;'  colspan=7>{0}</td>", "");
            sbHtml.Append("</tr>");
            sbHtml.Append("<tr>");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "资产编号");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:280px;'  colspan=3>{0}</td>", DataConvert.ToString(dr["assetsBarcode"]));
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "对应课题号");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:280px;'  colspan=3>{0}</td>", "");
            sbHtml.Append("</tr>");
            sbHtml.Append("<tr>");
            sbHtml.AppendFormat("<td style='font-size:12px;text-align:center;height:20px;'  colspan=8><span style='font-weight:bold;'>{0}</span></td>", "使用记录");
            sbHtml.Append("</tr>");
            sbHtml.Append("<tr>");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "领用(移交)日期");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:80px;'>{0}</td>","领用人");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:80px;'>{0}</td>", "移交人");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "研发小组(所属部门)");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:100px;'>{0}</td>", "联系方式");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "地点");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:80px;'>{0}</td>", "退库日期");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:100px;'>{0}</td>", "备注");
            sbHtml.Append("</tr>");
            DataTable dtTransfer = ds.Tables["AssetsTransfer"];
            foreach (DataRow drTrans in dtTransfer.Rows)
            {
                sbHtml.Append("<tr>");
                sbHtml.AppendFormat("<td style='font-size:12px;height:20px;'>{0}</td>", DataConvert.ToDateTime(drTrans["transferDate"]).ToString("yyyy-MM-dd"));
                sbHtml.AppendFormat("<td style='font-size:12px;height:20px;'>{0}</td>", DataConvert.ToString(drTrans["newUsePeople"]));
                sbHtml.AppendFormat("<td style='font-size:12px;height:20px;'>{0}</td>", DataConvert.ToString(drTrans["originalUsePeople"]));
                sbHtml.AppendFormat("<td style='font-size:12px;height:20px;'>{0}</td>", DataConvert.ToString(drTrans["newDepartmentId"]));
                sbHtml.AppendFormat("<td style='font-size:12px;height:20px;'>{0}</td>", DataConvert.ToString(drTrans["tel"]));
                sbHtml.AppendFormat("<td style='font-size:12px;height:20px;'>{0}</td>", DataConvert.ToString(drTrans["newStoreSiteId"]));
                sbHtml.AppendFormat("<td style='font-size:12px;height:20px;'>{0}</td>", "");
                sbHtml.AppendFormat("<td style='font-size:12px;height:20px;'>{0}</td>", DataConvert.ToString(drTrans["remark"]));
                sbHtml.Append("</tr>");
            }
            sbHtml.Append("</table>");
            string ss = sbHtml.ToString();
            return sbHtml;
        }

    }
}

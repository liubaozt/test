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
            sbHtml.AppendFormat("<td style='font-size:12px;text-align:center;height:40px;' colspan=8><span style='font-weight:bold;font-size:20px;'>{0}</span>&#12288;&#12288;&#12288;&#12288;{1}</td>", "�̶��ʲ��浵��", "�浵���ţ�" + AutoNoGenerator.GetMaxNo("AssetsCunDang",false,5));
            sbHtml.Append("</tr>");
            sbHtml.Append("<tr>");
            sbHtml.AppendFormat("<td style='font-size:12px;text-align:center;height:20px;'  colspan=8><span style='font-weight:bold;'>{0}</span></td>", "�������");
            sbHtml.Append("</tr>");
            sbHtml.Append("<tr>");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "�������");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:280px;' colspan=3>{0}</td>", DataConvert.ToDateTime(dr["purchaseDate"]).ToString("yyyy-MM-dd"));
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "��������");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:280px;' colspan=3>{0}</td>", scrapDate);
            sbHtml.Append("</tr>");
            sbHtml.Append("<tr>");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "�ʲ�����");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:280px;'  colspan=3>{0}</td>", DataConvert.ToString(dr["assetsName"]));
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "���/�ͺ�");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:280px;'  colspan=3>{0}</td>", DataConvert.ToString(dr["spec"]));
            sbHtml.Append("</tr>");
            sbHtml.Append("<tr>");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "�ʲ����");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:280px;'  colspan=3>{0}</td>", DataConvert.ToString(dr["assetsValue"]));
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "ԭ��λ�ʲ����");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:280px;'  colspan=3>{0}</td>", DataConvert.ToString(dr["assetsNo"]));
            sbHtml.Append("</tr>");
            sbHtml.Append("<tr>");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "��ظ���");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:280px;'  colspan=3>{0}</td>", "");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "�������");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:280px;'  colspan=3>{0}</td>", "");
            sbHtml.Append("</tr>");
            sbHtml.Append("<tr>");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "��Ӧ����/�ʲ���Դ");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:680px;'  colspan=7>{0}</td>", "");
            sbHtml.Append("</tr>");
            sbHtml.Append("<tr>");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "�ʲ����");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:280px;'  colspan=3>{0}</td>", DataConvert.ToString(dr["assetsBarcode"]));
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "��Ӧ�����");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:280px;'  colspan=3>{0}</td>", "");
            sbHtml.Append("</tr>");
            sbHtml.Append("<tr>");
            sbHtml.AppendFormat("<td style='font-size:12px;text-align:center;height:20px;'  colspan=8><span style='font-weight:bold;'>{0}</span></td>", "ʹ�ü�¼");
            sbHtml.Append("</tr>");
            sbHtml.Append("<tr>");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "����(�ƽ�)����");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:80px;'>{0}</td>","������");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:80px;'>{0}</td>", "�ƽ���");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "�з�С��(��������)");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:100px;'>{0}</td>", "��ϵ��ʽ");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:120px;'>{0}</td>", "�ص�");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:80px;'>{0}</td>", "�˿�����");
            sbHtml.AppendFormat("<td style='font-size:12px;height:20px;width:100px;'>{0}</td>", "��ע");
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

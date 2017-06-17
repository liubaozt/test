using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace BaseCommon.Data
{
    public class ExcelHelper
    {
        //public static void CreateExcel(HttpResponseBase resp, DataTable dt, Dictionary<string, GridInfo> layout)
        //{
        //    string FileName = IdGenerator.GetMaxId("ExcelExport") + ".xls";
        //    resp.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        //    resp.ContentType = "application/vnd.ms-excel";
        //    resp.AppendHeader("Content-Disposition", "attachment;filename=" + FileName);
        //    string colHeaders = "", ls_item = "";
        //    DataRow[] myRow = dt.Select();//可以类似dt.Select("id>10")之形式达到数据筛选目的
        //    //int i = 0;
        //    int cl = dt.Columns.Count;
        //    //取得数据表各列标题，各标题之间以t分割，最后一个列标题后加回车符 

        //    foreach (KeyValuePair<string, GridInfo> dic in layout)
        //    {
        //        if (dic.Value.Hidden == "false")
        //            colHeaders += dic.Value.Caption + "\t";
        //    }
        //    colHeaders = colHeaders.Substring(0, colHeaders.Length - 1);
        //    colHeaders += "\n";

        //    resp.Write(colHeaders);
        //    //向HTTP输出流中写入取得的数据信息 

        //    //逐行处理数据   
        //    foreach (DataRow row in myRow)
        //    {
        //        foreach (KeyValuePair<string, GridInfo> dic in layout)
        //        {
        //            if (dic.Value.Hidden == "false")
        //            {
        //                ls_item += row[dic.Key].ToString() + "\t";
        //            }
        //        }
        //        ls_item = ls_item.Substring(0, ls_item.Length - 1);
        //        ls_item += "\n";
        //        resp.Write(ls_item);
        //        ls_item = "";

        //    }
        //    resp.End();
        //}

        public static StringBuilder CreateExcel(System.Data.DataTable dt, AdvanceGridLayout layout)
        {
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");

            if (layout.GridGroupHeader != null)
            {
                sbHtml.Append("<tr>");
                foreach (KeyValuePair<string, GirdGroupHeaderInfo> dic in layout.GridGroupHeader)
                {
                    if (DataConvert.ToString(dic.Value.NumberOfRows) != "")
                        sbHtml.AppendFormat("<td style='font-size:14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25' rowspan={0}>{1}</td>", dic.Value.NumberOfRows, dic.Value.TitleText);
                    if (DataConvert.ToString(dic.Value.NumberOfColumns) != "")
                        sbHtml.AppendFormat("<td style='font-size:14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25' colspan={0}>{1}</td>", dic.Value.NumberOfColumns, dic.Value.TitleText);
                }
                sbHtml.Append("</tr>");
            }
            sbHtml.Append("<tr>");
            List<string> lstTitle = new List<string>();
            foreach (KeyValuePair<string, GridInfo> dic in layout.GridLayouts)
            {
                if (dic.Value.Hidden == "false" && dic.Value.IsRowspan != "true")
                    lstTitle.Add(dic.Value.Caption);
            }

            foreach (var item in lstTitle)
            {
                sbHtml.AppendFormat("<td style='font-size:14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", item);
            }
            sbHtml.Append("</tr>");

            //逐行处理数据   
            foreach (DataRow row in dt.Rows)
            {
                sbHtml.Append("<tr>");
                foreach (KeyValuePair<string, GridInfo> dic in layout.GridLayouts)
                {
                    if (dic.Value.Hidden == "false")
                    {
                        sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;width:{0}'>{1}</td>", dic.Value.Width, row[dic.Key].ToString());
                    }
                }
                sbHtml.Append("</tr>");
            }
            sbHtml.Append("</table>");
            string ss = sbHtml.ToString();
            return sbHtml;
        }

        public static StringBuilder CreateExcel(System.Data.DataTable dt, GridLayout layout)
        {
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
            sbHtml.Append("<tr>");
            List<string> lstTitle = new List<string>();
            foreach (KeyValuePair<string, GridInfo> dic in layout.GridLayouts)
            {
                if (dic.Value.Hidden == "false" && dic.Value.IsRowspan != "true")
                    lstTitle.Add(dic.Value.Caption);
            }

            foreach (var item in lstTitle)
            {
                sbHtml.AppendFormat("<td style='font-size:14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", item);
            }
            sbHtml.Append("</tr>");

            //逐行处理数据   
            foreach (DataRow row in dt.Rows)
            {
                sbHtml.Append("<tr>");
                foreach (KeyValuePair<string, GridInfo> dic in layout.GridLayouts)
                {
                    if (dic.Value.Hidden == "false")
                    {
                        sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;width:{0}'>{1}</td>", dic.Value.Width, row[dic.Key].ToString());
                    }
                }
                sbHtml.Append("</tr>");
            }
            sbHtml.Append("</table>");
            string ss = sbHtml.ToString();
            return sbHtml;
        }


        enum ColumnName { A1 = 1, B1, C1, D1, E1, F1, G1, H1, I1, J1, K1, L1, M1, N1, O1, P1, Q1, R1, S1, T1, U1, V1, W1, X1, Y1, Z1 }
        /// <summary>
        /// 导出到Execl
        /// </summary>
        /// <param name="dt">数据集</param>
        /// <param name="strSheetName">工作部名称</param>
        /// <param name="pathloading">保存路径</param>
        /// <param name="title">标题名</param>
        public static void PrintExcel(System.Data.DataTable dt, AdvanceGridLayout layout, string strSheetName)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();  //Execl的操作类
            Workbook bookDest = (Workbook)excel.Workbooks.Add(Missing.Value);
            Worksheet sheetDest = bookDest.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value) as Worksheet;//给工作薄添加一个Sheet  
            sheetDest.Name = strSheetName;
            for (int i = bookDest.Worksheets.Count; i > 1; i--)
            {
                Worksheet wt = (Worksheet)bookDest.Worksheets[i];
                if (wt.Name != strSheetName)
                {
                    wt.Delete();
                }
            }

            //Range rngRow = (Microsoft.Office.Interop.Excel.Range)sheetDest.Columns[1, Type.Missing];
            ////rngRow.UseStandardWidth = 70;
            //Range rngA = (Range)sheetDest.Columns["A", Type.Missing];//设置单元格格式
            //rngA.NumberFormatLocal = "@";//字符型格式
            //Range rngJ = (Range)sheetDest.Columns["J", Type.Missing];
            //rngJ.NumberFormatLocal = "@";
            //Range rngQ = (Range)sheetDest.Columns["Q", Type.Missing];
            //rngQ.NumberFormatLocal = "@";
            //Range rngE = (Range)sheetDest.Columns["E", Type.Missing];
            //rngE.NumberFormatLocal = @"yyyy-mm-dd";//日期型格式
            excel.Application.Workbooks.Add(true);
            try
            {
                int rowIndex = 2;
                int colIndex = 1;
                if (layout.GridGroupHeader != null)
                {
                    ////导入group头
                    foreach (KeyValuePair<string, GirdGroupHeaderInfo> dic in layout.GridGroupHeader)
                    {
                        Range rng;
                        if (DataConvert.ToString(dic.Value.NumberOfRows) != "")
                        {

                            rng = sheetDest.Range[sheetDest.Cells[1, colIndex], sheetDest.Cells[1 + DataConvert.ToInt32(dic.Value.NumberOfRows) - 1, colIndex]];
                            rng.Merge(rng.MergeCells);
                            sheetDest.Cells[1, colIndex] = dic.Value.TitleText;
                            SetHeaderRang(rng);
                            colIndex++;
                        }
                        else
                        {
                            rng = sheetDest.Range[sheetDest.Cells[1, colIndex], sheetDest.Cells[1, colIndex + DataConvert.ToInt32(dic.Value.NumberOfColumns) - 1]];
                            rng.Merge(rng.MergeCells);
                            sheetDest.Cells[1, colIndex] = dic.Value.TitleText;
                            SetHeaderRang(rng);
                            colIndex += DataConvert.ToInt32(dic.Value.NumberOfColumns);
                        }

                    }
                }
                colIndex = 0;
                //导入列头
                foreach (KeyValuePair<string, GridInfo> dic in layout.GridLayouts)
                {
                    if (dic.Value.Hidden == "false")
                    {
                        colIndex++;
                        Range rng = (Range)sheetDest.Cells[2, colIndex];
                        sheetDest.Cells[2, colIndex] = dic.Value.Caption;//Execl中的第一列把DataTable的列名先导进去
                        SetHeaderRang(rng);
                        rng.ColumnWidth = DataConvert.ToDouble(dic.Value.Width) / 7.647;
                    }
                }
                //导入数据行
                foreach (DataRow row in dt.Rows)
                {

                    rowIndex++;
                    colIndex = 0;
                    foreach (KeyValuePair<string, GridInfo> dic in layout.GridLayouts)
                    {
                        if (dic.Value.Hidden == "false")
                        {
                            colIndex++;
                            sheetDest.Cells[rowIndex, colIndex] = row[dic.Key].ToString();
                            Range rng01 = (Range)sheetDest.Cells[rowIndex, colIndex];
                            SetDataRang(rng01);
                        }
                    }
                }
            }
            catch
            {
                throw new Exception();
            }
            bookDest.PrintOutEx();
            //excel.Quit();
            //excel = null;
            //GC.Collect();
        }

        private static void SetHeaderRang(Range rng)
        {
            rng.Font.Name = "宋体";
            rng.Font.Size = 9;
            rng.Font.Bold = true;
            rng.Font.Color = ConsoleColor.Blue;
            rng.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            rng.RowHeight = 15;
            rng.Borders.LineStyle = XlLineStyle.xlContinuous;
        }

        private static void SetDataRang(Range rng)
        {
            rng.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            rng.Borders.LineStyle = XlLineStyle.xlContinuous;
            rng.RowHeight = 15;
            rng.Font.Name = "宋体";
            rng.Font.Size = 9;
        }
    }
}

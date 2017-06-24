using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Data;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            ExcelHelper1 eh = new ExcelHelper1(@"d:\templet.xls", @"d:\out.xls"); 
            DataTable dt = new DataTable("table");
            dt.Columns.Add("col1", System.Type.GetType("System.String"));//
            dt.Columns.Add("col2", System.Type.GetType("System.String"));
            dt.Columns.Add("col3", System.Type.GetType("System.String"));
            DataRow dr = dt.NewRow();
            dr["col1"] = "刘宝";
            dr["col2"] = "30";
            dr["col3"] = "男";
            dt.Rows.Add(dr);
            eh.DataTableToExcel(dt, 1, 2, 1, "test");  //test hhhhh0624

            //这是我添加的注释 用于测试。2017-6-19

            // test 2017-06-25
        }
    }
}

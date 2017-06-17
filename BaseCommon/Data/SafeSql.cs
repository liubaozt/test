using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseCommon.Data
{
    public class SafeSql
    {
        /// <summary>
        /// 检验用户提交的数据是否正常
        /// </summary>
        /// <param name="Str">用户提交的数据</param>
        /// <param name="type">1：更新 0：是查询</param>
        /// <returns></returns>
        public static bool ProcessSqlStr(string Str, int type)
        {
            string SqlStr;
            if (type == 1)
                SqlStr = "exec |insert |select |delete |update |count |chr |mid |master |truncate |char |declare ";
            else
                SqlStr = "'|and |exec |insert |select |delete |update |count |*|chr |mid |master |truncate |char |declare ";
            bool ReturnValue = true;
            try
            {
                if (Str != "")
                {
                    string[] anySqlStr = SqlStr.Split('|');
                    foreach (string ss in anySqlStr)
                    {
                        if (Str.IndexOf(ss) >= 0)
                        {
                            ReturnValue = false;
                        }
                    }
                }
            }
            catch
            {
                ReturnValue = false;
            }
            return ReturnValue;
        }

        public static string SqlOrderBy(string str, string type)
        {
            string[] strs = str.Split(',');
            string sql = "";
            for (int i = 0; i < strs.Length; i++)
            {
                sql += strs[i] + " " + type + ",";
            }
            sql = sql.Substring(0, sql.Length - 1);
            return sql;
        }


        public static string SafeSqlStr(string str)
        {
            str = str.Replace("'","");
            return str;
        }

    }
}

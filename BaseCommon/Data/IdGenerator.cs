using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;

namespace BaseCommon.Data
{
    public class IdGenerator
    {
        public static string GetMaxId(string tableName, string special = "")
        {
            string dateString = GetServerDate().ToString("yyyy-MM-dd");
            string sql = @"select * from AppId where tableName='" + tableName + "' and dateValue='" + dateString + "'";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            int index = 1;
            if (dt.Rows.Count > 0)
            {
                index = DataConvert.ToInt32(dt.Rows[0]["indexValue"]) + 1;
                sql = "update AppId set indexValue=" + index +
                    " where tableName='" + tableName + "' and dateValue='" + dateString + "'";
                AppMember.DbHelper.ExecuteSql(sql);
            }
            else
            {
                sql = "insert into AppId(id,tableName,indexValue,dateValue)" +
                    "values('" + Guid.NewGuid() + "','" + tableName + "'," + index + ",'" + dateString + "')";
                AppMember.DbHelper.ExecuteSql(sql);
            }
            dateString = GetServerDate().ToString("yyMMdd");
            string prefixStr = "";
            if (special == "")
            {
                if (AppMember.TablePrefix.ContainsKey(tableName.ToLower()))
                {
                    prefixStr = AppMember.TablePrefix[tableName.ToLower()];
                }
                else
                    prefixStr = tableName.Length > 3 ? tableName.Substring(0, 3) : tableName;
            }
            else
            {
                if (AppMember.TablePrefix.ContainsKey(special.ToLower()))
                {
                    prefixStr = AppMember.TablePrefix[special.ToLower()];
                }
            }
            int prelen = prefixStr.Length;
            string indexformat = "";
            for (int i = 0; i < 6 - prelen; i++)
            {
                indexformat += "0";
            }
            string strId = prefixStr + dateString + index.ToString(indexformat);
            return strId;
        }

   
        public static DateTime GetServerDate()
        {
            string sql = " select getdate() date";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            DateTime serverDate = DataConvert.ToDateTime(dt.Rows[0]["date"]);
            return serverDate;
        }
    }
}

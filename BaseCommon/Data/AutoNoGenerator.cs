using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;

namespace BaseCommon.Data
{
    public class AutoNoGenerator
    {
        public static string GetMaxNo(string tableName, bool hasPrefixStr = true, int numLength = 6,string defaultPrefixstr="FA")
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("tableName", tableName);
            string sql = @"select * from AppAutoNo where tableName='" + tableName + "'";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            int index = 1;
            string prefixStr = "";
            if (dt.Rows.Count > 0)
            {
                index = DataConvert.ToInt32(dt.Rows[0]["indexValue"]) + 1;
                paras.Add("indexValue", index);
                sql = @"update AppAutoNo set indexValue=@indexValue
                     where tableName=@tableName";
                AppMember.DbHelper.ExecuteSql(sql, paras);
                numLength = DataConvert.ToInt32(dt.Rows[0]["numLength"]);
                prefixStr = DataConvert.ToString(dt.Rows[0]["prefixStr"]);
            }
            else
            {
                paras.Add("autoNoId", Guid.NewGuid());
                paras.Add("indexValue", index);
                sql = @"insert into AppAutoNo(autoNoId,tableName,indexValue,prefixStr,numLength)
                    values(@autoNoId,@tableName,@indexValue,'FA'," + numLength.ToString() + ")";
                AppMember.DbHelper.ExecuteSql(sql, paras);
                prefixStr = defaultPrefixstr;
            }
            string zeroStr = "0";
            for (int i = 0; i < numLength; i++)
                zeroStr += "0";
            if (hasPrefixStr)
                return prefixStr + index.ToString(zeroStr);
            else
                return index.ToString(zeroStr);
        }

        public static string GetMaxNo(string tableName, string prefixStr, int numLength = 6)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("tableName", tableName);
            string sql = @"select * from AppAutoNo where tableName='" + tableName + "'";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            int index = 1;
            if (dt.Rows.Count > 0)
            {
                index = DataConvert.ToInt32(dt.Rows[0]["indexValue"]) + 1;
                paras.Add("indexValue", index);
                sql = @"update AppAutoNo set indexValue=@indexValue
                     where tableName=@tableName";
                AppMember.DbHelper.ExecuteSql(sql, paras);
                numLength = DataConvert.ToInt32(dt.Rows[0]["numLength"]);
                prefixStr = DataConvert.ToString(dt.Rows[0]["prefixStr"]);
            }
            else
            {
                paras.Add("autoNoId", Guid.NewGuid());
                paras.Add("indexValue", index);
                sql = @"insert into AppAutoNo(autoNoId,tableName,indexValue,prefixStr,numLength)
                    values(@autoNoId,@tableName,@indexValue,'" + prefixStr + "'," + numLength.ToString() + ")";
                AppMember.DbHelper.ExecuteSql(sql, paras);
            }
            string zeroStr = "";
            for (int i = 0; i < numLength; i++)
                zeroStr += "0";
            return prefixStr + index.ToString(zeroStr);

        }

    }
}

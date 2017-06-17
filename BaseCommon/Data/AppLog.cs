using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;

namespace BaseCommon.Data
{
    public class AppLog
    {
        public static int WriteLog(string userId, LogType logType, string logger, string message)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("LogDate", IdGenerator.GetServerDate().ToString("yyyy-MM-dd HH:mm:ss"));
            paras.Add("userId", userId);
            paras.Add("logType", logType.ToString());
            paras.Add("logger", logger);
            paras.Add("message", message);
            string sql = "insert into AppLog(LogDate,UserId,LogLevel,Logger,Message)" +
                     "values(@LogDate,@userId,@logType,@logger,@message)";
            return AppMember.DbHelper.ExecuteSql(sql,paras);
        }
    }
}

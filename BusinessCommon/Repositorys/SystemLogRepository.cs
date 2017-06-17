using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;
using System.Data;
using BaseCommon.Data;
using BusinessCommon.Models.SystemLog;

namespace BusinessCommon.Repositorys
{
    public class SystemLogRepository : IQuery
    {


        public virtual DataTable GetReportGridDataTable(ListCondition condition)
        {
            int rowSize = condition.PageIndex * condition.PageRowNum; //子查询返回行数的尺寸
            string sql = string.Format(@"select top 500 LogDate,
                        UserId,
                        Message
                from AppLog where LogLevel not in ('Error','Debug') {0} order by LogDate desc ", ListWhereSql(condition).Sql,
                                                                                                                                                                                   " order by   AssetsScrap.assetsScrapNo  ,Assets.assetsNo");
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, ListWhereSql(condition).DBPara).Tables[0];
            return dtGrid;
        }

        protected  WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            EntryModel model = JsonHelper.Deserialize<EntryModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.UserName) != "")
            {
                wcd.Sql += @" and AppLog.UserId  like '%'+@UserId+'%'";
                wcd.DBPara.Add("UserId", model.UserName);
            }
            if (DataConvert.ToString(model.LogMessage) != "")
            {
                wcd.Sql += @" and AppLog.Message  like '%'+@LogMessage+'%'";
                wcd.DBPara.Add("LogMessage", model.LogMessage);
            }
        
            if (DataConvert.ToString(model.LogDate1) != "")
            {
                wcd.Sql += @" and AppLog.LogDate>=@LogDate1";
                wcd.DBPara.Add("LogDate1", DataConvert.ToString(model.LogDate1) + " 00:00:00");
            }
            if (DataConvert.ToString(model.LogDate2) != "")
            {
                wcd.Sql += @" and AppLog.LogDate<=@LogDate2";
                wcd.DBPara.Add("LogDate2", DataConvert.ToString(model.LogDate2) + " 23:59:59");
            }
            return wcd;
        }



    }
}

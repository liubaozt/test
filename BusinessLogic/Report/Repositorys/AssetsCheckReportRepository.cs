using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Report;
using BaseCommon.Basic;
using System.Data;
using BaseCommon.Data;
using BusinessLogic.Report.Models.AssetsCheckReport;

namespace BusinessLogic.Report.Repositorys
{
    public class AssetsCheckReportRepository : IQueryReport
    {


        public virtual DataTable GetReportGridDataTable(ListCondition condition)
        {
            int rowSize = condition.PageIndex * condition.PageRowNum; //子查询返回行数的尺寸
            string sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        AssetsCheck.assetsCheckNo assetsCheckNo,
                        AssetsCheck.assetsCheckName assetsCheckName,
                       (select userName from AppUser where AssetsCheck.checkPeople=AppUser.userId) checkPeople,
                        AssetsCheck.checkDate checkDate,
                        AssetsCheckDetail.remark Remark
                from AssetsCheckDetail,Assets,AssetsCheck where AssetsCheckDetail.assetsId=Assets.assetsId  and AssetsCheckDetail.assetsCheckId=AssetsCheck.assetsCheckId {0} ", ListWhereSql(condition).Sql);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, ListWhereSql(condition).DBPara).Tables[0];
            return dtGrid;
        }

        protected  WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            EntryModel model = JsonHelper.Deserialize<EntryModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.AssetsCheckNo) != "")
            {
                wcd.Sql += @" and AssetsCheck.assetsCheckNo  like '%'+@assetsCheckNo+'%'";
                wcd.DBPara.Add("assetsCheckNo", model.AssetsCheckNo);
            }
            if (DataConvert.ToString(model.AssetsCheckName) != "")
            {
                wcd.Sql += @" and AssetsCheck.assetsCheckName  like '%'+@assetsCheckName+'%'";
                wcd.DBPara.Add("assetsCheckName", model.AssetsCheckName);
            }
            if (DataConvert.ToString(model.AssetsNo) != "")
            {
                wcd.Sql += @" and Assets.assetsNo  like '%'+@assetsNo+'%'";
                wcd.DBPara.Add("assetsNo", model.AssetsNo);
            }
            if (DataConvert.ToString(model.AssetsName) != "")
            {
                wcd.Sql += @" and Assets.assetsName  like '%'+@assetsName+'%'";
                wcd.DBPara.Add("assetsName", model.AssetsName);
            }
           
            return wcd;
        }



    }
}

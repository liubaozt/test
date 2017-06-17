using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;
using System.Data;
using BaseCommon.Data;
using BusinessLogic.Report.Models.UpcomingGuaranteeQuery;

namespace BusinessLogic.Report.Repositorys
{
    public class UpcomingGuaranteeQueryRepository : IQuery
    {

        public virtual DataTable GetReportGridDataTable(ListCondition condition)
        {
            int rowSize = condition.PageIndex * condition.PageRowNum; //子查询返回行数的尺寸
            string sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
	                (select departmentName from AppDepartment where Assets.departmentId=AppDepartment.departmentId) departmentId,
                   (select storeSiteName from StoreSite where Assets.storeSiteId=StoreSite.storeSiteId) storeSiteId,
	                (select userName from AppUser where Assets.usePeople=AppUser.userId) usePeople,
	                (select userName from AppUser where Assets.keeper=AppUser.userId) keeper,
                                convert(nvarchar(100),   Assets.purchaseDate ,23) purchaseDate,
	                 Assets.guaranteeDays guaranteeDays,
                     convert(nvarchar(100), DATEADD(dd,Assets.guaranteeDays,Assets.purchaseDate) ,23)   endDate
                from Assets 
                where 1=1 {0} {1} ", ListWhereSql(condition).Sql,
                 " order by Assets.assetsNo ");
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, ListWhereSql(condition).DBPara).Tables[0];
            return dtGrid;
        }

        protected  WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            EntryModel model = JsonHelper.Deserialize<EntryModel>(condition.ListModelString);
            if (model == null) return wcd;
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
            if (DataConvert.ToString(model.DepartmentId) != "")
            {
                wcd.Sql += @" and Assets.departmentId=@departmentId";
                wcd.DBPara.Add("departmentId", model.DepartmentId);
            }
            if (DataConvert.ToString(model.StoreSiteId) != "")
            {
                wcd.Sql += @" and Assets.storeSiteId=@storeSiteId";
                wcd.DBPara.Add("storeSiteId", model.StoreSiteId);
            }
            if (DataConvert.ToString(model.RemindDays) != "")
            {
                DateTime time = IdGenerator.GetServerDate();
                wcd.Sql += @" and datediff(day,'" + time .ToShortDateString()+ "', DATEADD(dd,Assets.guaranteeDays,Assets.purchaseDate))<@remindDays ";
                wcd.DBPara.Add("remindDays", model.RemindDays);
            }
            return wcd;
        }



    }
}

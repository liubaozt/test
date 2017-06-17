using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;
using System.Data;
using BaseCommon.Data;
using BusinessLogic.Report.Models.AssetsCheckQuery;

namespace BusinessLogic.Report.Repositorys
{
    public class AssetsCheckQueryRepository : IQuery
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
                         convert(nvarchar(100),   AssetsCheckDetail.checkDate ,23)  checkDate,
                      convert(nvarchar(100),    AssetsCheckDetail.actualCheckDate ,23)  actualCheckDate,
                        (select storeSiteName from StoreSite where StoreSite.storeSiteId=AssetsCheckDetail.storeSiteId) storeSiteId,  
                        (select storeSiteName from StoreSite where StoreSite.storeSiteId=AssetsCheckDetail.actualStoreSiteId) actualStoreSiteId,                       
                        (select CodeTable.codeName from CodeTable where AssetsCheckDetail.checkResult=CodeTable.codeNo and CodeTable.codeType='CheckResult' and CodeTable.languageVer='{0}' ) checkResult,
                        AssetsCheckDetail.isFinished isFinished,
                        AssetsCheckDetail.remark Remark
                       from AssetsCheckDetail,Assets,AssetsCheck 
                      where AssetsCheckDetail.assetsId=Assets.assetsId  
                        and AssetsCheckDetail.assetsCheckId=AssetsCheck.assetsCheckId 
                        and (AssetsCheckDetail.approveState='E' or AssetsCheckDetail.approveState is null or AssetsCheckDetail.approveState='') 
                        {1} {2} ", AppMember.AppLanguage.ToString(), ListWhereSql(condition).Sql,
                                                                                                                                                                                   " order by AssetsCheck.assetsCheckNo,Assets.assetsNo ");
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

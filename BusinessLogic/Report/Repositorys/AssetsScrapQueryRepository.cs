using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;
using System.Data;
using BaseCommon.Data;
using BusinessLogic.Report.Models.AssetsScrapQuery;

namespace BusinessLogic.Report.Repositorys
{
    public class AssetsScrapQueryRepository : IQuery
    {


        public virtual DataTable GetReportGridDataTable(ListCondition condition)
        {
            int rowSize = condition.PageIndex * condition.PageRowNum; //子查询返回行数的尺寸
            string sql = string.Format(@"select  Assets.assetsId assetsId,
                        Assets.assetsNo assetsNo,
                        Assets.assetsName assetsName,
                        AssetsScrap.assetsScrapNo assetsScrapNo,
                        AssetsScrap.assetsScrapName assetsScrapName,
                        (select assetsClassName from AssetsClass where Assets.assetsClassId=AssetsClass.assetsClassId) assetsClassId,
                 (select assetsTypeName from AssetsType where Assets.assetsTypeId=AssetsType.assetsTypeId) assetsTypeId,
                (select departmentName from AppDepartment where Assets.departmentId=AppDepartment.departmentId) departmentId,
                  (select scrapTypeName from ScrapType where AssetsScrapDetail.scrapTypeId=ScrapType.scrapTypeId)   ScrapTypeId,
                 convert(nvarchar(100),  AssetsScrapDetail.scrapDate,23) ScrapDate,
                    AssetsScrapDetail.remark Remark
                from AssetsScrapDetail,Assets,AssetsScrap 
                where AssetsScrapDetail.assetsId=Assets.assetsId  
                and AssetsScrapDetail.assetsScrapId=AssetsScrap.assetsScrapId 
                and (AssetsScrapDetail.approveState='E' or AssetsScrapDetail.approveState is null  or AssetsScrapDetail.approveState='') 
                {0} {1} ", ListWhereSql(condition).Sql,
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
            if (DataConvert.ToString(model.AssetsScrapNo) != "")
            {
                wcd.Sql += @" and AssetsScrap.assetsScrapNo  like '%'+@assetsScrapNo+'%'";
                wcd.DBPara.Add("assetsScrapNo", model.AssetsScrapNo);
            }
            if (DataConvert.ToString(model.AssetsScrapName) != "")
            {
                wcd.Sql += @" and AssetsScrap.assetsScrapName  like '%'+@assetsScrapName+'%'";
                wcd.DBPara.Add("assetsScrapName", model.AssetsScrapName);
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
            if (DataConvert.ToString(model.AssetsTypeId) != "")
            {
                wcd.Sql += @" and Assets.assetsTypeId  like '%'+@assetsTypeId+'%'";
                wcd.DBPara.Add("assetsTypeId", model.AssetsTypeId);
            }
            if (DataConvert.ToString(model.ScrapTypeId) != "")
            {
                wcd.Sql += @" and AssetsScrapDetail.scrapTypeId  like '%'+@scrapTypeId+'%'";
                wcd.DBPara.Add("scrapTypeId", model.ScrapTypeId);
            }
            if (DataConvert.ToString(model.AssetsClassId) != "")
            {
                wcd.Sql += @" and  Assets.assetsClassId  like '%'+@assetsClassId+'%'";
                wcd.DBPara.Add("assetsClassId", model.AssetsClassId);
            }
            if (DataConvert.ToString(model.DepartmentId) != "")
            {
                wcd.Sql += @" and Assets.departmentId  like '%'+@departmentId+'%'";
                wcd.DBPara.Add("departmentId", model.DepartmentId);
            }
            if (DataConvert.ToString(model.ScrapDate1) != "")
            {
                wcd.Sql += @" and AssetsScrapDetail.scrapDate>=@scrapDate1";
                wcd.DBPara.Add("scrapDate1", DataConvert.ToString(model.ScrapDate1) + " 00:00:00");
            }
            if (DataConvert.ToString(model.ScrapDate2) != "")
            {
                wcd.Sql += @" and AssetsScrapDetail.scrapDate<=@scrapDate2";
                wcd.DBPara.Add("scrapDate2", DataConvert.ToString(model.ScrapDate2) + " 23:59:59");
            }
            return wcd;
        }



    }
}

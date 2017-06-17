using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Data;
using log4net;
using System.Reflection;
using System.Data;
using System.Data.Common;

namespace BaseCommon.Basic
{
    public abstract class ListRepository : BaseRepository
    {
      

        protected abstract string WhereSql(Dictionary<string, object> paras);

        protected abstract string SubSelectSql(Dictionary<string, object> paras);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paras"></param>
        /// <param name="isInnerOrder">内部的排序用相反排序，外部正常排序</param>
        /// <returns></returns>
        protected virtual string ProcessOrderBy(Dictionary<string, object> paras, int layer, string defaultSort)
        {
            string sql = defaultSort;
            string sortField = DataConvert.ToString(paras["sortField"]);
            string sortType = DataConvert.ToString(paras["sortType"]);
            if (sortField != "")
            {
                if (SafeSql.ProcessSqlStr(sortField, 0) && SafeSql.ProcessSqlStr(sortType, 0))
                {
                    if (layer == 2)
                    {
                        if (sortType == "asc")
                            sql = string.Format(" order by  {0} desc ", sortField);
                        else
                        {
                            sql = string.Format(" order by  {0}  ", sortField);
                        }
                    }
                    else
                    {
                        sql = string.Format(" order by  {0} {1} ", sortField, sortType);
                    }
                }
            }
            return sql;
        }

        protected string DefaulteGridSortField { get; set; }

        public virtual DataTable GetGridDataTable(Dictionary<string, object> paras)
        {
            int pageIndex = DataConvert.ToInt32(paras["pageIndex"]);
            int rowNum = DataConvert.ToInt32(paras["rowNum"]);
            int totalRownum = DataConvert.ToInt32(paras["totalRownum"]);
            int topNum = 0;
            if (pageIndex * rowNum > totalRownum)
                topNum = totalRownum - (pageIndex - 1) * rowNum;
            else
                topNum = rowNum;
            string sql = string.Format(@" select * from 
                                            (select top {0} * from
                                               ({1}{2})T 
                                             {3})F {4}  ", DataConvert.ToString(topNum),
                                           SubSelectSql(paras),
                                           ProcessOrderBy(paras, 1, string.Format(@"order by {0} ", DefaulteGridSortField)),
                                           ProcessOrderBy(paras, 2, string.Format(@"order by {0} desc ", DefaulteGridSortField)),
                                           ProcessOrderBy(paras, 3, string.Format(@"order by {0} ", DefaulteGridSortField)));
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

    }
}

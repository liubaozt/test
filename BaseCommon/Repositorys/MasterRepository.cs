using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Data;
using System.Reflection;
using System.Data;
using System.Data.Common;
using BaseCommon.Basic;
using BaseCommon.Models;

namespace BaseCommon.Repositorys
{
    public abstract class MasterRepository : BaseRepository, IEntry, ILoadList
    {
       
        public virtual DataTable GetDropListSource()
        {
            throw new NotImplementedException();
        }

        public virtual List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            throw new NotImplementedException();
        }


        public virtual int Update(EntryViewModel model, UserInfo sysUser, string mode, string pkValue, string viewTitle)
        {
            try
            {
                switch (mode)
                {
                    case "new":
                        return Add(model, sysUser, viewTitle + "[" + AppMember.AppText["new"].ToString() + "]");
                    case "new2":
                        return Add(model, sysUser, viewTitle);
                    case "edit":
                        return Modified(model, sysUser, pkValue, viewTitle + "[" + AppMember.AppText["edit"].ToString() + "]", mode);
                    case "actual":
                        return Modified(model, sysUser, pkValue, viewTitle + "[" + AppMember.AppText["actual"].ToString() + "]", mode);
                    case "rename":
                        return Modified(model, sysUser, pkValue, viewTitle + "[" + AppMember.AppText["rename"].ToString() + "]", mode);
                    case "delete":
                        {
                            int ret = Delete(model, sysUser, pkValue, viewTitle);
                            if (ret > 0)
                            {
                                //AppLog.WriteLog(sysUser.UserNo, LogType.Info, "Delete", sysUser.UserNo + " delete data that pk=" + pkValue + ".");
                            }
                            return ret;
                        }
                    case "reapply":
                        return Modified(model, sysUser, pkValue, viewTitle , mode);
                    default:
                        return 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        protected abstract int Add(EntryViewModel model, UserInfo sysUser, string viewTitle);

        protected abstract int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode);

        protected abstract int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle);

        protected abstract WhereConditon ListWhereSql(ListCondition condition);
       

        protected abstract string ListSql(ListCondition condition);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paras"></param>
        /// <param name="isInnerOrder">内部的排序用相反排序，外部正常排序</param>
        /// <returns></returns>
        private string OrderBy(ListCondition condition, int layer, string defaultSort)
        {
            string sql = defaultSort;
            string sortField = condition.SortField;
            string sortType = condition.SortType;
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

        protected string MasterTable { get; set; }


        #region ILoadList 成员

        public virtual int GetGridDataCount(ListCondition condition)
        {
//            string sql = string.Format(@"select count(*) cnt
//                          from {0}  where 1=1 {1}", MasterTable,ListWhereSql(condition).Sql);
            string sql =ListSql(condition)+ListWhereSql(condition).Sql;
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, ListWhereSql(condition).DBPara).Tables[0];
            return dtGrid.Rows.Count;
        }

      

        public virtual DataTable GetGridDataTable(ListCondition condition)
        {
            int topNum = 0; //当前页显示的条数
            if (condition.PageIndex * condition.PageRowNum > condition.TotalRowNum)
                topNum = condition.TotalRowNum - (condition.PageIndex - 1) * condition.PageRowNum;
            else
                topNum = condition.PageRowNum;
            int rowSize = condition.PageIndex * condition.PageRowNum; //当前视图需要的尺寸
            string sql = string.Format(@" select * from 
                                            (select top {0} * from
                                               (select top {1} * from ({2} {3} )P {4})T 
                                             {5})F {6}  ", DataConvert.ToString(topNum),
                                           rowSize,
                                           ListSql(condition),
                                           ListWhereSql(condition).Sql,
                                           OrderBy(condition, 1, string.Format(@"order by {0} desc ", DefaulteGridSortField)),
                                           OrderBy(condition, 2, string.Format(@"order by {0} ", DefaulteGridSortField)),
                                           OrderBy(condition, 3, string.Format(@"order by {0} desc", DefaulteGridSortField)));
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, ListWhereSql(condition).DBPara).Tables[0];
            return dtGrid;
        }

        public virtual DataTable GetAllGridDataTable(ListCondition condition)
        {
            string sql = string.Format(@" {0} {1} {2} ",
                                           ListSql(condition),
                                           ListWhereSql(condition).Sql,
                                           OrderBy(condition, 1, string.Format(@"order by {0} ", DefaulteGridSortField)));
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, ListWhereSql(condition).DBPara).Tables[0];
            return dtGrid;
        }

        #endregion


    }
}

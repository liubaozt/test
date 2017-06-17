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
    public abstract class MasterRepository : BaseRepository,IEntry, ILoadList
    {

        public virtual int Update(Dictionary<string, object> objs, UserInfo sysUser, string mode, string pkValue, string viewTitle)
        {
            try
            {
                switch (mode)
                {
                    case "new":
                    case "new2":
                        return AddData(objs, sysUser, viewTitle);
                    case "edit":
                        return EditData(objs, sysUser, pkValue, viewTitle, mode);
                    case "delete":
                        {
                            int ret = DeleteData(objs, sysUser, pkValue);
                            if (ret > 0)
                            {
                                ILog log = LogManager.GetLogger(this.GetType());
                                log.Info(sysUser.UserNo + " delete data that pk=" + pkValue + ".");
                            }
                            return ret;
                        }
                    case "reapply":
                        return EditData(objs, sysUser, pkValue, viewTitle, mode);
                    default:
                        return 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        protected abstract int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle);

        protected abstract int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode);

        protected abstract int DeleteData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue);

        protected abstract string WhereSql(Dictionary<string, object> paras);

        protected abstract string SubSelectSql(Dictionary<string, object> paras);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paras"></param>
        /// <param name="isInnerOrder">内部的排序用相反排序，外部正常排序</param>
        /// <returns></returns>
        private  string OrderBy(Dictionary<string, object> paras, int layer, string defaultSort)
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


        #region ILoadList 成员

        public virtual int GetGridDataCount(Dictionary<string, object> paras)
        {
            throw new NotImplementedException();
        }

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
                                           OrderBy(paras, 1, string.Format(@"order by {0} ", DefaulteGridSortField)),
                                           OrderBy(paras, 2, string.Format(@"order by {0} desc ", DefaulteGridSortField)),
                                           OrderBy(paras, 3, string.Format(@"order by {0} ", DefaulteGridSortField)));
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

        #endregion

      
    }
}

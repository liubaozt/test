using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Collections;
using System.Data.OleDb;
using BaseCommon.Repositorys;
using BusinessLogic.BasicData.Models.AutoPeriod;

namespace BusinessLogic.BasicData.Repositorys
{

    public class AutoPeriodRepository : BaseRepository
    {


        public int Update(UserInfo sysUser, EntryModel model)
        {
            List<DataTable> dtlist = new List<DataTable>();
            string sql = @"select * from FiscalYear where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "FiscalYear";

            int formYear = DataConvert.ToInt32(model.FromYear);
            int toYear = DataConvert.ToInt32(model.ToYear);

            string sql2 = @"select * from FiscalPeriod where 1<>1 ";
            DataTable dt2 = AppMember.DbHelper.GetDataSet(sql2).Tables[0];
            dt2.TableName = "FiscalPeriod";

            for (int i = formYear; i <= toYear; i++)
            {
                DataRow dr = dt.NewRow();
                string id = IdGenerator.GetMaxId(dt.TableName);
                dr["fiscalYearId"] = id;
                dr["fiscalYearName"] = i.ToString();
                dr["fromDate"] = i.ToString() + "/01/01";
                dr["toDate"] = i.ToString() + "/12/31";
                dt.Rows.Add(dr);
                Create5Field(dt, sysUser.UserId, model.ViewTitle);
                UpdateFiscalPeriod(sysUser, model, i, id, dt2);

            }
            dtlist.Add(dt);
            dtlist.Add(dt2);
            DbUpdate = new DataUpdate();
            return DbUpdate.Update(dtlist, true);
        }

        private void UpdateFiscalPeriod(UserInfo sysUser, EntryModel model, int year, string fiscalYearId, DataTable dt2)
        {
            for (int i = 1; i <= 12; i++)
            {
                DataRow dr = dt2.NewRow();
                string id = IdGenerator.GetMaxId(dt2.TableName);
                dr["fiscalPeriodId"] = id;
                dr["fiscalPeriodName"] = i.ToString();
                dr["fiscalYearId"] = fiscalYearId;
                dr["fromDate"] = year+"/"+i.ToString("00") + "/01";
                if (i == 12)
                    dr["toDate"] = year + "/12/31";
                else
                    dr["toDate"] = DateTime.Parse(year + "/" + (i + 1).ToString("00") + "/01").AddDays(-1).ToString("yyyy/MM/dd");
                dt2.Rows.Add(dr);
                Create5Field(dt2, sysUser.UserId, model.ViewTitle);
            }
        }

    }
}

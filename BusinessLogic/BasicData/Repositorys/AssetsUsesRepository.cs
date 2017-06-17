using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.BasicData.Models.AssetsUses;
using BaseCommon.Models;

namespace BusinessLogic.BasicData.Repositorys
{
    public class AssetsUsesRepository : MasterRepository
    {

        public AssetsUsesRepository()
        {
            DefaulteGridSortField = "assetsUsesNo";
            MasterTable = "AssetsUses";
        }



        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            ListModel model = JsonHelper.Deserialize<ListModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.AssetsUsesNo) != "")
            {
                wcd.Sql += @" and AssetsUses.assetsUsesNo like '%'+@assetsUsesNo+'%'";
                wcd.DBPara.Add("assetsUsesNo", model.AssetsUsesNo);
            }
            if (DataConvert.ToString(model.AssetsUsesName) != "")
            {
                wcd.Sql += @" and AssetsUses.assetsUsesName like '%'+@assetsUsesName+'%'";
                wcd.DBPara.Add("assetsUsesName", model.AssetsUsesName);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            string subViewSql = string.Format(@"select  AssetsUses.assetsUsesId assetsUsesId,
                                 AssetsUses.assetsUsesNo assetsUsesNo,
                                 AssetsUses.assetsUsesName assetsUsesName,
                                 U1.userName createId ,
                                 AssetsUses.createTime createTime ,
                                 U2.userName updateId ,
                                 AssetsUses.updateTime updateTime ,
                                 AssetsUses.updatePro updatePro
                          from AssetsUses left join AppUser U1 on AssetsUses.createId=U1.userId
                                    left join AppUser U2 on AssetsUses.updateId=U2.userId 
                          where 1=1 ");
            return subViewSql;
        }



        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            if (formMode != "new")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("assetsUsesId", primaryKey);
                string sql = @"select * from AssetsUses where assetsUsesId=@assetsUsesId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.AssetsUsesId = primaryKey;
                model.AssetsUsesNo = DataConvert.ToString(dr["assetsUsesNo"]);
                model.AssetsUsesName = DataConvert.ToString(dr["assetsUsesName"]);
            }
        }


        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsUses where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsUses";
            DataRow dr = dt.NewRow();
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dr);
            dr["assetsUsesId"] = IdGenerator.GetMaxId(dt.TableName);
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsUsesId", pkValue);
            string sql = @"select * from AssetsUses where assetsUsesId=@assetsUsesId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsUses";
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dt.Rows[0]);
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsUsesId", pkValue);
            string sql = @"select * from AssetsUses where assetsUsesId=@assetsUsesId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsUses";
            dt.Rows[0].Delete();
            return DbUpdate.Update(dt);
        }

        protected void SetDataRow(EntryModel model, DataRow dr)
        {
            dr["assetsUsesNo"] = model.AssetsUsesNo;
            dr["assetsUsesName"] = model.AssetsUsesName;
        }


        public override DataTable GetDropListSource()
        {
            string sql = @"select * from AssetsUses  order by assetsUsesName ";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dtGrid;
        }

        public override List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Select(filterExpression))
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["assetsUsesId"]);
                dropList.Text = DataConvert.ToString(dr["assetsUsesName"]);
                list.Add(dropList);
            }
            return list;
        }


    }
}

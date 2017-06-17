using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.BasicData.Models.ScrapType;
using BaseCommon.Models;

namespace BusinessLogic.BasicData.Repositorys
{
    public class ScrapTypeRepository : MasterRepository
    {

        public ScrapTypeRepository()
        {
            DefaulteGridSortField = "scrapTypeNo";
            MasterTable = "ScrapType";
        }

        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            ListModel model = JsonHelper.Deserialize<ListModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.ScrapTypeNo) != "")
            {
                wcd.Sql += @" and ScrapType.scrapTypeNo like '%'+@scrapTypeNo+'%'";
                wcd.DBPara.Add("scrapTypeNo", model.ScrapTypeNo);
            }
            if (DataConvert.ToString(model.ScrapTypeName) != "")
            {
                wcd.Sql += @" and ScrapType.scrapTypeName like '%'+@scrapTypeName+'%'";
                wcd.DBPara.Add("scrapTypeName", model.ScrapTypeName);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            string subViewSql = string.Format(@"select ScrapType.scrapTypeId scrapTypeId,
                                 ScrapType.scrapTypeNo scrapTypeNo,
                                 ScrapType.scrapTypeName scrapTypeName,
                                 U1.userName createId ,
                                 ScrapType.createTime createTime ,
                                 U2.userName updateId ,
                                 ScrapType.updateTime updateTime ,
                                 ScrapType.updatePro updatePro
                          from ScrapType left join AppUser U1 on ScrapType.createId=U1.userId
                                    left join AppUser U2 on ScrapType.updateId=U2.userId 
                          where 1=1 ");
            return subViewSql;
        }



        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            if (formMode != "new")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("scrapTypeId", primaryKey);
                string sql = @"select * from ScrapType where scrapTypeId=@scrapTypeId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.ScrapTypeId = primaryKey;
                model.ScrapTypeNo = DataConvert.ToString(dr["scrapTypeNo"]);
                model.ScrapTypeName = DataConvert.ToString(dr["scrapTypeName"]);
            }
        }


        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from ScrapType where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "ScrapType";
            DataRow dr = dt.NewRow();
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dr);
            dr["scrapTypeId"] = IdGenerator.GetMaxId(dt.TableName);
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("scrapTypeId", pkValue);
            string sql = @"select * from ScrapType where scrapTypeId=@scrapTypeId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "ScrapType";
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dt.Rows[0]);
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("scrapTypeId", pkValue);
            string sql = @"select * from ScrapType where scrapTypeId=@scrapTypeId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "ScrapType";
            dt.Rows[0].Delete();
            return DbUpdate.Update(dt);
        }

        protected void SetDataRow(EntryModel model, DataRow dr)
        {
            dr["scrapTypeNo"] = model.ScrapTypeNo;
            dr["scrapTypeName"] = model.ScrapTypeName;
        }

        public override DataTable GetDropListSource()
        {
            string sql = @"select * from ScrapType  order by scrapTypeName";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dtGrid;
        }


        public override List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Select(filterExpression))
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["scrapTypeId"]);
                dropList.Text = DataConvert.ToString(dr["scrapTypeName"]);
                list.Add(dropList);
            }
            return list;
        }

    }
}

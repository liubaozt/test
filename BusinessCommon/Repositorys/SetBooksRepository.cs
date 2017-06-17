using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BusinessCommon.Models.SetBooks;
using BaseCommon.Repositorys;
using BaseCommon.Models;

namespace BusinessCommon.Repositorys
{
    
    public class SetBooksRepository : MasterRepository
    {

        public SetBooksRepository()
        {
            DefaulteGridSortField = "setBooksNo";
            MasterTable = "SetBooks";
        }

        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            ListModel model = JsonHelper.Deserialize<ListModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.SetBooksNo) != "")
            {
                wcd.Sql += @" and SetBooks.setBooksNo like '%'+@setBooksNo+'%'";
                wcd.DBPara.Add("setBooksNo", model.SetBooksNo);
            }
            if (DataConvert.ToString(model.SetBooksName) != "")
            {
                wcd.Sql += @" and SetBooks.setBooksName like '%'+@setBooksName+'%'";
                wcd.DBPara.Add("setBooksName", model.SetBooksName);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            string subViewSql = string.Format(@"select  SetBooks.setBooksId setBooksId,
                                 SetBooks.setBooksNo setBooksNo,
                                 SetBooks.setBooksName setBooksName,
                                 SetBooks.remark remark,
                                (select CodeTable.codeName from CodeTable where SetBooks.isFixed=CodeTable.codeNo and CodeTable.codeType='BoolVal' and CodeTable.languageVer='{0}' ) isFixed,
                                 U1.userName createId ,
                                 SetBooks.createTime createTime ,
                                 U2.userName updateId ,
                                 SetBooks.updateTime updateTime ,
                                 SetBooks.updatePro updatePro
                          from SetBooks left join AppUser U1 on SetBooks.createId=U1.userId
                                       left join AppUser U2 on SetBooks.updateId=U2.userId
                          where 1=1 ", AppMember.AppLanguage.ToString());
            return subViewSql;
        }



        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            if (formMode != "new")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("setBooksId", primaryKey);
                string sql = @"select * from SetBooks where setBooksId=@setBooksId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.SetBooksId = primaryKey;
                model.SetBooksNo = DataConvert.ToString(dr["setBooksNo"]);
                model.SetBooksName = DataConvert.ToString(dr["setBooksName"]);
                model.Remark = DataConvert.ToString(dr["remark"]);
                model.IsFixed = DataConvert.ToString(dr["isFixed"]); 
            }
        }


        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from SetBooks where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "SetBooks";
            DataRow dr = dt.NewRow();
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dr);
            dr["setBooksId"] = IdGenerator.GetMaxId(dt.TableName);
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("setBooksId", pkValue);
            string sql = @"select * from SetBooks where setBooksId=@setBooksId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "SetBooks";
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dt.Rows[0]);
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("setBooksId", pkValue);
            string sql = @"select * from SetBooks where setBooksId=@setBooksId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "SetBooks";
            dt.Rows[0].Delete();
            return DbUpdate.Update(dt);
        }

        protected void SetDataRow(EntryModel model,DataRow dr)
        {
            dr["setBooksNo"] = model.SetBooksNo;
            dr["setBooksName"] = model.SetBooksName;
            dr["remark"] = model.Remark;
        }

        public override DataTable GetDropListSource()
        {
            string sql = @"select * from SetBooks  order by setBooksNo ";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dtGrid;
        }

        public CurSetBooks GetCurSetBooks(string setBooksId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("setBooksId", setBooksId);
            string sql = @"select * from SetBooks where setBooksId=@setBooksId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            CurSetBooks curSetBooks = new CurSetBooks();
            curSetBooks.SetBooksId = setBooksId;
            curSetBooks.SetBooksNo =DataConvert.ToString( dt.Rows[0]["setBooksNo"]);
            curSetBooks.SetBooksName = DataConvert.ToString(dt.Rows[0]["setBooksName"]);
            return curSetBooks;
        }

        public override List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Select(filterExpression))
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["setBooksId"]);
                dropList.Text = DataConvert.ToString(dr["setBooksName"]);
                list.Add(dropList);
            }
            return list;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessCommon.Models.Post;
using BaseCommon.Models;

namespace BusinessCommon.Repositorys
{
    public class PostRepository : MasterRepository
    {

        public PostRepository()
        {
            DefaulteGridSortField = "postNo";
            MasterTable = "AppPost";
        }



        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            ListModel model = JsonHelper.Deserialize<ListModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.PostNo) != "")
            {
                wcd.Sql += @" and AppPost.postNo like '%'+@postNo+'%'";
                wcd.DBPara.Add("postNo", model.PostNo);
            }
            if (DataConvert.ToString(model.PostName) != "")
            {
                wcd.Sql += @" and AppPost.postName like '%'+@postName+'%'";
                wcd.DBPara.Add("postName", model.PostName);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            string subViewSql = string.Format(@"select AppPost.postId postId,
                                 AppPost.postNo postNo,
                                 AppPost.postName postName,
                                 U1.userName createId ,
                                 AppPost.createTime createTime ,
                                 U2.userName updateId ,
                                 AppPost.updateTime updateTime ,
                                 AppPost.updatePro updatePro
                          from AppPost left join AppUser U1 on AppPost.createId=U1.userId
                                    left join AppUser U2 on AppPost.updateId=U2.userId 
                          where 1=1 ");
            return subViewSql;
        }



        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            if (formMode != "new")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("postId", primaryKey);
                string sql = @"select * from AppPost where postId=@postId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.PostId = primaryKey;
                model.PostNo = DataConvert.ToString(dr["postNo"]);
                model.PostName = DataConvert.ToString(dr["postName"]);
            }
        }


        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AppPost where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AppPost";
            DataRow dr = dt.NewRow();
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dr);
            dr["postId"] = IdGenerator.GetMaxId(dt.TableName);
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("postId", pkValue);
            string sql = @"select * from AppPost where postId=@postId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppPost";
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dt.Rows[0]);
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("postId", pkValue);
            string sql = @"select * from AppPost where postId=@postId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AppPost";
            dt.Rows[0].Delete();
            return DbUpdate.Update(dt);
        }

        protected void SetDataRow(EntryModel model, DataRow dr)
        {
            dr["postNo"] = model.PostNo;
            dr["postName"] = model.PostName;
        }


        public override DataTable GetDropListSource()
        {
            string sql = @"select * from AppPost  order by postName";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dtGrid;
        }

        public override List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Select(filterExpression))
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["postId"]);
                dropList.Text = DataConvert.ToString(dr["postName"]);
                list.Add(dropList);
            }
            return list;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.BasicData.Models.AssetsType;
using BaseCommon.Models;

namespace BusinessLogic.BasicData.Repositorys
{
    public class AssetsTypeRepository : MasterRepository
    {

        public AssetsTypeRepository()
        {
            DefaulteGridSortField = "assetsTypeNo";
            MasterTable = "AssetsType";
        }



        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            ListModel model = JsonHelper.Deserialize<ListModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.AssetsTypeNo) != "")
            {
                wcd.Sql += @" and AssetsType.assetsTypeNo like '%'+@assetsTypeNo+'%'";
                wcd.DBPara.Add("assetsTypeNo", model.AssetsTypeNo);
            }
            if (DataConvert.ToString(model.AssetsTypeName) != "")
            {
                wcd.Sql += @" and AssetsType.assetsTypeName like '%'+@assetsTypeName+'%'";
                wcd.DBPara.Add("assetsTypeName", model.AssetsTypeName);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            string subViewSql = string.Format(@"select AssetsType.assetsTypeId assetsTypeId,
                                 AssetsType.assetsTypeNo assetsTypeNo,
                                 AssetsType.assetsTypeName assetsTypeName,
                                 U1.userName createId ,
                                 AssetsType.createTime createTime ,
                                 U2.userName updateId ,
                                 AssetsType.updateTime updateTime ,
                                 AssetsType.updatePro updatePro
                          from AssetsType left join AppUser U1 on AssetsType.createId=U1.userId
                                    left join AppUser U2 on AssetsType.updateId=U2.userId 
                          where 1=1 ");
            return subViewSql;
        }



        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            if (formMode != "new")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("assetsTypeId", primaryKey);
                string sql = @"select * from AssetsType where assetsTypeId=@assetsTypeId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.AssetsTypeId = primaryKey;
                model.AssetsTypeNo = DataConvert.ToString(dr["assetsTypeNo"]);
                model.AssetsTypeName = DataConvert.ToString(dr["assetsTypeName"]);
            }
        }


        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsType where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsType";
            DataRow dr = dt.NewRow();
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dr);
            dr["assetsTypeId"] = IdGenerator.GetMaxId(dt.TableName);
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsTypeId", pkValue);
            string sql = @"select * from AssetsType where assetsTypeId=@assetsTypeId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsType";
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dt.Rows[0]);
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsTypeId", pkValue);
            string sql = @"select * from AssetsType where assetsTypeId=@assetsTypeId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsType";
            dt.Rows[0].Delete();
            return DbUpdate.Update(dt);
        }

        protected void SetDataRow(EntryModel model, DataRow dr)
        {
            dr["assetsTypeNo"] = model.AssetsTypeNo;
            dr["assetsTypeName"] = model.AssetsTypeName;
        }

        public override DataTable GetDropListSource()
        {
            string sql = @"select * from AssetsType  order by assetsTypeName ";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dtGrid;
        }

        public override List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Select(filterExpression))
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["assetsTypeId"]);
                dropList.Text = DataConvert.ToString(dr["assetsTypeName"]);
                list.Add(dropList);
            }
            return list;
        }

    }
}

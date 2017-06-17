using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using BaseCommon.Repositorys;
using BusinessLogic.BasicData.Models.AssetsClass;
using BaseCommon.Models;

namespace BusinessLogic.BasicData.Repositorys
{
    public class AssetsClassRepository : MasterRepository
    {

        public AssetsClassRepository()
        {
            DefaulteGridSortField = "assetsClassNo";
            MasterTable = "AssetsClass";
        }



        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            ListModel model = JsonHelper.Deserialize<ListModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.AssetsClassNo) != "")
            {
                wcd.Sql += @" and AssetsClass.assetsClassNo like '%'+@assetsClassNo+'%'";
                wcd.DBPara.Add("assetsClassNo", model.AssetsClassNo);
            }
            if (DataConvert.ToString(model.AssetsClassName) != "")
            {
                wcd.Sql += @" and AssetsClass.assetsClassName like '%'+@assetsClassName+'%'";
                wcd.DBPara.Add("assetsClassName", model.AssetsClassName);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            string subViewSql = string.Format(@"select AssetsClass.assetsClassId assetsClassId,
                                 AssetsClass.assetsClassNo assetsClassNo,
                                 AssetsClass.assetsClassName assetsClassName,
                                 T.assetsClassName parentId,
                                AssetsClass.remainRate remainRate,
                                AssetsClass.durableYears durableYears,
                                (select CodeTable.codeName from CodeTable where AssetsClass.depreciationType=CodeTable.codeNo and CodeTable.codeType='DepreciationType' and CodeTable.languageVer='{0}' ) depreciationType,
                                (select unitName from Unit where AssetsClass.unitId=Unit.unitId) unitId,
                                 U1.userName createId ,
                                 AssetsClass.createTime createTime ,
                                 U2.userName updateId ,
                                 AssetsClass.updateTime updateTime ,
                                 AssetsClass.updatePro updatePro
                          from AssetsClass left join AppUser U1 on AssetsClass.createId=U1.userId
                                    left join AppUser U2 on AssetsClass.updateId=U2.userId 
                                    left join AssetsClass T on AssetsClass.parentId=T.assetsClassId
                          where 1=1 ", AppMember.AppLanguage.ToString());
            return subViewSql;
        }




        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            if (formMode != "new")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("assetsClassId", primaryKey);
                string sql = @"select * from AssetsClass where assetsClassId=@assetsClassId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.AssetsClassId = primaryKey;
                model.AssetsClassNo = DataConvert.ToString(dr["assetsClassNo"]);
                model.AssetsClassName = DataConvert.ToString(dr["assetsClassName"]);
                model.DepreciationType = DataConvert.ToString(dr["depreciationType"]);
                model.DurableYears = DataConvert.ToIntNull(dr["durableYears"]);
                model.UnitId = DataConvert.ToString(dr["unitId"]);
                model.RemainRate = DataConvert.ToDoubleNull(dr["remainRate"]);
                model.ParentId = DataConvert.ToString(dr["parentId"]);
            }
        }

        public DataRow GetModel(string primaryKey)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsClassId", primaryKey);
            string sql = @"select * from AssetsClass where assetsClassId=@assetsClassId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }


        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsClass where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsClass";
            DataRow dr = dt.NewRow();
            EntryModel myModel = model as EntryModel;
            myModel.AssetsClassId = IdGenerator.GetMaxId(dt.TableName);
            SetDataRow(myModel, dr);
            dr["assetsClassId"] = myModel.AssetsClassId;
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsClassId", pkValue);
            string sql = @"select * from AssetsClass where assetsClassId=@assetsClassId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsClass";
            EntryModel myModel = model as EntryModel;
            SetDataRow(myModel, dt.Rows[0]);
            Update5Field(dt, sysUser.UserId, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsClassId", pkValue);
            string sql = @"select * from AssetsClass where assetsClassId=@assetsClassId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsClass";
            dt.Rows[0].Delete();
            return DbUpdate.Update(dt);
        }

        protected void SetDataRow(EntryModel model, DataRow dr)
        {
            dr["assetsClassNo"] = model.AssetsClassNo;
            dr["assetsClassName"] = model.AssetsClassName;
            dr["parentId"] = DataConvert.ToString(model.ParentId) == "" ? "0" : DataConvert.ToString(model.ParentId);
            dr["durableYears"] = DataConvert.ToDBObject(model.DurableYears);
            dr["remainRate"] = DataConvert.ToDBObject(model.RemainRate);
            dr["unitId"] = model.UnitId;
            dr["depreciationType"] = model.DepreciationType;
            if (DataConvert.ToString(model.ParentId) == "")
            {
                dr["assetsClassPath"] = model.AssetsClassId;
            }
            else
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("assetsClassId", model.ParentId);
                string sql = @"select assetsClassPath from AssetsClass where assetsClassId=@assetsClassId";
                DataTable dtp = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                dr["assetsClassPath"] =DataConvert.ToString(dtp.Rows[0]["assetsClassPath"])+","+model.AssetsClassId;
            }

        }

        public DataTable GetAssetsClassTree()
        {
            string sql = @"select assetsClassId,parentId,assetsClassName,0 isOpen ,'false' checked from AssetsClass ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dt;
        }

        public DataTable GetDropListSource(string userId, string currentId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("userId", userId);
            paras.Add("assetsClassId", DataConvert.ToString(currentId));
            string sql = @"select AssetsClass.* from AssetsClass,AppCache 
                  where AppCache.tableName='AssetsClass' 
                  and AppCache.userId=@userId 
                  and AppCache.pkValue=AssetsClass.assetsClassId  
                  union 
                  select AssetsClass.* from AssetsClass
                  where AssetsClass.assetsClassId=@assetsClassId
                  order by AssetsClass.assetsClassName ";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }


        public override List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Select(filterExpression))
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["assetsClassId"]);
                dropList.Text = DataConvert.ToString(dr["assetsClassName"]);
                list.Add(dropList);
            }
            return list;
        }


    }
}

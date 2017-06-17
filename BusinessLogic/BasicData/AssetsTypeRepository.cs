using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using log4net;

namespace BusinessLogic.BasicData
{
    public class AssetsTypeRepository : MasterRepository
    {

        public AssetsTypeRepository()
        {
            DefaulteGridSortField = "assetsTypeNo";
        }

        

        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("assetsTypeNo") && DataConvert.ToString(paras["assetsTypeNo"]) != "")
                whereSql += @" and AssetsType.assetsTypeNo like '%'+@assetsTypeNo+'%'";
            if (paras.ContainsKey("assetsTypeName") && DataConvert.ToString(paras["assetsTypeName"]) != "")
                whereSql += @" and AssetsType.assetsTypeName like '%'+@assetsTypeName+'%'";
            return whereSql;
        }

        protected override string SubSelectSql(Dictionary<string, object> paras)
        {
            int pageIndex = DataConvert.ToInt32(paras["pageIndex"]);
            int rowNum = DataConvert.ToInt32(paras["rowNum"]);
            int rowSize = pageIndex * rowNum; //子查询返回行数的尺寸
            string subViewSql = string.Format(@"select top {0} AssetsType.assetsTypeId assetsTypeId,
                                 AssetsType.assetsTypeNo assetsTypeNo,
                                 AssetsType.assetsTypeName assetsTypeName,
                                 U1.userName createId ,
                                 AssetsType.createTime createTime ,
                                 U2.userName updateId ,
                                 AssetsType.updateTime updateTime ,
                                 AssetsType.updatePro updatePro
                          from AssetsType left join AppUser U1 on AssetsType.createId=U1.userId
                                    left join AppUser U2 on AssetsType.updateId=U2.userId 
                          where 1=1 {1}", DataConvert.ToString(rowSize),WhereSql(paras));
            return subViewSql;
        }

        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from AssetsType  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetAssetsType(string assetsTypeId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsTypeId", assetsTypeId);
            string sql = @"select * from AssetsType where assetsTypeId=@assetsTypeId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        
        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from AssetsType where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "AssetsType";
            DataRow dr = dt.NewRow();
            foreach (KeyValuePair<string, object> kv in objs)
            {
                dr[kv.Key] = kv.Value;
            }
            dr["assetsTypeId"] = IdGenerator.GetMaxId(dt.TableName);
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);          
            return dbUpdate.Update(dt);
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsTypeId", pkValue);
            string sql = @"select * from AssetsType where assetsTypeId=@assetsTypeId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsType";
            foreach (KeyValuePair<string, object> kv in objs)
            {
                dt.Rows[0][kv.Key] = kv.Value;
            }
            Update5Field(dt, sysUser.UserId, viewTitle);
            return dbUpdate.Update(dt);
        }

        protected override int DeleteData(Dictionary<string, object> objs,UserInfo sysUser, string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsTypeId", pkValue);
            string sql = @"select * from AssetsType where assetsTypeId=@assetsTypeId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "AssetsType";
            dt.Rows[0].Delete();
            return dbUpdate.Update(dt);
        }

        public List<DropListSource> DropList()
        {
            string sql = @"select assetsTypeId,assetsTypeName from AssetsType  order by assetsTypeName ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Rows)
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

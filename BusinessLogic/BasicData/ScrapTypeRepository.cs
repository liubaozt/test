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
    public class ScrapTypeRepository : MasterRepository
    {

        public ScrapTypeRepository()
        {
            DefaulteGridSortField = "scrapTypeNo";
        }

        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("scrapTypeNo") && DataConvert.ToString(paras["scrapTypeNo"]) != "")
                whereSql += @" and ScrapType.scrapTypeNo like '%'+@scrapTypeNo+'%'";
            if (paras.ContainsKey("scrapTypeName") && DataConvert.ToString(paras["scrapTypeName"]) != "")
                whereSql += @" and ScrapType.scrapTypeName like '%'+@scrapTypeName+'%'";
            return whereSql;
        }

        protected override string SubSelectSql(Dictionary<string, object> paras)
        {
            int pageIndex = DataConvert.ToInt32(paras["pageIndex"]);
            int rowNum = DataConvert.ToInt32(paras["rowNum"]);
            int rowSize = pageIndex * rowNum; //子查询返回行数的尺寸
            string subViewSql = string.Format(@"select top {0} ScrapType.scrapTypeId scrapTypeId,
                                 ScrapType.scrapTypeNo scrapTypeNo,
                                 ScrapType.scrapTypeName scrapTypeName,
                                 U1.userName createId ,
                                 ScrapType.createTime createTime ,
                                 U2.userName updateId ,
                                 ScrapType.updateTime updateTime ,
                                 ScrapType.updatePro updatePro
                          from ScrapType left join AppUser U1 on ScrapType.createId=U1.userId
                                    left join AppUser U2 on ScrapType.updateId=U2.userId 
                          where 1=1 {1}", DataConvert.ToString(rowSize),WhereSql(paras));
            return subViewSql;
        }

        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from ScrapType  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetScrapType(string scrapTypeId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("scrapTypeId", scrapTypeId);
            string sql = @"select * from ScrapType where scrapTypeId=@scrapTypeId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        
        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from ScrapType where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "ScrapType";
            DataRow dr = dt.NewRow();
            foreach (KeyValuePair<string, object> kv in objs)
            {
                dr[kv.Key] = kv.Value;
            }
            dr["scrapTypeId"] = IdGenerator.GetMaxId(dt.TableName);
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);          
            return dbUpdate.Update(dt);
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle,string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("scrapTypeId", pkValue);
            string sql = @"select * from ScrapType where scrapTypeId=@scrapTypeId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "ScrapType";
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
            paras.Add("scrapTypeId", pkValue);
            string sql = @"select * from ScrapType where scrapTypeId=@scrapTypeId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "ScrapType";
            dt.Rows[0].Delete();
            return dbUpdate.Update(dt);
        }

        public List<DropListSource> DropList()
        {
            string sql = @"select scrapTypeId,scrapTypeName from ScrapType  order by scrapTypeName ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Rows)
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

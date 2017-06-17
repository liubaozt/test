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
    public class UnitRepository : MasterRepository
    {

        public UnitRepository()
        {
            DefaulteGridSortField = "unitNo";
        }

        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("unitNo") && DataConvert.ToString(paras["unitNo"]) != "")
                whereSql += @" and Unit.unitNo like '%'+@unitNo+'%'";
            if (paras.ContainsKey("unitName") && DataConvert.ToString(paras["unitName"]) != "")
                whereSql += @" and Unit.unitName like '%'+@unitName+'%'";
            return whereSql;
        }

        protected override string SubSelectSql(Dictionary<string, object> paras)
        {
            int pageIndex = DataConvert.ToInt32(paras["pageIndex"]);
            int rowNum = DataConvert.ToInt32(paras["rowNum"]);
            int rowSize = pageIndex * rowNum; //子查询返回行数的尺寸
            string subViewSql = string.Format(@"select top {0} Unit.unitId unitId,
                                 Unit.unitNo unitNo,
                                 Unit.unitName unitName,
                                 CodeTable.codeName unitType,
                                 U1.userName createId ,
                                 Unit.createTime createTime ,
                                 U2.userName updateId ,
                                 Unit.updateTime updateTime ,
                                 Unit.updatePro updatePro
                          from Unit left join AppUser U1 on Unit.createId=U1.userId
                                    left join AppUser U2 on Unit.updateId=U2.userId
                                    left join CodeTable on (CodeTable.codeNo=Unit.unitType and CodeTable.codeType='{1}' and languageVer='{2}')
                          where 1=1 {3}", DataConvert.ToString(rowSize),"UnitType",AppMember.AppLanguage.ToString(), WhereSql(paras));
            return subViewSql;
        }

        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from Unit  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetUnit(string unitId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("unitId", unitId);
            string sql = @"select * from Unit where unitId=@unitId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        
        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from Unit where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "Unit";
            DataRow dr = dt.NewRow();
            foreach (KeyValuePair<string, object> kv in objs)
            {
                dr[kv.Key] = kv.Value;
            }
            dr["unitId"] = IdGenerator.GetMaxId(dt.TableName);
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);          
            return dbUpdate.Update(dt);
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("unitId", pkValue);
            string sql = @"select * from Unit where unitId=@unitId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "Unit";
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
            paras.Add("unitId", pkValue);
            string sql = @"select * from Unit where unitId=@unitId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "Unit";
            dt.Rows[0].Delete();
            return dbUpdate.Update(dt);
        }

        public List<DropListSource> DropList()
        {
            string sql = @"select unitId,unitName,unitType from Unit  order by unitName ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Rows)
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["unitId"]);
                dropList.Text = DataConvert.ToString(dr["unitName"]);
                dropList.Filter = DataConvert.ToString(dr["unitType"]);
                list.Add(dropList);
            }
            return list;
        }

    }
}

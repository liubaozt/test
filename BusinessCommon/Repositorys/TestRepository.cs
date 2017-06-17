using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Data;
using BaseCommon.Basic;
using System.Data.Common;
using BaseCommon.Repositorys;

namespace BusinessCommon.Repositorys
{
    public class Test
    {
        public string TestId { get; set; }
        public string UserId { get; set; }
        public string UserNo { get; set; }
        public string Remark { get; set; }
        public string CreateTime { get; set; }
    }

    public class TestRepository : BaseRepository
    {
        public TestRepository()
        {

        }

        public TestRepository(DataUpdate _dbUpdate)
        {
            DbUpdate = _dbUpdate;
        }

        public DataTable GetGridDataTable(Dictionary<string, object> paras)
        {
            string sql = string.Format(@" select testId,userId,userId userNo,remark,createTime from test ");
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

        public int Save(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            List<Test> gridData = (List<Test>)objs["gridData"];
            foreach (Test test in gridData)
            {
                if (DataConvert.ToString(test.TestId) == "")
                {
                    AddData(test, sysUser.UserId, viewTitle);
                }
                else
                {
                    EditData(test, sysUser.UserId, viewTitle);
                }
            }
            string[] pks = ((string)objs["deletePks"]).Split(',');
            if (pks.Length > 0)
            {
                for (int i = 0; i < pks.Length; i++)
                {
                    if (DataConvert.ToString(pks[i]) != "")
                    {
                        DeleteData(pks[i]);
                    }
                }
            }
            return 1;

        }

        protected int AddData(Test test, string sysUser, string viewTitle)
        {
            string sql = @"select * from Test where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "Test";
            DataRow dr = dt.NewRow();
            dr["testId"] = IdGenerator.GetMaxId(dt.TableName);
            dr["userId"] = test.UserId;
            dr["remark"] = test.Remark;
            dr["createTime"] = test.CreateTime;
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected int EditData(Test test, string sysUser, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("testId", test.TestId);
            string sql = @"select * from Test where testId=@testId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "Test";
            dt.Rows[0]["userId"] = test.UserId;
            dt.Rows[0]["remark"] = test.Remark;
            dt.Rows[0]["createTime"] = test.CreateTime;
            Update5Field(dt, sysUser, viewTitle);
            return DbUpdate.Update(dt);
        }

        protected int DeleteData(string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("testId", pkValue);
            string sql = @"select * from Test where testId=@testId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "Test";
            dt.Rows[0].Delete();
            return DbUpdate.Update(dt);
        }

    }
}

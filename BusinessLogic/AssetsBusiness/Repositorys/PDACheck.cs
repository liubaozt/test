using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;

namespace BusinessLogic.AssetsBusiness.Repositorys
{
    public class PDACheck
    {
        #region 下载

        protected DataSet DownLoadAssetsCheckDetail(string assetsCheckId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsCheckId", assetsCheckId);
            string sql = @" select AssetsCheckDetail.assetsCheckId,AssetsCheck.assetsCheckNo,AssetsCheck.assetsCheckName, 
                         Assets.assetsId, Assets.assetsNo,Assets.assetsName ,Assets.spec remark ,
                        Assets.assetsBarcode,Assets.panDianHao,
                         AssetsCheckDetail.storeSiteId ,AssetsCheckDetail.checkDate,
                        isnull((select userName from AppUser where Assets.usePeople=AppUser.userId),Assets.usePeople) assetsUser
                         from AssetsCheck,AssetsCheckDetail,Assets
                         where AssetsCheck.assetsCheckId=AssetsCheckDetail.assetsCheckId
                         and Assets.assetsId=AssetsCheckDetail.assetsId 
                         and AssetsCheckDetail.assetsCheckId=@assetsCheckId";
            return AppMember.DbHelper.GetDataSet(sql, paras);
        }

        protected DataSet DownLoadStoreSite()
        {
            string sql = @" select storeSiteId,storeSiteNo,storeSiteName
                         from StoreSite";
            return AppMember.DbHelper.GetDataSet(sql);
        }

        protected int AddAssetsCheckDetail(DataTable dt)
        {
            SqliteHelper sqliteDb = new SqliteHelper();
            string sql;
            try
            {
                sqliteDb.Open();
                sqliteDb.BeginTransaction();
                sqliteDb.ExecuteNonQuery(@"DELETE FROM AssetsCheckDetail");
                int ret = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    sql = string.Format(@"insert into AssetsCheckDetail(assetsCheckId,assetsCheckNo,assetsCheckName,
                                               assetsId,assetsNo,assetsName,remark,storeSiteId,checkDate,assetsUser)
                                               values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                                                     DataConvert.ToString(dr["assetsCheckId"]),
                                                     DataConvert.ToString(dr["assetsCheckNo"]),
                                                     DataConvert.ToString(dr["assetsCheckName"]),
                                                     DataConvert.ToString(dr["assetsId"]),
                                                     DataConvert.ToString(dr["assetsNo"]),
                                                     SafeSql.SafeSqlStr(DataConvert.ToString(dr["assetsName"])),
                                                     SafeSql.SafeSqlStr(DataConvert.ToString(dr["remark"])),
                                                     DataConvert.ToString(dr["storeSiteId"]),
                                                     DataConvert.ToDateTime(dr["checkDate"]).ToString("yyyy-MM-dd"),
                                                     DataConvert.ToString(dr["assetsUser"])
                                                     );
                    ret += sqliteDb.ExecuteNonQuery(sql);
                }
                sqliteDb.Commit();
                return ret;
            }
            catch (Exception ex)
            {
                sqliteDb.RollBack();
                throw ex;
            }
            finally
            {
                sqliteDb.Close();
            }
        }

        protected int AddStoreSite(DataTable dt)
        {
            SqliteHelper sqliteDb = new SqliteHelper();
            try
            {
                sqliteDb.Open();
                sqliteDb.BeginTransaction();
                sqliteDb.ExecuteNonQuery(@"DELETE FROM StoreSite");
                int ret = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    string sql = string.Format(@"insert into StoreSite(storeSiteId,storeSiteNo,storeSiteName)
                                               values ('{0}','{1}','{2}')",
                                                      DataConvert.ToString(dr["storeSiteId"]),
                                                      DataConvert.ToString(dr["storeSiteNo"]),
                                                      DataConvert.ToString(dr["storeSiteName"])
                                                      );
                    ret += sqliteDb.ExecuteNonQuery(sql);
                }
                sqliteDb.Commit();
                return ret;
            }
            catch (Exception ex)
            {
                sqliteDb.RollBack();
                throw ex;
            }
            finally
            {
                sqliteDb.Close();
            }
        }

        public int DownLoad(string primaryKey)
        {
            DataTable dt = DownLoadAssetsCheckDetail(primaryKey).Tables[0];
            DataTable dt2 = DownLoadStoreSite().Tables[0];
            AddAssetsCheckDetail(dt);
            AddStoreSite(dt2);
            return 1;
        }

        #endregion

        #region 上传

        protected DataSet GetAssetsCheckDetail(string DBName)
        {
            SqliteHelper sqliteDb = new SqliteHelper(DBName);
            sqliteDb.Open();
            string sql = @"select AssetsCheckDetail.assetsCheckId,AssetsCheckDetail.assetsCheckNo,AssetsCheckDetail.assetsCheckName,
                                  AssetsCheckDetail.assetsId,AssetsCheckDetail.assetsNo,AssetsCheckDetail.assetsName,AssetsCheckDetail.remark,
                                    AssetsCheckDetail.storeSiteId,
                                    AssetsCheckDetail.actualStoreSiteId,
                                    A.storeSiteNo storeSiteNo,
                                    B.storeSiteNo actualStoreSiteNo,
                                    AssetsCheckDetail.checkDate ,
                                    AssetsCheckDetail.actualCheckDate ,
                                    A.storeSiteName storeSiteName,
                                    B.storeSiteName actualStoreSiteName
                                    from AssetsCheckDetail left join StoreSite A on AssetsCheckDetail.storeSiteId=A.storeSiteId 
                                    left join StoreSite B on AssetsCheckDetail.actualStoreSiteId=B.storeSiteId ";
            return sqliteDb.FillDataSet(sql);
        }


        protected int UpdateAssetsCheckDetail(string assetsCheckId, string assetsId, string actualStoreSiteId, string actualCheckDate)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("actualStoreSiteId", actualStoreSiteId);
            paras.Add("assetsCheckId", assetsCheckId);
            paras.Add("assetsId", assetsId);
            paras.Add("actualCheckDate", actualCheckDate);
            string sql = @" update AssetsCheckDetail set actualStoreSiteId=@actualStoreSiteId,actualCheckDate=@actualCheckDate ,checkResult='O',isFinished='Y' 
                        where assetsCheckId=@assetsCheckId and assetsId=@assetsId";
            return AppMember.DbHelper.ExecuteSql(sql, paras);
        }

        protected int UpdateChecking(string assetsId, string actualStoreSiteId = "")
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("assetsId", assetsId);
            string sql = string.Format(@"update Assets set checking='N' {0} where assetsId=@assetsId", DataConvert.ToString(actualStoreSiteId) != "" ? (",storeSiteId='" + actualStoreSiteId + "'") : "");
            return AppMember.DbHelper.ExecuteSql(sql, paras);
        }

        public int UpLoad(string DBName)
        {
            DataTable dt = GetAssetsCheckDetail(DBName).Tables[0];
            dt.TableName = "AssetsCheckDetail";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (DataConvert.ToString(dr["actualStoreSiteId"]) != "")
                    {
                        UpdateAssetsCheckDetail(DataConvert.ToString(dr["assetsCheckId"]), DataConvert.ToString(dr["assetsId"]),
                                DataConvert.ToString(dr["actualStoreSiteId"]), DataConvert.ToString(dr["actualCheckDate"]));
                        UpdateChecking(DataConvert.ToString(dr["assetsId"]), DataConvert.ToString(dr["actualStoreSiteId"]));
                    }
                }
            }
            return 1;
        }
        #endregion
    }
}

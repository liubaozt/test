using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using log4net;
using System.Text.RegularExpressions;
using System.Data.Common;

namespace BusinessCommon.AppMng
{
    public class BarcodeStyleRepository : MasterRepository
    {

        public BarcodeStyleRepository()
        {
            DefaulteGridSortField = "BarcodeStyleName";
        }



        protected override string WhereSql(Dictionary<string, object> paras)
        {
            string whereSql = "";
            if (paras.ContainsKey("barcodeStyleName") && DataConvert.ToString(paras["barcodeStyleName"]) != "")
                whereSql += @" and BarcodeStyle.barcodeStyleName like '%'+@barcodeStyleName+'%'";
            return whereSql;
        }

        protected override string SubSelectSql(Dictionary<string, object> paras)
        {
            int pageIndex = DataConvert.ToInt32(paras["pageIndex"]);
            int rowNum = DataConvert.ToInt32(paras["rowNum"]);
            int rowSize = pageIndex * rowNum; //子查询返回行数的尺寸
            string subViewSql = string.Format(@"select top {0} BarcodeStyle.barcodeStyleId barcodeStyleId,
                                 BarcodeStyle.barcodeStyleName barcodeStyleName,
                                 BarcodeStyle.remark remark,
                                 U1.userNo createId ,
                                 BarcodeStyle.createTime createTime ,
                                 U2.userNo updateId ,
                                 BarcodeStyle.updateTime updateTime ,
                                 BarcodeStyle.updatePro updatePro
                          from BarcodeStyle left join AppUser U1 on BarcodeStyle.createId=U1.userId
                                       left join AppUser U2 on BarcodeStyle.updateId=U2.userId
                          where 1=1 {2}", DataConvert.ToString(rowSize), AppMember.AppLanguage.ToString(), WhereSql(paras));
            return subViewSql;
        }

        public override int GetGridDataCount(Dictionary<string, object> paras)
        {
            string sql = @"select count(*) cnt
                          from BarcodeStyle  where 1=1 " + WhereSql(paras);
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return DataConvert.ToInt32(dtGrid.Rows[0]["cnt"]);
        }


        public DataRow GetBarcodeStyle(string barcodeStyleId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("barcodeStyleId", barcodeStyleId);
            string sql = @"select * from BarcodeStyle where barcodeStyleId=@barcodeStyleId";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid.Rows[0];
        }

        public string GetBarcodeDefaultStyle()
        {
            string sql = @"select * from BarcodeStyle where isDefaultStyle='Y'";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return DataConvert.ToString(dtGrid.Rows[0]["barcodeStyleId"]);
        }

        protected override int AddData(Dictionary<string, object> objs, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from BarcodeStyle where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "BarcodeStyle";
            DataRow dr = dt.NewRow();
            foreach (KeyValuePair<string, object> kv in objs)
            {
                if (kv.Key != "sync")
                    dr[kv.Key] = kv.Value;
            }
            string pk = IdGenerator.GetMaxId(dt.TableName);
            dr["barcodeStyleId"] = pk;
            string barcodeStyleJson = objs["stylejson"].ToString();
            string sync = objs["sync"].ToString();
            SaveDetail(ref barcodeStyleJson, pk, sync);
            dr["stylejson"] = barcodeStyleJson;
            Create5Field(dr, sysUser.UserId, viewTitle);
            dt.Rows.Add(dr);
            dbUpdate.Update(dt);
            return 1;
        }

        protected override int EditData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("barcodeStyleId", pkValue);
            string sql = @"select * from BarcodeStyle where barcodeStyleId=@barcodeStyleId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "BarcodeStyle";
            foreach (KeyValuePair<string, object> kv in objs)
            {
                if (kv.Key != "sync")
                    dt.Rows[0][kv.Key] = kv.Value;
            }
            string barcodeStyleJson = objs["stylejson"].ToString();
            string sync = objs["sync"].ToString();
            SaveDetail(ref barcodeStyleJson, pkValue, sync);
            dt.Rows[0]["stylejson"] = barcodeStyleJson;
            Update5Field(dt, sysUser.UserId, viewTitle);
            dbUpdate.Update(dt);
            return 1;
        }

        protected override int DeleteData(Dictionary<string, object> objs, UserInfo sysUser, string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("barcodeStyleId", pkValue);
            string sql = @"select * from BarcodeStyle where barcodeStyleId=@barcodeStyleId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "BarcodeStyle";
            dt.Rows[0].Delete();
            string barcodeStyleJson = objs["stylejson"].ToString();
            DeleteDetail(pkValue);
            dbUpdate.Update(dt);
            return 1;
        }

        protected int SaveDetail(ref string barcodeStyleJson, string pkValue, string sync)
        {
            DeleteDetail(pkValue);
            string[] sArray = Regex.Split(barcodeStyleJson, "}}}},", RegexOptions.IgnoreCase);
            string[] statesArray = Regex.Split(sArray[0], "}}},", RegexOptions.IgnoreCase);

            foreach (string states in statesArray)
            {
                string[] states_propsArray = Regex.Split(states, "props:{", RegexOptions.IgnoreCase);
                string[] states1Array = Regex.Split(states_propsArray[0], ",text:{text:", RegexOptions.IgnoreCase);
                string[] states11Array = Regex.Split(states1Array[0], ":{type:'", RegexOptions.IgnoreCase);
                string nodeId = states11Array[0].Replace("{states:{", "");
                string nodeType = states11Array[1].Replace("'", "");
                string x = "", y = "", width = "", height = "";
                string[] states12Array = Regex.Split(states1Array[1], "attr:{", RegexOptions.IgnoreCase);
                string[] states121Array = Regex.Split(states12Array[1], ", width:", RegexOptions.IgnoreCase);
                string[] states1211Array = Regex.Split(states121Array[0], ", y:", RegexOptions.IgnoreCase);
                string x1 = DataConvert.ToString(states1211Array[0].Replace("x:", ""));
                string y1 = states1211Array[1];
                string[] states1212Array = Regex.Split(states121Array[1], ", height:", RegexOptions.IgnoreCase);
                string width1 = DataConvert.ToString(states1212Array[0]);
                string height1 = DataConvert.ToString(states1212Array[1].Replace("},", ""));


                string[] states2Array = Regex.Split(states_propsArray[1], "'},refField:{value:'", RegexOptions.IgnoreCase);
                string[] states21Array = Regex.Split(states2Array[0], "'},barcodeType:{value:'", RegexOptions.IgnoreCase);
                string nodeText = "";
                string barcodeType = "";
                string refField = "";
                string[] states22Array;
                if (nodeType != "rectangle")
                {
                    nodeText = states21Array[0].Replace("text:{value:'", "");
                    nodeText = nodeText.Replace("'", "");
                    if (states21Array.Length > 1)
                        barcodeType = states21Array[1].Replace("'", "");
                    states22Array = Regex.Split(states2Array[1], "'},x:{value:'", RegexOptions.IgnoreCase);
                    if (states22Array.Length > 1)
                        refField = states22Array[0].Replace("'", "");
                }
                else
                {
                    states22Array = Regex.Split(states_propsArray[1], "'},x:{value:'", RegexOptions.IgnoreCase);
                }

                string[] states221Array = Regex.Split(states22Array[1], "'},y:{value:'", RegexOptions.IgnoreCase);
                string x2 = states221Array[0];
                string[] states2211Array = Regex.Split(states221Array[1], "'},width:{value:'", RegexOptions.IgnoreCase);
                string y2 = states2211Array[0];
                string[] states22111Array = Regex.Split(states2211Array[1], "'},heigth:{value:'", RegexOptions.IgnoreCase);
                string width2 = states22111Array[0];
                string height2 = states22111Array[1].Replace("'", "");


                if (sync == "sync")
                {
                    x = x1;
                    y = y1;
                    width = width1;
                    height = height1;
                    string[] jsonpro = Regex.Split(states, "x:{value:", RegexOptions.IgnoreCase);
                    string jsonproreplace = jsonpro[1];
                    string xywhpro = "'" + x + "'},y:{value:'" + y + "'},width:{value:'" + width + "'},heigth:{value:'" + height + "'";
                    string statesold = states;
                    string statesrep = states.Replace(jsonproreplace, xywhpro);
                    barcodeStyleJson = barcodeStyleJson.Replace(statesold, statesrep);
                }
                else if (sync == "resync")
                {
                    x = DataConvert.ToString(x2) == "" ? x1 : x2;
                    y = DataConvert.ToString(y2) == "" ? y1 : y2;
                    width = DataConvert.ToString(width2) == "" ? width1 : width2;
                    height = DataConvert.ToString(height2) == "" ? height1 : height2;
                    string[] jsonstr = Regex.Split(states, "}, props:{", RegexOptions.IgnoreCase);
                    string[] jsonstr1 = Regex.Split(jsonstr[0], "attr:{", RegexOptions.IgnoreCase);
                    string jsonreplace = jsonstr1[1];
                    string xywh = String.Format("x:{0}, y:{1}, width:{2}, height:{3}", x, y, width, height);
                    string[] jsonpro = Regex.Split(states, "x:{value:", RegexOptions.IgnoreCase);
                    string jsonproreplace = jsonpro[1];
                    string xywhpro = "'" + x + "'},y:{value:'" + y + "'},width:{value:'" + width + "'},heigth:{value:'" + height + "'";
                    string statesold = states;
                    string statesrep = states.Replace(jsonreplace, xywh);
                    statesrep = statesrep.Replace(jsonproreplace, xywhpro);
                    barcodeStyleJson = barcodeStyleJson.Replace(statesold, statesrep);
                }

                AddDetail(pkValue, nodeId, nodeType, nodeText, refField, barcodeType, DataConvert.ToInt32(x), DataConvert.ToInt32(y), DataConvert.ToInt32(width), DataConvert.ToInt32(height));
            }
            return 1;
        }

        protected int DeleteDetail(string pkValue)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("barcodeStyleId", pkValue);
            string sql = @"select * from BarcodeStyleDetail where barcodeStyleId=@barcodeStyleId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "BarcodeStyleDetail";
            foreach (DataRow dr in dt.Rows)
            {
                dr.Delete();
            }
            dbUpdate.Update(dt);
            return 1;
        }



        protected int AddDetail(string pkValue, string nodeId, string nodeType, string nodeText, string refField, string barcodeType, int x, int y, int width, int height)
        {
            string sql = @"select * from BarcodeStyleDetail where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "BarcodeStyleDetail";
            DataRow dr = dt.NewRow();
            dr["barcodeStyleId"] = pkValue;
            dr["nodeId"] = nodeId;
            dr["nodeType"] = nodeType;
            dr["nodeText"] = nodeText;
            dr["refField"] = refField;
            dr["barcodeType"] = barcodeType;
            dr["x"] = x;
            dr["y"] = y;
            dr["width"] = width;
            dr["height"] = height;
            dt.Rows.Add(dr);
            return dbUpdate.Update(dt);
        }

        public List<DropListSource> DropList()
        {
            string sql = @"select barcodeStyleId,barcodeStyleName from BarcodeStyle  order by isDefaultStyle, barcodeStyleName ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Rows)
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["barcodeStyleId"]);
                dropList.Text = DataConvert.ToString(dr["barcodeStyleName"]);
                list.Add(dropList);
            }
            return list;
        }


    }
}

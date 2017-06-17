using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Data.Common;
using BaseCommon.Repositorys;
using BusinessCommon.Models.BarcodeStyle;
using BaseCommon.Models;

namespace BusinessCommon.Repositorys
{
    public class BarcodeStyleRepository : MasterRepository
    {

        public BarcodeStyleRepository()
        {
            DefaulteGridSortField = "BarcodeStyleName";
            MasterTable = "BarcodeStyle";
        }



        protected override WhereConditon ListWhereSql(ListCondition condition)
        {
            WhereConditon wcd = new WhereConditon();
            wcd.DBPara = new Dictionary<string, object>();
            ListModel model = JsonHelper.Deserialize<ListModel>(condition.ListModelString);
            if (model == null) return wcd;
            if (DataConvert.ToString(model.BarcodeStyleName) != "")
            {
                wcd.Sql += @" and BarcodeStyle.barcodeStyleName like '%'+@barcodeStyleName+'%'";
                wcd.DBPara.Add("barcodeStyleName", model.BarcodeStyleName);
            }
            return wcd;
        }

        protected override string ListSql(ListCondition condition)
        {
            int rowSize = condition.PageIndex * condition.PageRowNum;  //子查询返回行数的尺寸
            string subViewSql = string.Format(@"select  BarcodeStyle.barcodeStyleId barcodeStyleId,
                                 BarcodeStyle.barcodeStyleName barcodeStyleName,
                                 BarcodeStyle.remark remark,
                                 U1.userNo createId ,
                                 BarcodeStyle.createTime createTime ,
                                 U2.userNo updateId ,
                                 BarcodeStyle.updateTime updateTime ,
                                 BarcodeStyle.updatePro updatePro
                          from BarcodeStyle left join AppUser U1 on BarcodeStyle.createId=U1.userId
                                       left join AppUser U2 on BarcodeStyle.updateId=U2.userId
                          where 1=1 ");
            return subViewSql;
        }



        public void SetModel(string primaryKey, string formMode, EntryModel model)
        {
            if (formMode != "new")
            {
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras.Add("barcodeStyleId", primaryKey);
                string sql = @"select * from BarcodeStyle where barcodeStyleId=@barcodeStyleId";
                DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
                DataRow dr = dtGrid.Rows[0];
                model.BarcodeStyleId = primaryKey;
                model.BarcodeStyleName = DataConvert.ToString(dr["barcodeStyleName"]);
                model.Remark = DataConvert.ToString(dr["remark"]);
                model.StyleJson = DataConvert.ToString(dr["stylejson"]);
                model.IsDefaultStyle = DataConvert.ToString(dr["isDefaultStyle"]) == "Y" ? true : false;
            }
            else
            {
                model.StyleJson = "1";
            }
        }

        public string GetBarcodeDefaultStyle()
        {
            string sql = @"select * from BarcodeStyle where isDefaultStyle='Y'";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            if (dtGrid.Rows.Count > 0)
                return DataConvert.ToString(dtGrid.Rows[0]["barcodeStyleId"]);
            else
                return "";
        }

        public DataTable GetBarcodeStyle(string barcodeStyleId)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("barcodeStyleId", barcodeStyleId);
            string sql = string.Format(@"select BarcodeStyleDetail.* from BarcodeStyle,BarcodeStyleDetail 
                        where BarcodeStyle.barcodeStyleId=BarcodeStyleDetail.barcodeStyleId 
                        and BarcodeStyle.barcodeStyleId=@barcodeStyleId ");
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            return dtGrid;
        }

        protected override int Add(EntryViewModel model, UserInfo sysUser, string viewTitle)
        {
            string sql = @"select * from BarcodeStyle where 1<>1 ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            dt.TableName = "BarcodeStyle";
            DataRow dr = dt.NewRow();
            EntryModel myModel = model as EntryModel;
            dr["barcodeStyleName"] = myModel.BarcodeStyleName;
            dr["remark"] = myModel.Remark;
            dr["isDefaultStyle"] = myModel.IsDefaultStyle ? "Y" : "N";
            string pk = IdGenerator.GetMaxId(dt.TableName);
            dr["barcodeStyleId"] = pk;
            string barcodeStyleJson = myModel.StyleJson.ToString();
            string sync = myModel.Sync.ToString();
            SaveDetail(ref barcodeStyleJson, pk, sync);
            dr["stylejson"] = barcodeStyleJson;
            dt.Rows.Add(dr);
            Create5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            return 1;
        }

        protected override int Modified(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle, string formMode)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("barcodeStyleId", pkValue);
            string sql = @"select * from BarcodeStyle where barcodeStyleId=@barcodeStyleId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "BarcodeStyle";
            EntryModel myModel = model as EntryModel;
            dt.Rows[0]["barcodeStyleName"] = myModel.BarcodeStyleName;
            dt.Rows[0]["remark"] = myModel.Remark;
            dt.Rows[0]["isDefaultStyle"] = myModel.IsDefaultStyle ? "Y" : "N";
            string barcodeStyleJson = myModel.StyleJson.ToString();
            string sync = myModel.Sync.ToString();
            SaveDetail(ref barcodeStyleJson, pkValue, sync);
            dt.Rows[0]["stylejson"] = barcodeStyleJson;
            Update5Field(dt, sysUser.UserId, viewTitle);
            DbUpdate.Update(dt);
            return 1;
        }

        protected override int Delete(EntryViewModel model, UserInfo sysUser, string pkValue, string viewTitle)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("barcodeStyleId", pkValue);
            string sql = @"select * from BarcodeStyle where barcodeStyleId=@barcodeStyleId";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql, paras).Tables[0];
            dt.TableName = "BarcodeStyle";
            dt.Rows[0].Delete();
            DeleteDetail(pkValue);
            DbUpdate.Update(dt);
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
                string barWidth = "";
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
                    {
                        string[] states22subArray = Regex.Split(states22Array[0], "'},barWidth:{value:'", RegexOptions.IgnoreCase);
                        refField = states22subArray[0].Replace("'", "");
                        if (states22subArray.Length > 1)
                            barWidth = states22subArray[1].Replace("'", "");
                    }
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

                AddDetail(pkValue, nodeId, nodeType, nodeText, refField, barcodeType, barWidth, DataConvert.ToInt32(x), DataConvert.ToInt32(y), DataConvert.ToInt32(width), DataConvert.ToInt32(height));
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
            DbUpdate.Update(dt);
            return 1;
        }



        protected int AddDetail(string pkValue, string nodeId, string nodeType, string nodeText, string refField, string barcodeType, string barWidth, int x, int y, int width, int height)
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
            if (DataConvert.ToString(barWidth) != "")
            {
                dr["barWidth"] = DataConvert.ToInt32(barWidth);
            }
            dr["x"] = x;
            dr["y"] = y;
            dr["width"] = width;
            dr["height"] = height;
            dt.Rows.Add(dr);
            return DbUpdate.Update(dt);
        }
        public override DataTable GetDropListSource()
        {
            string sql = @"select * from BarcodeStyle  order by isDefaultStyle, barcodeStyleName";
            DataTable dtGrid = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            return dtGrid;
        }
        public override List<DropListSource> DropList(DataTable dt, string filterExpression)
        {
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Select(filterExpression))
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseCommon.Basic;
using BaseCommon.Data;

namespace BusinessCommon.Repositorys
{
    public class CodeTableRepository
    {
        public List<DropListSource> DropList()
        {
            string sql = @"select codeNo,codeName,codeType from CodeTable  order by codeName ";
            DataTable dt = AppMember.DbHelper.GetDataSet(sql).Tables[0];
            List<DropListSource> list = new List<DropListSource>();
            foreach (DataRow dr in dt.Rows)
            {
                DropListSource dropList = new DropListSource();
                dropList.Value = DataConvert.ToString(dr["codeNo"]);
                dropList.Text = DataConvert.ToString(dr["codeName"]);
                dropList.Filter = DataConvert.ToString(dr["codeType"]);
                list.Add(dropList);
            }
            return list;
        }
    }
}

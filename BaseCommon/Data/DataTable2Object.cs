using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BaseCommon.Data
{
    public class DataTable2Object
    {
        public static Object[] Data(DataTable dtGrid, Dictionary<string, GridInfo> layout)
        {
            if (dtGrid==null)
                return new object[0];
            var rows = new object[dtGrid.Rows.Count];
            for (int i = 0; i < dtGrid.Rows.Count; i++)
            {

                var fields = new object[dtGrid.Columns.Count];
                int colIndex = 0;
                foreach (KeyValuePair<string, GridInfo> dic in layout)
                {
                    fields[colIndex] = DataConvert.ToString(dtGrid.Rows[i][dic.Value.Name]);
                    colIndex += 1;
                }

                rows[i] = new
                {
                    id = i + 1,
                    cell = fields
                };
            }
            return rows;
        }

       

        public static Object[] Data(DataTable dtGrid)
        {
            var rows = new object[dtGrid.Rows.Count];
            for (int i = 0; i < dtGrid.Rows.Count; i++)
            {

                var fields = new object[dtGrid.Columns.Count];
                int colIndex = 0;
                for (int j = 0; j < dtGrid.Columns.Count; j++)
                {
                    fields[colIndex] = DataConvert.ToString(dtGrid.Rows[i][j]);
                    colIndex += 1;
                }

                rows[i] = new
                {
                    id = i + 1,
                    cell = fields
                };
            }
            return rows;
        }
    }
}

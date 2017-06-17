using System;
using System.Collections.Generic;
using System.Text;

namespace BaseCommon.Data
{
    /// <summary>
    /// Êý¾Ý×ª»»
    /// </summary>
    public class DataConvert
    {

        public static string ToString(object o)
        {
            return (IsNullOrDbNull(o) ? string.Empty : o.ToString().Trim());
        }

        public static Int32 ToInt32(object o)
        {
            return (IsNullOrDbNull(o) ? 0 : Convert.ToInt32(o));
        }

        public static int? ToIntNull(object o)
        {
            int? a = null;
            if (!IsNullOrDbNull(o))
                a = Convert.ToInt32(o);
            return a;
        }

        public static object ToDBObject(object o)
        {
            if (o == null || o == DBNull.Value || o.ToString().Trim() == "")
                return DBNull.Value;
            else
                return o;
        }


        public static Int64 ToInt64(object o)
        {
            return (IsNullOrDbNull(o) ? 0 : Convert.ToInt64(o));
        }



        public static long ToLong(object o)
        {
            return (IsNullOrDbNull(o) ? 0 : Convert.ToInt64(o));
        }

        public static decimal ToDecimal(object o)
        {
            return (IsNullOrDbNull(o) ? 0 : Convert.ToDecimal(o));
        }

        public static DateTime ToDateTime(object o)
        {
            return (IsNullOrDbNull(o) ? DateTime.MinValue : Convert.ToDateTime(o));
        }

        public static DateTime? ToDateTimeOrNull(object o)
        {
            if (IsNullOrDbNull(o))
                return null;
            else
                return Convert.ToDateTime(o);
        }

        public static double ToDouble(object o)
        {
            return (IsNullOrDbNull(o) ? 0 : Convert.ToDouble(o));
        }

        public static double? ToDoubleNull(object o)
        {
            double? a = null;
            if (!IsNullOrDbNull(o))
                a = Convert.ToDouble(o);
            return a;
        }

        public static Boolean ToBoolean(object o)
        {
            return (IsNullOrDbNull(o) ? false : Convert.ToBoolean(o));
        }

        private static bool IsNullOrDbNull(object o)
        {

            if (o is string)
            {
                return (Convert.ToString(o).Trim() == string.Empty ? true : false);
            }
            else
            {
                if (o == null || o == DBNull.Value)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}

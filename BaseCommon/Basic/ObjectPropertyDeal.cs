using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace BaseCommon.Basic
{
    public  class ObjectPropertyDeal
    {
        public static string GetObjectPropertyValue<T>(T t, string propertyname)
        {
            Type type = typeof(T);
            PropertyInfo property = type.GetProperty(propertyname);
            if (property == null) return string.Empty;
            object o = property.GetValue(t, null);
            if (o == null) return string.Empty;
            return o.ToString();
        }
    }
}

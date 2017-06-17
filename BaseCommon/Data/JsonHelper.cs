using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;

namespace BaseCommon.Data
{
    public static class JsonHelper
    {
        public static string ToJson(object obj)
        {
            //JSON序列化
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            //定义一个stream用来存放序列化之后的内容
            Stream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            //从头到尾将stream读取成一个字符串形式的数据，并且返回
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);

            return reader.ReadToEnd();

        }


        public static List<T> JSONStringToList<T>(this string JsonStr)
        {
            List<T> objs = Deserialize<List<T>>(JsonStr);
            return objs;
        }



        public static T Deserialize<T>(string json)
        {
            if (DataConvert.ToString(json) == "")
                return default(T);
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(ms);
            }
        }

    }
}

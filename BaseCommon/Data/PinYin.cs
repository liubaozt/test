using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.International.Converters.PinYinConverter;
using System.Collections.ObjectModel;

namespace BaseCommon.Data
{
    public class PinYin
    {
        /// <summary> 
        /// 汉字转化为拼音
        /// </summary> 
        /// <param name="str">汉字</param> 
        /// <returns>全拼</returns> 
        public static string GetPinyin(string str)
        {
            string returnStr = string.Empty;
            List<string> py = new List<string>();
            py.Add("");
            foreach (char obj in str)
            {
                try
                {

                    ChineseChar chineseChar = new ChineseChar(obj);
                    ReadOnlyCollection<string> pinyin = chineseChar.Pinyins;
                    List<string> dypy = new List<string>();
                    foreach (string pin in pinyin)
                    {
                        if (pin != null)
                        {
                            string r = DataConvert.ToString(pin.Substring(0, pin.Length - 1));
                            if (!dypy.Contains(r))
                            {
                                dypy.Add(r);
                            }
                        }
                    }
                    if (dypy.Count < 2)
                    {
                        if (chineseChar.Pinyins[0] != null)
                        {
                            string t = DataConvert.ToString(chineseChar.Pinyins[0].ToString());
                            string r = DataConvert.ToString(t.Substring(0, t.Length - 1));
                            for (int i = 0; i < py.Count; i++)
                            {
                                py[i] += r;
                            }
                        }
                    }
                    else
                    {
                        List<string> prePY = new List<string>();
                        foreach (string strpy in py)
                        {
                            prePY.Add(strpy);
                        }
                        int pyIndex = 0;
                        foreach (string prepy in prePY)
                        {
                            for (int i = 0; i < dypy.Count; i++)
                            {
                                if (i > 0)
                                    py.Add(prepy);
                                py[pyIndex] += dypy[i];
                                pyIndex++;
                            }
                        }
                    }

                }
                catch
                {
                    returnStr += obj.ToString();
                }
            }
            foreach (string rs in py)
            {
                returnStr += rs + ",";
            }
            returnStr = returnStr.Substring(0, returnStr.Length - 1);
            return returnStr;
        }

        /// <summary> 
        /// 汉字转化为拼音首字母
        /// </summary> 
        /// <param name="str">汉字</param> 
        /// <returns>首字母</returns> 
        //public static string GetFirstPinyin(string str)
        //{
        //    string r = string.Empty;
        //    foreach (char obj in str)
        //    {
        //        try
        //        {
        //            ChineseChar chineseChar = new ChineseChar(obj);
        //            string t = chineseChar.Pinyins[0].ToString();
        //            r += t.Substring(0, 1);
        //        }
        //        catch
        //        {
        //            r += obj.ToString();
        //        }
        //    }
        //    return r;
        //}

        public static string GetFirstPinyin(string str)
        {
            string returnStr = string.Empty;
            List<string> py = new List<string>();
            py.Add("");
            foreach (char obj in str)
            {
                try
                {

                    ChineseChar chineseChar = new ChineseChar(obj);
                    ReadOnlyCollection<string> pinyin = chineseChar.Pinyins;
                    List<string> dypy = new List<string>();
                    foreach (string pin in pinyin)
                    {
                        if (pin != null)
                        {
                            string r = DataConvert.ToString(pin.Substring(0, 1));
                            if (!dypy.Contains(r))
                            {
                                dypy.Add(r);
                            }
                        }
                    }
                    if (dypy.Count < 2)
                    {
                        if (chineseChar.Pinyins[0] != null)
                        {
                            string t = DataConvert.ToString(chineseChar.Pinyins[0].ToString());
                            string r = DataConvert.ToString(t.Substring(0, 1));
                            for (int i = 0; i < py.Count; i++)
                            {
                                py[i] += r;
                            }
                        }
                    }
                    else
                    {
                        List<string> prePY = new List<string>();
                        foreach (string strpy in py)
                        {
                            prePY.Add(strpy);
                        }
                        int pyIndex = 0;
                        foreach (string prepy in prePY)
                        {
                            for (int i = 0; i < dypy.Count; i++)
                            {
                                if (i > 0)
                                    py.Add(prepy);
                                py[pyIndex] += dypy[i];
                                pyIndex++;
                            }
                        }
                    }

                }
                catch
                {
                    returnStr += obj.ToString();
                }
            }
            foreach (string rs in py)
            {
                returnStr += rs + ",";
            }
            returnStr = returnStr.Substring(0, returnStr.Length - 1);
            return returnStr;
        }

    }
}

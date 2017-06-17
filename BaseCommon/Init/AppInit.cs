using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.Common;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.IO;
using System.Collections;

namespace BaseCommon.Init
{
    public delegate string TextFilePath();
    public class AppInit
    {
        public static void Init()
        {
            string connstrs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            //string[] connstrArray = connstrs.Split(';');
            //string pwd = connstrArray[3].Replace("pwd=", "");
            //pwd = AppSecurity.Decryption.Decrypt(pwd);
            //string connstr = connstrArray[0] + ";" + connstrArray[1] + ";" + connstrArray[2] + ";" + "pwd=" + pwd;
            AppMember.ConnectionString = connstrs;
            AppMember.Provider = DbProviderFactories.GetFactory(
                    ConfigurationManager.ConnectionStrings["ConnectionString"].ProviderName);
            AppMember.DbHelper = new DatabaseHelper();
            AppMember.AppLanguage = GetLanguage();
            AppMember.GridPath = GetGridPath(AppMember.AppLanguage);
            AppMember.AppText = GetAppText(AppMember.AppLanguage);
            AppMember.AppDBMS = GetDBMS();
            AppMember.TablePrefix = GetTablePrefix();
            AppMember.AutoMonthUpdate = ConfigurationManager.AppSettings["AutoMonthUpdate"].ToString();
            AppMember.LaunchDepreciation = ConfigurationManager.AppSettings["LaunchDepreciation"].ToString();
            AppMember.AutoDepreciation = ConfigurationManager.AppSettings["AutoDepreciation"].ToString();
            AppMember.PrinterName = ConfigurationManager.AppSettings["PrinterName"].ToString();
            AppMember.SqllitePath = AppDomain.CurrentDomain.BaseDirectory + "Content\\uploads\\sqlite\\";
            AppMember.UsePeopleControlLevel = GetUsePeopleControlLevel();
            AppMember.DepreciationRuleOpen = GetDepreciationRuleOpen();
            AppMember.ViewVersion = ConfigurationManager.AppSettings["ViewVersion"].ToString();
        }

        private static UsePeopleControlLevel GetUsePeopleControlLevel()
        {
            string level = ConfigurationManager.AppSettings["UsePeopleControlLevel"].ToString();
            if (level.ToLower() == UsePeopleControlLevel.High.ToString().ToLower())
                return UsePeopleControlLevel.High;
            else if (level.ToLower() == UsePeopleControlLevel.Low.ToString().ToLower())
                return UsePeopleControlLevel.Low;
            else
                return UsePeopleControlLevel.High;
        }

        private static bool GetDepreciationRuleOpen()
        {
            string state = ConfigurationManager.AppSettings["DepreciationRuleOpen"].ToString();
            if (state.ToLower() == "true")
                return true;
            else
                return false;
        }

        private static Dictionary<string, string> GetAppText(Language lg)
        {
            switch (lg)
            {
                case Language.CN:
                    return GetAppText(CNFilePath);
                case Language.EN:
                    return GetAppText(ENFilePath);
                default:
                    return GetAppText(CNFilePath);
            }
        }



        private static string GetGridPath(Language lg)
        {
            switch (lg)
            {
                case Language.CN:
                    return CNFilePath() + "\\grid\\";
                case Language.EN:
                    return ENFilePath() + "\\grid\\";
                default:
                    return CNFilePath() + "\\grid\\";
            }
        }

        private static Dictionary<string, string> GetAppText(TextFilePath textFilePath)
        {
            Dictionary<string, string> textCollection = new Dictionary<string, string>();
            if (Directory.Exists(textFilePath() + "\\genaral"))
            {
                foreach (string d in Directory.GetFileSystemEntries(textFilePath() + "\\genaral"))
                {
                    if (Directory.Exists(d))
                    {
                        foreach (string dd in Directory.GetFileSystemEntries(d))
                        {
                            Dictionary<string, string> nonGridTextCollection = XmlHelper.GetAppText(dd, "/layout");
                            foreach (KeyValuePair<string, string> dic in nonGridTextCollection)
                            {
                                if (!textCollection.ContainsKey(dic.Key))
                                    textCollection.Add(dic.Key, dic.Value);
                            }
                        }
                    }
                    else
                    {
                        Dictionary<string, string> nonGridTextCollection = XmlHelper.GetAppText(d, "/layout");
                        foreach (KeyValuePair<string, string> dic in nonGridTextCollection)
                        {
                            if (!textCollection.ContainsKey(dic.Key))
                                textCollection.Add(dic.Key, dic.Value);
                        }
                    }
                }
            }
            return textCollection;
        }


        private static Dictionary<string, string> GetTablePrefix()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Content\\text\\IdPrefixStr.xml";
            Dictionary<string, string> tablePrefixCollection = XmlHelper.GetAppText(path, "/Table");
            return tablePrefixCollection;
        }


        private static string CNFilePath()
        {
            return AppDomain.CurrentDomain.BaseDirectory + "Content\\text\\cn";
        }

        private static string ENFilePath()
        {
            return AppDomain.CurrentDomain.BaseDirectory + "Content\\text\\en";
        }

        protected static Language GetLanguage()
        {
            Language lg = new Language();
            switch (ConfigurationManager.AppSettings["Language"].ToString())
            {
                case "CN":
                    lg = Language.CN;
                    break;
                case "EN":
                    lg = Language.EN;
                    break;
            }
            return lg;
        }

        protected static DBMS GetDBMS()
        {
            DBMS dbms = new DBMS();
            switch (ConfigurationManager.AppSettings["Database"].ToString())
            {
                case "SqlServer":
                    dbms = DBMS.SqlServer;
                    break;
                case "Access":
                    dbms = DBMS.Access;
                    break;
                case "Oracle":
                    dbms = DBMS.Oracle;
                    break;
            }
            return dbms;
        }
    }
}

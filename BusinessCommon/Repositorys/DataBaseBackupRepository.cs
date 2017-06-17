using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Basic;
using BaseCommon.Data;
using System.Configuration;
using System.Data.Common;
using System.IO;

namespace BusinessCommon.Repositorys
{
    public class DataBaseBackupRepository
    {
        static bool TaskExcuting = false;
        public static void ExcuteAutoBackUpByThread()
        {
            if (!TaskExcuting)
            {
                TaskExcuting = true;
                try
                {
                    DBBackUp("");
                }
                catch (Exception ex)
                {
                    AppLog.WriteLog(AppMember.AppText["SystemUser"], LogType.Error, "AutoTask", ex.Message);
                    throw ex;
                }
                finally
                {
                    TaskExcuting = false;
                }
            }

        }

        private static int BackUp(string filePath)
        {
            DateTime dtnow = DateTime.Now;
            string fileName = "AppDBHZ_" + dtnow.ToString("yyyyMMdd_hhmmss") + ".bak";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string path = "";
            if (filePath.EndsWith("\\"))
                path = filePath + fileName;
            else
                path = filePath + "\\" + fileName;
            string sql = "backup database AppDBHZ to disk='" + path + "'";
            AppMember.DbHelper.ExecuteSql(sql);
            string connstrs = ConfigurationManager.ConnectionStrings["BackupConnectionString"].ConnectionString;
            DbProviderFactory provider = DbProviderFactories.GetFactory(
                     ConfigurationManager.ConnectionStrings["BackupConnectionString"].ProviderName);
            DatabaseHelper dbHelper = new DatabaseHelper(connstrs, provider);
            dbHelper.ExecuteSql(sql);
            CopyFile(filePath, fileName);
            return 1;
        }

        static System.Timers.Timer myTimer;
        public static void ExcuteAutoBackUp()
        {
            int timerIntervalMinute = DataConvert.ToInt32(ConfigurationManager.AppSettings["TimerIntervalMinute"].ToString());
            myTimer = new System.Timers.Timer(1000 * 60 * timerIntervalMinute);
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            myTimer.Enabled = true;
            myTimer.AutoReset = true;

        }
        private static void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            myTimer.Stop();
            try
            {
                DBBackUp("2");
            }
            catch (Exception ex)
            {
                AppLog.WriteLog(AppMember.AppText["SystemUser"], LogType.Error, "AutoTask", ex.Message);
                throw ex;
            }
            finally
            {
                myTimer.Start();
            }

        }

        public static void DBBackUp(string tag)
        {
            //AppLog.WriteLog(AppMember.AppText["SystemUser"], LogType.Debug, "AutoTask"+tag, string.Format(AppMember.AppText["AutoBackupProcess"]));
            DateTime dtnow = DateTime.Now;
            string path = ConfigurationManager.AppSettings["DBBackupPath"].ToString();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string[] files = Directory.GetFileSystemEntries(path);
            bool existFile = false;
            foreach (string file in files)
            {
                if (file.Contains(dtnow.ToString("yyyyMMdd")))
                {
                    existFile = true;
                    break;
                }
            }
            string day = ConfigurationManager.AppSettings["DBBackupDay"].ToString();
            int intDay = (int)dtnow.DayOfWeek;
            if (DataConvert.ToString(intDay) == day && !existFile)
            {
                BackUp(path);
            }
        }


        private static void CopyFile(string sourcePath, string fileName)
        {
            string autoCopy = ConfigurationManager.AppSettings["AutoCopy"].ToString();
            string path = ConfigurationManager.AppSettings["DBCopyPath"].ToString();
            if (autoCopy == "true")
            {
                if (!System.IO.Directory.Exists(path))
                {
                    // 目录不存在，建立目录
                    System.IO.Directory.CreateDirectory(path);
                }
                bool isrewrite = true; // true=覆盖已存在的同名文件,false则反之
                System.IO.File.Copy(sourcePath + fileName, path + fileName, isrewrite);
            }
        }
    }
}

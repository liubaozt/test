using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BaseCommon.Init;
using WebCommon.Common;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness.Repositorys;
using BaseCommon.Basic;
using System.Configuration;
using BusinessCommon.Repositorys;
using System.Net;
using System.IO;
using System.Threading;

namespace WebApp
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.Add(new Route("ReportView/{page}", new WebFormsRouteHandler()));

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Home", action = "Login", id = UrlParameter.Optional } // 参数默认值
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
            AppInit.Init();
            Thread thread = new Thread(new ThreadStart(AutoDepreciationTask));
            thread.Start();

            //Thread thread2 = new Thread(new ThreadStart(AutoDBBackUpTask));
            //thread2.Start();

            Thread thread3 = new Thread(new ThreadStart(AutoMonthUpdateTask));
            thread3.Start();
        }

        void AutoDepreciationTask()
        {
            if (AppMember.AutoDepreciation == "true")
            {
                AppLog.WriteLog(AppMember.AppText["SystemUser"], LogType.Debug, "AutoTask", string.Format(AppMember.AppText["AutoDepreciationTask"]));
                while (true)
                {
                    AutoDepreciation.ExcuteAutoUpdateByThread();
                    int timerIntervalMinute = DataConvert.ToInt32(ConfigurationManager.AppSettings["TimerIntervalMinute"].ToString());
                    Thread.CurrentThread.Join(1000 * 60 * timerIntervalMinute);//阻止timerIntervalMinute分钟   
                }
            }
        }

        void AutoMonthUpdateTask()
        {
            if (AppMember.AutoMonthUpdate == "true")
            {
                AppLog.WriteLog(AppMember.AppText["SystemUser"], LogType.Debug, "AutoTask", string.Format(AppMember.AppText["AutoMonthUpdateTask"]));
                AutoMonthUpdate.ExcuteAutoUpdate();
            }
        }

        void AutoDBBackUpTask()
        {
            string autoBackup = ConfigurationManager.AppSettings["AutoBackup"].ToString();
            if (autoBackup == "true")
            {
                AppLog.WriteLog(AppMember.AppText["SystemUser"], LogType.Debug, "AutoTask", string.Format(AppMember.AppText["AutoBackupTask"]));
                while (true)
                {
                    DataBaseBackupRepository.ExcuteAutoBackUpByThread();
                    int timerIntervalMinute = DataConvert.ToInt32(ConfigurationManager.AppSettings["TimerIntervalMinute"].ToString());
                    Thread.CurrentThread.Join(1000 * 60 * timerIntervalMinute);//阻止timerIntervalMinute分钟  
                }
            }
        }


        protected void Application_End(object sender, EventArgs e)
        {
            //AppLog.WriteLog(AppMember.AppText["SystemUser"], LogType.Debug, "AutoTask", string.Format(AppMember.AppText["AutoTaskEnd"]));
            //Thread.Sleep(1000);
            /////下面的代码是关键，可解决IIS应用程序池自动回收的问题   
            ////这里设置你的web地址，可以随便指向你的任意一个aspx页面甚至不存在的页面，目的是要激发Application_Start   
            ////string url = HttpRuntime.AppDomainAppVirtualPath + "/Home/Error";
            ////string s1=  RouteTable.Routes["Default"].GetRouteData;
            ////string ss=UrlHelper.GenerateUrl("Default","Error","Home",null,RouteTable.Routes,app
            ////string sss = Request.Path;
            ////AppLog.WriteLog(AppMember.AppText["SystemUser"], LogType.Debug, "AutoTask", sss);
            //string hostUrl = ConfigurationManager.AppSettings["HostUrl"].ToString();
            //if (!hostUrl.EndsWith("/"))
            //    hostUrl = hostUrl + "/";
            //string url = hostUrl + "Home/Error";
            //AppLog.WriteLog(AppMember.AppText["SystemUser"], LogType.Debug, "AutoTask", url);
            //HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            //Stream receiveStream = myHttpWebResponse.GetResponseStream();//得到回写的字节流   

        }
    }
}
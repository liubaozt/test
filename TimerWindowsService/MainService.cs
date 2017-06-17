using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration;
using System.Threading;
using System.Data.Common;

namespace TimerWindowsService
{
    public partial class MainService : ServiceBase
    {
        public MainService()
        {
            InitializeComponent();
        }


        protected override void OnStart(string[] args)
        {
            ExcuteAutoBackUp();
        }

        protected override void OnStop()
        {

        }

        System.Timers.Timer myTimer;
        public void ExcuteAutoBackUp()
        {
           int TimerIntervalMinute=Convert.ToInt32( ConfigurationManager.AppSettings["TimerIntervalMinute"].ToString());
           myTimer = new System.Timers.Timer(1000 * 60 * TimerIntervalMinute);
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnAutoDBBackUpTaskEvent);
            myTimer.Enabled = true;
            myTimer.AutoReset = true;

        }
        private void OnAutoDBBackUpTaskEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            myTimer.Stop();
            try
            {
                TimerWindowsService.AutoTaskService.AutoTaskSoapClient service = new AutoTaskService.AutoTaskSoapClient();
                service.StartTask();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                myTimer.Start();
            }

        }

    }
}

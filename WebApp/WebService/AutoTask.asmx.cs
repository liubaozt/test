using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BaseCommon.Init;
using BusinessCommon.Repositorys;

namespace WebApp.WebService
{
    /// <summary>
    /// AutoTask 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class AutoTask : System.Web.Services.WebService
    {
        public AutoTask()
        {
            AppInit.Init();
        }

        [WebMethod]
        public int StartTask()
        {
            return 1;
        }

        [WebMethod]
        public int ExcuteAutoBackUp()
        {
            DataBaseBackupRepository.DBBackUp("3");
            return 1;
        }
    }
}

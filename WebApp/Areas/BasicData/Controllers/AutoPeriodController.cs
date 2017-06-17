using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseCommon.Data;
using System.Data;
using BusinessLogic.BasicData.Models.AutoPeriod;
using WebCommon.Common;
using BaseCommon.Basic;
using System.Web.Caching;
using WebCommon.Init;
using BusinessLogic.BasicData.Repositorys;
using BaseCommon.Repositorys;
using WebCommon.HttpBase;

namespace WebApp.Areas.BasicData.Controllers
{
    public class AutoPeriodController : BaseController
    {
        public AutoPeriodController()
        {
            ControllerName = "AutoPeriod";
        }

        [AppAuthorize]
        public ActionResult Entry(string pageId, string viewTitle)
        {
            EntryModel model = new EntryModel();
            model.PageId = pageId;
            model.ViewTitle = viewTitle;
            model.FormId = "EntryForm";
            model.SaveUrl = Url.Action("Entry");
            model.CustomClick = false;
            return View(model);

        }

        [AppAuthorize]
        [HttpPost]
        public ActionResult Entry(EntryModel model)
        {
            AutoPeriodRepository rep = new AutoPeriodRepository();
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            rep.Update(sysUser, model);
            return View(model);

        }

     
    }
}

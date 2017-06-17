using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.BaseWeb.Common;
using BusinessLogic.Report.Models.AssetsMaintainReport;
using BaseCommon.Data;
using WebApp.BaseWeb.Init;
using BaseCommon.Basic;
using System.Data;
using BusinessLogic.Report.Repositorys;
using System.Text;

namespace WebApp.Areas.Report.Controllers
{
    public class AssetsMaintainReportController : QueryReportController
    {

        public AssetsMaintainReportController()
        {
            ControllerName = "AssetsMaintainReport";
            Repository = new AssetsMaintainReportRepository();
        }

        public ActionResult Entry(string pageId, string primaryKey, string viewTitle)
        {
            EntryModel model = new EntryModel();
            model.EntryGridId = "EntryGrid";
            model.EntryGridTitle = AppMember.AppText["AssetsMaintainStayList"].ToString();
            SetEntryModel(pageId, viewTitle, model);
            SetMustModel(model);
            return View(model);
        }

        private void SetMustModel(EntryModel model)
        {
            model.EntryGridLayout = EntryGridLayout();
        }
    }
}

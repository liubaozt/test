using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.BaseWeb.Common;
using BusinessLogic.Report.Models.AssetsTransferReport;
using BaseCommon.Data;
using WebApp.BaseWeb.Init;
using BaseCommon.Basic;
using System.Data;
using BusinessLogic.Report.Repositorys;
using System.Text;

namespace WebApp.Areas.Report.Controllers
{
    public class AssetsTransferReportController : QueryReportController
    {

        public AssetsTransferReportController()
        {
            ControllerName = "AssetsTransferReport";
            Repository = new AssetsTransferReportRepository();
        }

        public ActionResult Entry(string pageId, string primaryKey, string viewTitle)
        {
            EntryModel model = new EntryModel();
            model.EntryGridId = "EntryGrid";
            model.EntryGridTitle = AppMember.AppText["AssetsTransferStayList"].ToString();
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

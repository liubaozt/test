using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.BaseWeb.Common;
using BusinessLogic.Report.Models.AssetsScrapReport;
using BaseCommon.Data;
using WebApp.BaseWeb.Init;
using BaseCommon.Basic;
using System.Data;
using BusinessLogic.Report.Repositorys;
using System.Text;

namespace WebApp.Areas.Report.Controllers
{
    public class AssetsScrapReportController : QueryReportController
    {

        public AssetsScrapReportController()
        {
            ControllerName = "AssetsScrapReport";
            Repository = new AssetsScrapReportRepository();
        }

        public ActionResult Entry(string pageId, string primaryKey, string viewTitle)
        {
            EntryModel model = new EntryModel();
            model.EntryGridId = "EntryGrid";
            model.EntryGridTitle = AppMember.AppText["AssetsScrapStayList"].ToString();
            SetEntryModel(pageId, viewTitle, model);
            SetMustModel(model);
            return View(model);
        }

        private void SetMustModel(EntryModel model)
        {
            model.AssetsClassUrl = Url.Action("AssetsClassDropList", "DropList", new { Area = ""});
            model.DepartmentUrl = Url.Action("DepartmentDropList", "DropList", new { Area = ""});
            model.AssetsClassDialogUrl = Url.Action("Select", "AssetsClass", new { Area = "BasicData" });
            model.DepartmentDialogUrl = Url.Action("Select", "Department", new { Area = "BusinessCommon" });
            model.EntryGridLayout = EntryGridLayout();
        }

    }
}

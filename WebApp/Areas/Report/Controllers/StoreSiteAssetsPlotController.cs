using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic.Report.Models.StoreSiteAssetsPlot;
using WebCommon.Common;
using BusinessLogic.Report.Repositorys;

namespace WebApp.Areas.Report.Controllers
{
    public class StoreSiteAssetsPlotController : PlotController
    {
        //
        // GET: /Report/AssetsClassPlot/

        public ActionResult Index(string pageId, string formMode, string viewTitle)
        {
            EntryModel model = new EntryModel();
            model.PageId = pageId;
            model.ViewTitle = viewTitle;
            model.ExportUrl = Url.Action("ExcelExport");
            model.FormId = "QueryForm";
            return View(model);
        }

        public ActionResult Search(string pageId, string formVar)
        {
            StoreSiteAssetsPlotRepository rep = new StoreSiteAssetsPlotRepository();
            string vars = rep.GetSource(formVar);
            return JavaScript(vars);
        }

    }
}

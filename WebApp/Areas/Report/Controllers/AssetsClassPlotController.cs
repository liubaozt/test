using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic.Report.Models.AssetsClassPlot;
using WebCommon.Common;
using BaseCommon.Data;
using BusinessLogic.Report.Repositorys;

namespace WebApp.Areas.Report.Controllers
{
    public class AssetsClassPlotController : PlotController
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
            model.AssetsClassUrl = Url.Action("DropList", "AssetsClass", new { Area = "BasicData", currentId = model.AssetsClassId });
            model.AssetsClassDialogUrl = Url.Action("Select", "AssetsClass", new { Area = "BasicData" });
            model.AssetsClassAddFavoritUrl = Url.Action("AddFavorit", "AssetsClass", new { Area = "BasicData", tableName = "AssetsClass" });
            model.AssetsClassReplaceFavoritUrl = Url.Action("ReplaceFavorit", "AssetsClass", new { Area = "BasicData", tableName = "AssetsClass" });
            model.DepartmentUrl = Url.Action("DropList", "Department", new { Area = "BusinessCommon", currentId = model.DepartmentId });
            model.DepartmentDialogUrl = Url.Action("Select", "Department", new { Area = "BusinessCommon" });
            model.DepartmentAddFavoritUrl = Url.Action("AddFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });
            model.DepartmentReplaceFavoritUrl = Url.Action("ReplaceFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });
            return View(model);
        }


        public ActionResult Search(string pageId, string formVar)
        {
            AssetsClassPlotRepository rep = new AssetsClassPlotRepository();
            string vars =rep.GetSource(formVar);
            return JavaScript(vars);
        }
    }
}

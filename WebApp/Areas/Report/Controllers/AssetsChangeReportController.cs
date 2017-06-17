using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCommon.Common;
using BusinessLogic.Report.Models.AssetsChangeReport;
using BaseCommon.Basic;
using BaseCommon.Data;
using WebCommon.Init;
using System.Data;
using BusinessLogic.Report.Repositorys;
using WebCommon.HttpBase;

namespace WebApp.Areas.Report.Controllers
{
    public class AssetsChangeReportController : QueryController
    {
        public AssetsChangeReportController()
        {
            ControllerName = "AssetsChangeReport";
            Repository = new AssetsChangeReportRepository();
        }

        [AppAuthorize]
        public ActionResult Entry(string pageId, string formMode, string viewTitle)
        {
            EntryModel model = new EntryModel();
            model.FormMode = formMode;
            SetParentEntryModel(pageId, viewTitle, formMode, model);
            SetThisModel(model);
            return View(model);
        }


        private void SetThisModel(EntryModel model)
        {
            model.GridHeight = 390;
            model.EntryGridId = "EntryGrid";
            model.AssetsClassUrl = Url.Action("DropList", "AssetsClass", new { Area = "BasicData", filterExpression = "assetsClassId=" + DFT.SQ + model.AssetsClassId + DFT.SQ });
            model.DepartmentUrl = Url.Action("DropList", "Department", new { Area = "BusinessCommon", filterExpression = "departmentId=" + DFT.SQ + model.DepartmentId + DFT.SQ });
            model.AssetsClassDialogUrl = Url.Action("Select", "AssetsClass", new { Area = "BasicData" });
            model.DepartmentDialogUrl = Url.Action("Select", "Department", new { Area = "BusinessCommon" });
            model.DepartmentAddFavoritUrl = Url.Action("AddFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });
            model.DepartmentReplaceFavoritUrl = Url.Action("ReplaceFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });
            model.AssetsClassAddFavoritUrl = Url.Action("AddFavorit", "AssetsClass", new { Area = "BasicData", tableName = "AssetsClass" });
            model.AssetsClassReplaceFavoritUrl = Url.Action("ReplaceFavorit", "AssetsClass", new { Area = "BasicData", tableName = "AssetsClass" });
            model.StoreSiteUrl = Url.Action("DropList", "StoreSite", new { Area = "BasicData", currentId = model.StoreSiteId });
            model.StoreSiteDialogUrl = Url.Action("Select", "StoreSite", new { Area = "BasicData" });
            model.StoreSiteAddFavoritUrl = Url.Action("AddFavorit", "StoreSite", new { Area = "BasicData", tableName = "StoreSite" });
            model.StoreSiteReplaceFavoritUrl = Url.Action("ReplaceFavorit", "StoreSite", new { Area = "BasicData", tableName = "StoreSite" });

        }

        protected override AdvanceGridLayout EntryGridLayout(string formMode)
        {
            string layourName = "";
            if (DataConvert.ToString(formMode) == "")
                formMode = Request.QueryString["formMode"];
            switch (formMode)
            {
                case "AssetsClass":
                    layourName = "AssetsClassChangeReport";
                    break;
                case "Department":
                    layourName = "AssetsDepartmentChangeReport";
                    break;
                case "StoreSite":
                    layourName = "AssetsStoreSiteChangeReport";
                    break;
            }
            if (HttpContext.Cache[layourName + "GridLayout"] == null)
            {
                string areaName = RouteData.DataTokens["area"].ToString();

                CacheInit.CreateAdvanceGridLayoutCache(HttpContext, areaName, layourName);
            }
            return (AdvanceGridLayout)HttpContext.Cache[layourName + "GridLayout"];
        }


    }
}

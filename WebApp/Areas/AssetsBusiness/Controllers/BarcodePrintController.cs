using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseCommon.Data;
using System.Data;
using BusinessLogic.AssetsBusiness.Models.BarcodePrint;
using WebCommon.Common;
using BaseCommon.Basic;
using System.Web.Caching;
using WebCommon.Init;
using BaseControl.HtmlHelpers;
using WebCommon.HttpBase;
using BusinessLogic.AssetsBusiness.Repositorys;
using BusinessCommon.Repositorys;

namespace WebApp.Areas.AssetsBusiness.Controllers
{
    public class BarcodePrintController : MaintainController
    {
        public BarcodePrintController()
        {
            ControllerName = "BarcodePrint";

        }

        protected GridLayout EntryGridLayout()
        {
            string tag = "";
            if (AppMember.ViewVersion == "Cus_Simple")
                tag = "Cus_Simple_";
            if (HttpContext.Cache[tag+"BarcodePrintEntryGridLayout"] == null)
            {
                CacheInit.CreateGridLayoutCache(HttpContext, "AssetsBusiness", tag+"BarcodePrintEntry");
            }
            return (GridLayout)HttpContext.Cache[tag+"BarcodePrintEntryGridLayout"];
        }

        [HttpPost]
        public JsonResult EntryGridData(string formVar, string formMode, string primaryKey)
        {
            var rows = DataTable2Object.Data(EntryGridDataTable(formVar, formMode, primaryKey), EntryGridLayout().GridLayouts);
            var result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = new { page = 1, total = 1, rows = rows };
            return result;
        }

        protected DataTable EntryGridDataTable(string formVar, string formMode, string primaryKey)
        {
            EntryModel model = new EntryModel();
            Dictionary<string, object> paras = SetParas(formVar);
            return model.BarcodePrintRepository.GetEntryGridDataTable(formVar, formMode, primaryKey);
        }

        protected GridLayout StoreSiteGridLayout()
        {
            if (HttpContext.Cache["StoreSiteGridLayout"] == null)
            {
                CacheInit.CreateGridLayoutCache(HttpContext, "BasicData", "StoreSite");
            }
            return (GridLayout)HttpContext.Cache["StoreSiteGridLayout"];
        }

        [HttpPost]
        public JsonResult StoreSiteGridData(string formVar, string formMode, string primaryKey)
        {
            var rows = DataTable2Object.Data(StoreSiteGridDataTable(formVar, formMode, primaryKey), StoreSiteGridLayout().GridLayouts);
            var result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = new { page = 1, total = rows.Length, rows = rows };
            return result;
        }

        protected DataTable StoreSiteGridDataTable(string formVar, string formMode, string primaryKey)
        {
            EntryModel model = new EntryModel();
            Dictionary<string, object> paras = SetParas(formVar);
            return model.BarcodePrintRepository.GetStoreSiteGridDataTable(paras, formMode, primaryKey);
        }

        [AppAuthorize]
        public ActionResult Entry(string pageId, string primaryKey, string formMode, string viewTitle)
        {
            ClearClientPageCache(Response);
            EntryModel model = new EntryModel();
            SetParentEntryModel(pageId, formMode, viewTitle, model);
            model.EntryGridLayout = EntryGridLayout();
            model.EntryGridId = "EntryGrid";
            model.SelectUrl = Url.Action("Select", "AssetsManage", new { Area = "AssetsBusiness", pageId = model.PageId });
            model.BarcodeStyleId = model.BarcodeStyleRepository.GetBarcodeDefaultStyle();
            model.DefaultAssetsId = model.AssetsRepository.GetDefaultAssetsId();
            return View(model);
        }

        [AppAuthorize]
        public ActionResult Entry2(string pageId, string primaryKey, string formMode, string viewTitle)
        {
            ClearClientPageCache(Response);
            Entry2Model model = new Entry2Model();
            SetParentEntryModel(pageId, formMode, viewTitle, model);
            model.SaveUrl = Url.Action("Entry2");
            model.EntryGridLayout = EntryGridLayout();
            model.EntryGridId = "EntryGrid";
            model.PrinterName = AppMember.PrinterName;
            model.SelectUrl = Url.Action("Select", "AssetsManage", new { Area = "AssetsBusiness", pageId = model.PageId });
            model.BarcodeStyleId = model.BarcodeStyleRepository.GetBarcodeDefaultStyle();
            model.DefaultAssetsId = model.AssetsRepository.GetDefaultAssetsId();
            if (formMode == "new2")
                model.LabelType = "A";
            else if (formMode == "storeSite")
                model.LabelType = "S";
            model.StoreSiteGridLayout = StoreSiteGridLayout();
            model.StoreSiteGridId = "StoreSiteGrid";
            SetThisEntryModel(model);
            return View("Entry3", model);
        }

        private void SetThisEntryModel(Entry2Model model)
        {
            model.AssetsClassUrl = Url.Action("DropList", "AssetsClass", new { Area = "BasicData", currentId = model.AssetsClassId });
            model.AssetsClassDialogUrl = Url.Action("Select", "AssetsClass", new { Area = "BasicData" });
            model.AssetsClassAddFavoritUrl = Url.Action("AddFavorit", "AssetsClass", new { Area = "BasicData", tableName = "AssetsClass" });
            model.AssetsClassReplaceFavoritUrl = Url.Action("ReplaceFavorit", "AssetsClass", new { Area = "BasicData", tableName = "AssetsClass" });
            model.DepartmentUrl = Url.Action("DropList", "Department", new { Area = "BusinessCommon", currentId = model.DepartmentId });
            model.DepartmentDialogUrl = Url.Action("Select", "Department", new { Area = "BusinessCommon" });
            model.DepartmentAddFavoritUrl = Url.Action("AddFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });
            model.DepartmentReplaceFavoritUrl = Url.Action("ReplaceFavorit", "Department", new { Area = "BusinessCommon", tableName = "AppDepartment" });
            model.StoreSiteUrl = Url.Action("DropList", "StoreSite", new { Area = "BasicData", currentId = model.StoreSiteId });
            model.StoreSiteDialogUrl = Url.Action("Select", "StoreSite", new { Area = "BasicData" });
            model.StoreSiteAddFavoritUrl = Url.Action("AddFavorit", "StoreSite", new { Area = "BasicData", tableName = "StoreSite" });
            model.StoreSiteReplaceFavoritUrl = Url.Action("ReplaceFavorit", "StoreSite", new { Area = "BasicData", tableName = "StoreSite" });
            UserRepository user = new UserRepository();
            model.UserSource = user.UserAutoCompleteSource();
        }

        [HttpPost]
        public ActionResult Entry(EntryModel model)
        {
            return View(model);
        }

        [HttpPost]
        public ActionResult Entry2(Entry2Model model)
        {
            model.EntryGridLayout = EntryGridLayout();
            model.SelectUrl = Url.Action("Select", "AssetsManage", new { Area = "AssetsBusiness", pageId = model.PageId });
            model.StoreSiteGridLayout = StoreSiteGridLayout();
            BarcodePrintRepository Repository = new BarcodePrintRepository();
            Update(Repository, model, model.ViewTitle);
            if (model.LabelType == "A")
                model.FormMode = "new2";
            else if (model.LabelType == "S")
                model.FormMode = "storeSite";
            SetThisEntryModel(model);
            return View("Entry3", model);
        }

        public ActionResult LoadLabelStyle(string pageId, string barcodeStyleId)
        {
            BarcodePrintRepository Repository = new BarcodePrintRepository();
            return Content(Repository.GetLabelStyle(barcodeStyleId), "text/html");
        }

        public ActionResult LoadDefaultLabelStyle(string pageId, string formMode)
        {
            BarcodePrintRepository Repository = new BarcodePrintRepository();
            return Content(Repository.GetDefaultLabelStyle(formMode), "text/html");
        }

        public virtual JsonResult DropList(string formMode, string filterExpression)
        {
            ClearClientPageCache(Response);
            BarcodePrintRepository rep = new BarcodePrintRepository();
            DataTable source = rep.GetDropListSource();
            filterExpression = DFT.HandleExpress(filterExpression);
            if (formMode == "new2")
                filterExpression = "labelType='A'";
            else if (formMode == "storeSite")
                filterExpression = "labelType='S'";
            List<DropListSource> dropList = rep.DropList(source, filterExpression);
            return DropListJson(dropList);
        }

        protected int Update(BarcodePrintRepository rep, Entry2Model model, string viewTitle)
        {
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            DataUpdate dbUpdate = new DataUpdate();
            try
            {
                dbUpdate.BeginTransaction();
                rep.DbUpdate = dbUpdate;
                rep.Update(model, sysUser, viewTitle);
                dbUpdate.Commit();
                return 1;
            }
            catch (Exception ex)
            {
                dbUpdate.Rollback();
                model.Message = ex.Message;
                model.HasError = "true";
                return 0;
            }
            finally
            {
                dbUpdate.Close();
            }
        }

        [HttpPost]
        public ActionResult Print(EntryModel model)
        {
            ClearClientPageCache(Response);
            model.BarcodeStyleId = model.BarcodeStyleId;
            //model.DefaultAssetsId = model.AssetsRepository.GetDefaultAssetsId();
            List<AssetsPrint> gridData = JsonHelper.JSONStringToList<AssetsPrint>(DataConvert.ToString(model.EntryGridString));
            model.AllAssets = gridData;
            model.DefaultAssetsId = gridData[0].AssetsId;
            model.CurrentAssets = gridData[0];
            Session["PrintAssets"] = gridData;
            Session["PrintIndex"] = 0;
            model.AssetsCount = gridData.Count.ToString();
            model.BarcodeStyle = model.BarcodeStyleRepository.GetBarcodeStyle(model.BarcodeStyleId);
            Session["PrintBarcodeStyle"] = model.BarcodeStyle;
            return View(model);
        }

        protected Dictionary<string, object> SetParas(string formVar)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            if (DataConvert.ToString(formVar) != "")
            {
                EntryModel model = JsonHelper.Deserialize<EntryModel>(formVar);
            }
            return paras;
        }

        //[HttpPost]
        //public ActionResult ChangeBarcodeStyle(string pageId, string barcodeStyleId, string assetsId)
        //{
        //    if (DataConvert.ToString(assetsId) == "")
        //    {
        //        EntryModel model = new EntryModel();
        //        assetsId = model.AssetsRepository.GetDefaultAssetsId();
        //    }
        //    string style = AppBarcodeStyle.BarcodeStyle(this.Url, pageId, barcodeStyleId, assetsId);
        //    return Content(style, "text/html");
        //}


        [HttpPost]
        public ActionResult PreviousAssets(string pageId)
        {
            List<AssetsPrint> ap = (List<AssetsPrint>)Session["PrintAssets"];
            int i = (int)Session["PrintIndex"];
            i = i - 1;
            if (i < 0)
                return Content("0", "text/html");
            DataTable dt = (DataTable)Session["PrintBarcodeStyle"];
            string style = AppBarcodeStyle.BarcodeStyle<AssetsPrint>(this.Url, pageId, dt, ap[i]);
            Session["PrintIndex"] = i;
            return Content(style, "text/html");
        }

        [HttpPost]
        public ActionResult NextAssets(string pageId)
        {
            List<AssetsPrint> ap = (List<AssetsPrint>)Session["PrintAssets"];
            int i = (int)Session["PrintIndex"];
            i = i + 1;
            if (i > ap.Count - 1)
                return Content("0", "text/html");
            DataTable dt = (DataTable)Session["PrintBarcodeStyle"];
            string style = AppBarcodeStyle.BarcodeStyle<AssetsPrint>(this.Url, pageId, dt, ap[i]);
            Session["PrintIndex"] = i;
            return Content(style, "text/html");
        }

        [HttpPost]
        public ActionResult PrintAll(string pageId)
        {
            List<AssetsPrint> ap = (List<AssetsPrint>)Session["PrintAssets"];
            Session["PrintIndex"] = 0;
            DataTable dt = (DataTable)Session["PrintBarcodeStyle"];
            string style = AppBarcodeStyle.BarcodeStyle<AssetsPrint>(this.Url, pageId, dt, ap[0]);
            return Content(style, "text/html");
        }



    }
}

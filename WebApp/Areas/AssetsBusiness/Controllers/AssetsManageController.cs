using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseCommon.Data;
using System.Data;
using BusinessLogic.AssetsBusiness.Models.AssetsManage;
using WebCommon.Common;
using BaseCommon.Basic;
using System.Web.Caching;
using WebCommon.Init;
using BusinessCommon.Repositorys;
using System.Data.Common;
using BusinessLogic.AssetsBusiness;
using BusinessLogic.AssetsBusiness.Repositorys;
using BaseCommon.Repositorys;
using WebCommon.HttpBase;
using System.Text;

namespace WebApp.Areas.AssetsBusiness.Controllers
{
    public class AssetsManageController : ApproveMasterController
    {
        AssetsRepository Repository;
        public AssetsManageController()
        {
            ControllerName = "Assets";
            ExportFileName = "AST";
        }

        protected override IApproveMasterFactory CreateRepository()
        {
            Repository = new AssetsRepository();
            return new ApproveMasterRepositoryFactory<AssetsRepository>(Repository);
        }

        [AppAuthorize]
        public ActionResult List(string pageId, string viewTitle, string listMode)
        {
            ListModel model = new ListModel();
            SetParentListModel(pageId, viewTitle, listMode, "Assets", model);
            SetThisListModel(model);
            model.GridPkField = "assetsId";
            return View(model);
        }

        protected override GridLayout GridLayout(string listMode, string selectMode)
        {
            string tag = "";
            if (AppMember.ViewVersion == "Cus_Simple")
                tag = "Cus_Simple_";
            if (listMode == "approve" || listMode == "reapply")
            {
                if (HttpContext.Cache["AssetsApproveGridLayout"] == null)
                {
                    CacheInit.CreateGridLayoutCache(HttpContext, "Common", "AssetsApprove");
                }
                return (GridLayout)HttpContext.Cache["AssetsApproveGridLayout"];
            }
            else
            {
                if (selectMode == "AssetsSelect")
                {
                    if (HttpContext.Cache[tag + "AssetsSelectGridLayout"] == null)
                    {
                        CacheInit.CreateGridLayoutCache(HttpContext, "AssetsBusiness", tag + "AssetsSelect");
                    }
                    return (GridLayout)HttpContext.Cache[tag + "AssetsSelectGridLayout"];
                }
                else if (selectMode == "BorrowAssetsSelect")
                {
                    if (HttpContext.Cache["BorrowAssetsSelectGridLayout"] == null)
                    {
                        CacheInit.CreateGridLayoutCache(HttpContext, "AssetsBusiness", "BorrowAssetsSelect");
                    }
                    return (GridLayout)HttpContext.Cache["BorrowAssetsSelectGridLayout"];
                }
                else
                {
                    if (AppMember.DepreciationRuleOpen)
                    {
                        if (HttpContext.Cache[tag + "Assets_DepreciationRuleGridLayout"] == null)
                        {
                            CacheInit.CreateGridLayoutCache(HttpContext, "AssetsBusiness", tag + "Assets_DepreciationRule");
                        }
                        return (GridLayout)HttpContext.Cache[tag + "Assets_DepreciationRuleGridLayout"];
                    }
                    else
                    {
                        if (HttpContext.Cache[tag + "AssetsGridLayout"] == null)
                        {
                            CacheInit.CreateGridLayoutCache(HttpContext, "AssetsBusiness", tag + "Assets");
                        }
                        return (GridLayout)HttpContext.Cache[tag + "AssetsGridLayout"];
                    }
                }
            }
        }
        private void SetModelFromPurchase(EntryModel model,string purchaseObj)
        {
            if (DataConvert.ToString(purchaseObj) != "")
            {
               var obj= JsonHelper.Deserialize<AssetsPurchase>(purchaseObj);
               model.AssetsName = obj.assetsName;
               model.DepartmentId = obj.departmentId;
               model.StoreSiteId = obj.storeSiteId;
               model.AssetsValue =DataConvert.ToDoubleNull(obj.assetsValue);
               model.UsePeople = obj.usePeople;
               model.Keeper = obj.keeper;
               model.AssetsPurchaseDetailId = obj.assetsPurchaseDetailId;
               model.AssetsPurchaseId = obj.assetsPurchaseId;
            }
        }

        [AppAuthorize]
        public ActionResult Entry(string pageId, string primaryKey, string formMode, string viewTitle, string purchaseObj) 
        {
            ClearClientPageCache(Response);
            EntryModel model = new EntryModel();
            Repository.SetModel(primaryKey, formMode, model);
            SetParentEntryModel(pageId, primaryKey, formMode, viewTitle, model);
            SetThisEntryModel(model);
            if (formMode == "new" || formMode == "new2")
            {
                model.PurchaseTypeId = "PHT150425001";
                SetModelFromPurchase(model, purchaseObj);         
            }
            else
            {
                AssetsRepository rep = new AssetsRepository();
                model.IsStartDepreciation = rep.GetStartDepreciationValue(primaryKey);
            }
            if (AppMember.ViewVersion == "Cus_Simple")
                return View("Cus_Simple_Entry", model);
            else
                return View(model);
        }

        [AppAuthorize]
        [HttpPost]
        public ActionResult Entry(EntryModel model, string approveReturn)
        {
            ClearClientPageCache(Response);
            if (Update(Repository, model, model.AssetsId, approveReturn) == 1)
            {
                if (model.FormMode == "approve" || model.FormMode == "reapply")
                    return RedirectToAction("List", new { pageId = model.PageId, viewTitle = model.ViewTitle, listMode = model.FormMode });
                else if (model.FormMode == "new" || model.FormMode == "new2")
                {
                    SetThisEntryModel(model);
                    if (AppMember.ViewVersion == "Cus_Simple")
                        return View("Cus_Simple_Entry", model);
                    else
                        return View(model);
                }
                else
                {
                    return RedirectToAction("List", new { pageId = model.PageId, viewTitle = model.ViewTitle });
                }
            }
            else
            {
                if (model.FormMode == "approve")
                {
                    Repository.SetModel(model.ApprovePkValue, model.FormMode, model);
                    SetParentEntryModel(model.PageId, model.ApprovePkValue, model.FormMode, model.ViewTitle, model);
                }
                SetThisEntryModel(model);
                if (AppMember.ViewVersion == "Simple")
                    return View("Cus_Simple_Entry", model);
                else
                    return View(model);
            }
        }


        [HttpPost]
        public ActionResult GetAutoNo()
        {
            string no = AutoNoGenerator.GetMaxNo("Assets");
            return Content(no, "text/html");
        }

        private void SetThisEntryModel(EntryModel model)
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

        private void SetThisListModel(ListModel model)
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
            model.GridHeight = 410;
            UserRepository user = new UserRepository();
            model.UserSource = user.UserAutoCompleteSource();
        }

        public ActionResult Select(string pageId, string assetsState, string selectMode, string formName)
        {
            SelectModel model = new SelectModel();
            model.PageId = pageId;
            model.FormId = "SelectForm";
            model.GridId = "list";
            model.IsMultiSelect = true;
            model.GridWidth = 773;
            SetThisListModel(model);

            string checking = "";
            string assetsStateSql = "";
            if (DataConvert.ToString(formName) != "BarcodePrint")
            {
                checking = "and Assets.checking=" + DFT.SQ + "N" + DFT.SQ;
                //if (DataConvert.ToString(assetsState) == "")
                //    assetsState = "A,L,X,R,F";
                if (DataConvert.ToString(assetsState) == "")
                    assetsState = "";
            }
            else
            {
                assetsState = "";
            }
            if (assetsState != "")
            {
                if (assetsState.Contains(','))
                {
                    string[] aste = assetsState.Split(',');
                    string assetsStates = "";
                    foreach (string ss in aste)
                    {
                        assetsStates += DFT.SQ + ss + DFT.SQ + ",";
                    }
                    assetsStates = assetsStates.Substring(0, assetsStates.Length - 1);
                    assetsStateSql = "Assets.assetsState in (" + assetsStates + ") ";
                    //model.GridUrl = Url.Action("GridData", new { filterString = "Assets.assetsState in (" + assetsStates + ") " + checking, selectMode = selectMode });
                }
                else
                {
                    assetsStateSql = "Assets.assetsState=" + DFT.SQ + assetsState + DFT.SQ;
                    //model.GridUrl = Url.Action("GridData", new { filterString = "Assets.assetsState=" + DFT.SQ + assetsState + DFT.SQ + checking, selectMode = selectMode });
                }
            }
            else
            {
                assetsStateSql = " 1=1 ";
            }
            model.GridUrl = Url.Action("GridData", new { filterString = assetsStateSql + checking, selectMode = selectMode });
            model.GridLayout = GridLayout("", selectMode);
            return PartialView("AssetsSelect", model);
        }

        public ActionResult SingleSelect(string pageId, string assetsState, string selectMode)
        {
            SelectModel model = new SelectModel();
            model.PageId = pageId;
            model.FormId = "SelectForm";
            model.GridId = "list";
            model.IsMultiSelect = false;
            model.GridWidth = 773;
            if (DataConvert.ToString(assetsState) == "")
                assetsState = "A";
            model.GridUrl = Url.Action("GridData", new { filterString = "Assets.assetsState=" + DFT.SQ + assetsState + DFT.SQ, selectMode = selectMode });
            model.GridLayout = GridLayout("", selectMode);
            return PartialView("AssetsSelect", model);
        }

        [AppAuthorize]
        public ActionResult BatchDelete(string pageId, string formMode, string viewTitle)
        {
            ClearClientPageCache(Response);
            BatchDeleteModel model = new BatchDeleteModel();
            model.PageId = pageId;
            model.FormId = "SelectForm";
            model.GridFormId = "GridForm";
            model.GridId = "BatchGrid";
            model.IsMultiSelect = true;
            SetThisListModel(model);
            model.GridUrl = Url.Action("GridData", new { selectMode = "AssetsSelect" });
            model.GridLayout = GridLayout("", "AssetsSelect");
            model.GridWidth = 0;
            //return PartialView("AssetsSelect", model);
            return View(model);
        }


        [HttpPost]
        public ActionResult BatchDelete(string pageId, string formMode, string viewTitle, string gridString, string isCascadeDelete)
        {
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            AssetsRepository arep = new AssetsRepository();
            DataUpdate dbUpdate = new DataUpdate();
            try
            {
                dbUpdate.BeginTransaction();
                arep.DbUpdate = dbUpdate;
                arep.BatchDelete(sysUser, gridString, viewTitle, isCascadeDelete);
                dbUpdate.Commit();
                return Content("1");
            }
            catch (Exception)
            {
                dbUpdate.Rollback();
                return Content(AppMember.AppText["DeleteExistRefrence"]);
            }
            finally
            {
                dbUpdate.Close();
            }

        }
        public ActionResult ExportCard(string pageId, string primaryKey, string viewTitle)
        {
            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            curContext.Response.Charset = "gb2312";

            AssetsRepository arep = new AssetsRepository();
            DataSet ds = arep.GetAssetsCard(primaryKey);
            StringBuilder sbHtml = AssetsCardExcel.CreateCardExcel(ds);
            byte[] fileContents = Encoding.GetEncoding("gb2312").GetBytes(sbHtml.ToString());
            string fileName = "AssetsCard" + ".xls";
            if (ds.Tables.Contains("Assets"))
                fileName = DataConvert.ToString(ds.Tables["Assets"].Rows[0]["assetsBarcode"]) + ".xls";
            return File(fileContents, "application/ms-excel", fileName);
        }
    }
}

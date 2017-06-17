using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseCommon.Data;
using System.Data;
using BusinessCommon.Repositorys;
using WebCommon.Init;
using BaseCommon.Basic;
using BaseCommon.Models;
using BaseCommon.Repositorys;
using System.Web.Caching;
using System.Text;

namespace WebCommon.Common
{
    public abstract class MasterController : BaseController
    {
       
        private ILoadList ListRepository { get; set; }

        private IEntry EntryRepository { get; set; }

        protected bool NotNeedCache;

        protected double CacheExpiryMinute;

        protected CacheItemPriority CachePriority;

        protected string ExportFileName = "EXP";

        public MasterController()
        {
            IMasterFactory Repository = CreateRepository();
            ListRepository = Repository.CreateListRepository();
            EntryRepository = Repository.CreateEntryRepository();
        }

        protected abstract IMasterFactory CreateRepository();

        [HttpPost]
        public JsonResult GridData(string filterString)
        {
            ListCondition condition = new ListCondition();
            condition.SortField = DataConvert.ToString(Request.Form["sidx"]);
            condition.SortType = DataConvert.ToString(Request.Params["sord"]);
            condition.PageIndex = DataConvert.ToInt32(Request.Params["page"]);
            condition.PageRowNum = DataConvert.ToInt32(Request.Params["rows"]);
            condition.FilterString = filterString;
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            condition.SysUser = sysUser;
            //if (Request.Form.AllKeys.Contains("isQuery") && DataConvert.ToString(Request.Form["isQuery"]) == "true")
            //    condition.PageIndex = 1;
            if (Request.Form.AllKeys.Contains("formVar"))
                condition.ListModelString = DataConvert.ToString(Request.Form["formVar"]);
            int cnt = ListRepository.GetGridDataCount(condition);
            condition.TotalRowNum = cnt;
            DataTable dt = ListRepository.GetGridDataTable(condition);

            var rows = DataTable2Object.Data(dt, GridLayout().GridLayouts);
            double aa = (double)cnt / condition.PageRowNum;
            double pageCnt = Math.Ceiling(aa);
            var result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = new { page = condition.PageIndex, records = cnt, total = pageCnt, rows = rows };
            return result;
        }

        protected GridLayout GridLayout()
        {
            if (HttpContext.Cache[ControllerName + "GridLayout"] == null)
            {
                string areaName = RouteData.DataTokens["area"].ToString();
                CacheInit.CreateGridLayoutCache(HttpContext, areaName, ControllerName);
            }
            return (GridLayout)HttpContext.Cache[ControllerName + "GridLayout"];
        }


        protected virtual DataTable GetAuthorityGridButton(string pageId, string gridId)
        {
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            AuthorityRepository rep = new AuthorityRepository();
            DataTable dt = rep.GetUserGridButton(sysUser.UserId, pageId, gridId);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string urls = DataConvert.ToString(dt.Rows[i]["url"]);
                if (urls != "")
                {
                    string[] url = urls.Split('/');
                    dt.Rows[i]["url"] = Url.Action(url[3], url[2], new { Area = url[1] });
                }
            }
            return dt;
        }



        protected void SetParentListModel(string pageId, string viewTitle, ListViewModel model)
        {
            model.PageId = pageId;
            model.ViewTitle = viewTitle;
            model.GridId = "list";
            model.FormId = "ListForm";
            //model.GridHeight = DataConvert.ToInt32(Request.Cookies["HeighCookie"].Value) - 22;
            model.GridHeight = 320;
            model.GridLayout = GridLayout();
            model.AuthorityGridButton = GetAuthorityGridButton(pageId, model.GridId);
            model.GridUrl = Url.Action("GridData");
            model.GridDbClickUrl = Url.Action("Entry", new { formMode = "view" });
        }

        protected void SetParentEntryModel(string pageId, string formMode, string viewTitle, EntryViewModel model)
        {
            model.PageId = pageId;
            model.PageFlag = ControllerName;
            model.ViewTitle = viewTitle;
            model.FormMode = formMode;
            model.SaveUrl = Url.Action("Entry");
            model.ReturnUrl = Url.Action("List");
            model.FormId = "EntryForm";
            model.IsDisabled = true;
            model.CustomClick = false;
        }



        //protected void Update(IEntry rep, EntryViewModel model, string mode, string pkValue, string viewTitle)
        //{
        //    UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
        //    DataUpdate dbUpdate = new DataUpdate(); dbUpdate = new DataUpdate();
        //    try
        //    {
        //        dbUpdate.BeginTransaction();
        //        rep.DbUpdate = dbUpdate;
        //        rep.Update(model, sysUser, mode, pkValue, viewTitle);
        //        dbUpdate.Commit();
        //        CacheInit.RefreshCache(HttpContext, EntryRepository, ControllerName + "DropList", DateTime.Now.AddMinutes(CacheExpiryMinute), CachePriority, CacheCreateType.Immediately);
        //    }
        //    catch (Exception ex)
        //    {
        //        dbUpdate.Rollback();
        //        model.Message = ex.Message;
        //        model.HasError = "true";
        //    }
        //    finally
        //    {
        //        dbUpdate.Close();
        //    }
        //}

        protected virtual int Update(IEntry rep, EntryViewModel model, string pkValue)
        {
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            DataUpdate dbUpdate = new DataUpdate(); 
            try
            {
                if (CheckModelIsValid(model))
                {
                    if (CheckSelfBeforeSave(model))
                    {
                        dbUpdate.BeginTransaction();
                        rep.DbUpdate = dbUpdate;
                        rep.Update(model, sysUser, model.FormMode, pkValue, model.ViewTitle);
                        dbUpdate.Commit();
                        if (!NotNeedCache)
                            CacheInit.RefreshCache(HttpContext, EntryRepository, ControllerName + "DropList", DateTime.Now.AddMinutes(CacheExpiryMinute), CachePriority, CacheCreateType.Immediately);
                        return 1;
                    }
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                dbUpdate.Rollback();
                model.Message = ex.Message;
                model.HasError = "true";
                AppLog.WriteLog(sysUser.UserName, LogType.Error, "UpdateError", ex.Message);
                return 0;
            }
            finally
            {
                dbUpdate.Close();
            }
        }

        protected virtual bool CheckSelfBeforeSave(EntryViewModel model)
        {
            return true;
        }

        public virtual JsonResult DropList(string filterExpression, string pySearch)
        {
            ClearClientPageCache(Response);
            IEntry rep = EntryRepository;
            CacheInit.RefreshCache(HttpContext, rep, ControllerName + "DropList", DateTime.Now.AddMinutes(CacheExpiryMinute), CachePriority, CacheCreateType.IfNoThenGenerated);
            DataTable source = (DataTable)HttpContext.Cache[ControllerName + "DropList"];
            filterExpression = DFT.HandleExpress(filterExpression);
            List<DropListSource> dropList = rep.DropList(source, filterExpression);
            if (DataConvert.ToString(pySearch) != "")
            {
                List<DropListSource> filterDrop = new List<DropListSource>();
                foreach (var obj in dropList)
                {
                  string pyf=  PinYin.GetFirstPinyin(DataConvert.ToString(obj.Text)).ToUpper();
                  string[] pyfs = pyf.Split(',');
                  foreach (string py in pyfs)
                  {
                      if (py.StartsWith(pySearch.ToUpper()) && !filterDrop.Contains(obj))
                      {
                          filterDrop.Add(obj);
                      }
                  }
                 
                }
                return DropListJson(filterDrop);
            }
            return DropListJson(dropList);
        }

     

        public ActionResult AddFavorit(string tableName, string pkValue)
        {
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            DataUpdate dbUpdate = new DataUpdate();
            try
            {
                dbUpdate.BeginTransaction();
                CacheRepository drep = new CacheRepository();
                drep.DbUpdate = dbUpdate;
                drep.AddFavorit(pkValue, sysUser.UserId, tableName);
                dbUpdate.Commit();
            }
            catch (Exception ex)
            {
                dbUpdate.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                dbUpdate.Close();
            }
            return Content("0", "text/html");
        }

        public ActionResult ReplaceFavorit(string tableName, string pkValue)
        {
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            DataUpdate dbUpdate = new DataUpdate();
            try
            {
                dbUpdate.BeginTransaction();
                CacheRepository drep = new CacheRepository();
                drep.DbUpdate = dbUpdate;
                drep.ReplaceFavorit(pkValue, sysUser.UserId, tableName);
                dbUpdate.Commit();
            }
            catch (Exception ex)
            {
                dbUpdate.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                dbUpdate.Close();
            }
            return Content("0", "text/html");
        }

        public ActionResult Export(string formvar)
        {
            ListCondition condition = new ListCondition();
            condition.SortField = DataConvert.ToString(Request.Params["sidx"]);
            condition.SortType = DataConvert.ToString(Request.Params["sord"]);
            condition.PageIndex = DataConvert.ToInt32(Request.Params["page"]);
            condition.PageRowNum = DataConvert.ToInt32(Request.Params["rows"]);
            condition.ListModelString = formvar;
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            condition.SysUser = sysUser;
            DataTable dt = ListRepository.GetAllGridDataTable(condition);
            StringBuilder sbHtml = ExcelHelper.CreateExcel(dt, GridLayout());
            byte[] fileContents = Encoding.UTF8.GetBytes(sbHtml.ToString());
            return File(fileContents, "application/ms-excel", IdGenerator.GetMaxId(ExportFileName) + ".xls");
        }

    }
}
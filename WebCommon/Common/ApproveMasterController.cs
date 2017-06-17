using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Data;
using WebCommon.Init;
using BaseCommon.Basic;
using System.Web.Mvc;
using System.Data;
using BusinessCommon.Repositorys;
using BaseCommon.Models;
using BaseCommon.Repositorys;
using System.Text;

namespace WebCommon.Common
{
    public abstract class ApproveMasterController : BaseController
    {
        protected string ExportFileName = "EXP";
        protected ILoadList ListRepository { get; set; }

        private IApproveEntry EntryRepository { get; set; }

        public ApproveMasterController()
        {
            IApproveMasterFactory Repository = CreateRepository();
            ListRepository = Repository.CreateListRepository();
            EntryRepository = Repository.CreateApproveEntryRepository();
        }

        protected abstract IApproveMasterFactory CreateRepository();

        [HttpPost]
        public JsonResult GridData(string listMode, string filterString, string selectMode)
        {
            ApproveListCondition condition = new ApproveListCondition();
            condition.SortField = DataConvert.ToString(Request.Form["sidx"]);
            condition.SortType = DataConvert.ToString(Request.Params["sord"]);
            condition.PageIndex = DataConvert.ToInt32(Request.Params["page"]);
            condition.PageRowNum = DataConvert.ToInt32(Request.Params["rows"]);
            condition.ListMode = listMode;
            condition.SelectMode = selectMode;
            condition.FilterString = filterString;
            //if (Request.Form.AllKeys.Contains("isQuery") && DataConvert.ToString(Request.Form["isQuery"]) == "true")
            //    condition.PageIndex = 1;
            if (Request.Form.AllKeys.Contains("formVar"))
                condition.ListModelString = DataConvert.ToString(Request.Form["formVar"]);
            
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            condition.SysUser = sysUser;
            condition.Approver = sysUser.UserId;
            int cnt = ListRepository.GetGridDataCount(condition);
            condition.TotalRowNum = cnt;
            DataTable dt = ListRepository.GetGridDataTable(condition);
            var rows = DataTable2Object.Data(dt, GridLayout(listMode, selectMode).GridLayouts);
            double aa = (double)cnt / condition.PageRowNum;
            double pageCnt = Math.Ceiling(aa);
            var result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = new { page = condition.PageIndex, records = cnt, total = pageCnt, rows = rows };
            return result;
        }

        [HttpPost]
        public virtual JsonResult EntryGridData(string formVar, string formMode, string primaryKey)
        {
            Dictionary<string, object> paras = new Dictionary<string, object>();
            DataTable dt = EntryRepository.GetEntryGridDataTable(paras, formMode, primaryKey, formVar);
            var rows = DataTable2Object.Data(dt, EntryGridLayout(formMode).GridLayouts);
            var result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = new { page = 1, total = rows.Length, rows = rows };
            return result;
        }

        protected virtual GridLayout GridLayout(string listMode, string selectMode)
        {
            if (HttpContext.Cache[ControllerName + "GridLayout"] == null)
            {
                string areaName = RouteData.DataTokens["area"].ToString();
                CacheInit.CreateGridLayoutCache(HttpContext, areaName, ControllerName);
            }
            return (GridLayout)HttpContext.Cache[ControllerName + "GridLayout"];
        }

        protected virtual GridLayout EntryGridLayout(string formMode)
        {
            string cacheName = "";
            string layoutName = "";
            if (formMode == "approve" || formMode == "view")
            {
                layoutName = ControllerName + "Entry";
                cacheName = layoutName + "GridLayout" + "Disabled";
            }
            else
            {
                if (formMode == "actual")
                {
                    layoutName = ControllerName + "EntryActual";
                    cacheName = layoutName + "GridLayout";
                }
                else if (formMode == "actualview")
                {
                    layoutName = ControllerName + "EntryActualView";
                    cacheName = layoutName + "GridLayout" + "Disabled";
                }
                else
                {
                    layoutName = ControllerName + "Entry";
                    cacheName = layoutName + "GridLayout";
                }
            }
            if (HttpContext.Cache[cacheName] == null)
            {
                string areaName = RouteData.DataTokens["area"].ToString();
                CacheInit.CreateGridLayoutCache(HttpContext, areaName, layoutName, formMode, cacheName);
            }
            return (GridLayout)HttpContext.Cache[cacheName];
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



        protected GridLayout ApproveGridLayout()
        {
            if (HttpContext.Cache["ApproveGridLayout"] == null)
            {
                CacheInit.CreateGridLayoutCache(HttpContext, "Common", "Approve");
            }
            return (GridLayout)HttpContext.Cache["ApproveGridLayout"];
        }

        /// <summary>
        /// 保存审批信息，1:成功执行；2：成功执行并且是审批结束;3:同一节点已被同一角色下的其他人审批，不能再审批
        /// </summary>
        /// <param name="approveReturn"></param>
        /// <param name="tableName"></param>
        /// <param name="pkField"></param>
        /// <param name="pkValue"></param>
        /// <param name="approveMind"></param>
        /// <param name="approveLevel"></param>
        /// <param name="approveNode"></param>
        /// <param name="viewTitle"></param>
        /// <returns>1:成功执行；2：成功执行并且是审批结束;3:同一节点已被同一角色下的其他人审批，不能再审批</returns>
        protected int SaveApprove(IApproveEntry repApproveMaster, string approveReturn, string tableName, string pkField, string pkValue, string approveMind, string approveLevel, string approveNode, string viewTitle)
        {
            ApproveRepository rep = new ApproveRepository();
            rep.DbUpdate = repApproveMaster.DbUpdate;   
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            if (rep.CheckSameNodeOtherHasApprove(sysUser, tableName, pkField, pkValue, approveNode))
                return 3;
            if (DataConvert.ToString(approveReturn) == "true")
            {
                rep.EditData(sysUser, viewTitle, tableName, pkField, pkValue, "R", approveMind,approveNode);
            }
            else
            {
                bool isEndNode = false;
                repApproveMaster.InitNextApproveTask(tableName, approveNode, approveLevel, sysUser.UserId, pkField, pkValue, viewTitle, ref isEndNode);
                rep.EditData(sysUser, viewTitle, tableName, pkField, pkValue, "P", approveMind,approveNode,isEndNode);
                if (isEndNode)
                    return 2;
            }
            return 1;
        }

        protected void SetParentListModel(string pageId, string viewTitle, string listMode, string tableFlg, ApproveListViewModel model)
        {
            model.PageId = pageId;
            model.ViewTitle = viewTitle;
            model.GridId = "list";
            model.FormId = "ListForm";
            //model.GridHeight = DataConvert.ToInt32(Request.Cookies["HeighCookie"].Value) - 22;
            model.GridHeight = 320;
            model.GridLayout = GridLayout(listMode, "");
            model.AuthorityGridButton = GetAuthorityGridButton(pageId, model.GridId);
            if (DataConvert.ToString(listMode) != "")
            {
                model.ListMode = listMode;
                model.GridUrl = Url.Action("GridData", new { listMode = listMode });
                model.GridDbClickUrl = Url.Action("Entry", new { formMode = listMode + "view" });
            }
            else
            {
                model.GridUrl = Url.Action("GridData");
                model.GridDbClickUrl = Url.Action("Entry", new { formMode = "view" });
            }

            //if (listMode == "approve")
            //    model.GridTitle = AppMember.AppText["PreApproveList"].ToString();
            //else if (listMode == "reapply")
            //    model.GridTitle = AppMember.AppText["ReturnApproveList"].ToString();
            //else
            //    model.GridTitle = AppMember.AppText[tableFlg + "List"].ToString();
        }

        protected void SetParentEntryModel(string pageId, string primaryKey, string formMode, string viewTitle, ApproveEntryViewModel model)
        {
            model.PageId = pageId;
            model.ViewTitle = viewTitle;
            model.FormMode = formMode;
            model.FormId = "EntryForm";
            model.SaveUrl = Url.Action("Entry");
            model.IsDisabled = true;
            model.CustomClick = false;
            //if (formMode != "approve")
            //    model.CustomClick = true;
            //if (DataConvert.ToString(formMode).Contains("approve") ||DataConvert.ToString(formMode).Contains("reapply"))
            //    model.ReturnUrl = Url.Action("List", new { listMode = formMode });
            //else
            //    model.ReturnUrl = Url.Action("List");
            if (formMode.Contains("approve") || formMode.Contains("reapply"))
            {
                model.ApproveReturnUrl = Url.Action("Entry", new { approveReturn = "true" });
                model.ApproveGridLayout = ApproveGridLayout();
                model.ApprovePkValue = primaryKey;
                if (formMode == "approveinfo")
                {
                    model.ReturnUrl = Url.Action("List");
                }
                else
                {
                    model.ReturnUrl = Url.Action("List", new { listMode = formMode.Contains("approve") ? "approve" : "reapply" });
                    if (formMode.Contains("approve"))
                    {
                        ApproveRepository repApprove = new ApproveRepository();
                        UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
                        DataRow dr = repApprove.GetApproveData(model.ApproveTableName, primaryKey, sysUser.UserId);
                        model.ApproveNode = DataConvert.ToString(dr["approveNode"]);
                        model.ApproveLevel = DataConvert.ToString(dr["approveLevel"]);
                    }
                }
            }
            else
            {
                if (formMode.Contains("actual"))
                    model.ReturnUrl = Url.Action("List", new { listMode = "actual" });
                else
                    model.ReturnUrl = Url.Action("List");
            }
        }

        protected bool CheckModelIsValid(ApproveEntryViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.HasError = "false";
                return true;
            }
            else
            {
                model.HasError = "true";
                return false;
            }
        }

        //protected ActionResult DealApprove(IApproveEntry repApproveMaster, ApproveEntryViewModel model, string approveReturn)
        //{
        //    DataUpdate dbUpdate = new DataUpdate();
        //    try
        //    {
        //        dbUpdate.BeginTransaction();
        //        repApproveMaster.DbUpdate = dbUpdate;
        //        int ret = SaveApprove(repApproveMaster, approveReturn, model.ApproveTableName, model.ApprovePkField, model.ApprovePkValue, model.ApproveMind, model.ApproveLevel, model.ApproveNode, model.ViewTitle);
        //        if (ret == 2)
        //        {
        //            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
        //            repApproveMaster.DealEndApprove(model.ApprovePkValue, sysUser, model.ViewTitle);
        //        }
        //        dbUpdate.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        dbUpdate.Rollback();
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        dbUpdate.Close();
        //    }
        //    return RedirectToAction("List", new { pageId = model.PageId, viewTitle = model.ViewTitle, listMode = model.FormMode });
        //}


        //protected void Update(IApproveEntry rep, ApproveEntryViewModel model, string mode, string pkValue, string viewTitle)
        //{
        //    UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
        //    DataUpdate dbUpdate = new DataUpdate();
        //    try
        //    {
        //        dbUpdate.BeginTransaction();
        //        rep.DbUpdate = dbUpdate;
        //        rep.Update(model, sysUser, mode, pkValue, viewTitle);
        //        dbUpdate.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        dbUpdate.Rollback();
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        dbUpdate.Close();
        //    }
        //}

        protected int DealApprove(IApproveEntry repApproveMaster, ApproveEntryViewModel model, string approveReturn, string aa)
        {
            int ret = SaveApprove(repApproveMaster, approveReturn, model.ApproveTableName, model.ApprovePkField, model.ApprovePkValue, model.ApproveMind, model.ApproveLevel, model.ApproveNode, model.ViewTitle);
            if (ret == 2)
            {
                UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
                repApproveMaster.DealEndApprove(model.ApprovePkValue, sysUser, model.ViewTitle);
            }
            else if (ret == 3)
            {
                model.Message = AppMember.AppText["SameRoleHasApprove"];
                model.HasError = "true";
                return 0;
            }
            return 1;
        }

        protected int Update(IApproveEntry rep, ApproveEntryViewModel model, string pkValue, string approveReturn)
        {
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            DataUpdate dbUpdate = new DataUpdate();
            try
            {
                dbUpdate.BeginTransaction();
                rep.DbUpdate = dbUpdate;
                if (model.FormMode != "approve")
                {
                    if (CheckModelIsValid(model))
                        rep.Update(model, sysUser, model.FormMode, pkValue, model.ViewTitle);
                    else
                        return 0;
                }
                else
                {
                    ModelState.Clear();
                    int val= DealApprove(rep, model, approveReturn, "");
                    if (val == 0)
                        return 0;
                }
                dbUpdate.Commit();
                return 1;
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

        public ActionResult Export(string formvar)
        {
            ApproveListCondition condition = new ApproveListCondition();
            condition.SortField = DataConvert.ToString(Request.Params["sidx"]);
            condition.SortType = DataConvert.ToString(Request.Params["sord"]);
            condition.PageIndex = DataConvert.ToInt32(Request.Params["page"]);
            condition.PageRowNum = DataConvert.ToInt32(Request.Params["rows"]);
            condition.ListModelString = formvar;
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            condition.SysUser = sysUser;
            condition.Approver = sysUser.UserId;
            DataTable dt = ListRepository.GetAllGridDataTable(condition);
            StringBuilder sbHtml = ExcelHelper.CreateExcel(dt, GridLayout("", ""));
            byte[] fileContents = Encoding.UTF8.GetBytes(sbHtml.ToString());
            return File(fileContents, "application/ms-excel", IdGenerator.GetMaxId(ExportFileName) + ".xls");
        }

    }
}
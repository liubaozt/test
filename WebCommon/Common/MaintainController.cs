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

namespace WebCommon.Common
{
    public abstract class MaintainController : BaseController
    {
        protected void SetParentEntryModel(string pageId, string formMode, string viewTitle, EntryViewModel model)
        {
            model.PageId = pageId;
            model.ViewTitle = viewTitle;
            model.FormMode = formMode;
            model.SaveUrl = Url.Action("Entry");
            model.ReturnUrl = Url.Action("List");
            model.FormId = "EntryForm";
            model.IsDisabled = true ;
            model.CustomClick = false;
        }

        protected bool CheckModelIsValid(EntryViewModel model)
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

        protected int Update(IMaintain rep, EntryViewModel model, string viewTitle)
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCommon.Common;
using BaseCommon.Data;
using System.Data;
using BaseCommon.Basic;
using WebCommon.Init;
using BusinessCommon.Models.PasswordModifiy;
using System.Collections;
using BusinessCommon.Repositorys;
using WebCommon.HttpBase;

namespace WebApp.Areas.BusinessCommon.Controllers
{
    public class PasswordModifiyController : BaseController
    {
        public PasswordModifiyController()
        {
            ControllerName = "PasswordModifiy";
        }

        [AppAuthorize]
        public ActionResult Entry(string pageId, string viewTitle)
        {
            ClearClientPageCache(Response);
            EntryModel model = new EntryModel();
            model.PageId = pageId;
            model.ViewTitle = viewTitle;
            model.FormId = "EntryForm";
            model.SaveUrl = Url.Action("Entry");
            return View(model);
        }

        [AppAuthorize]
        [HttpPost]
        public ActionResult Entry(EntryModel model)
        {
            if (CheckModelIsValid(model))
            {
                UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
                DataUpdate dbUpdate = new DataUpdate();
                try
                {
                    dbUpdate.BeginTransaction();
                    PasswordModifiyRepository rep = new PasswordModifiyRepository();
                    rep.DbUpdate = dbUpdate;
                    rep.ModifiyPassword(model, sysUser.UserId);
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
            }
            return View(model);
        }

        protected bool CheckModelIsValid(EntryModel model)
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

        [HttpPost]
        public ActionResult CheckOldPassword(string oldPassword)
        {
            try
            {
                PasswordModifiyRepository rep = new PasswordModifiyRepository();
                if (rep.CheckOldPassword(oldPassword) == 1)
                    return Content("1", "text/html");
                else
                    return Content("0", "text/html");
            }
            catch (Exception)
            {
                return Content("0", "text/html");
            }
        }

        [HttpPost]
        public ActionResult CheckNewPassword(string newPassword, string newPassword2)
        {
            if (newPassword == newPassword2)
                return Content("1", "text/html");
            else
                return Content("0", "text/html");

        }

    }
}

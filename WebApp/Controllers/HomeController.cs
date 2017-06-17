using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebCommon.Common;
using WebApp.Models;
using BaseCommon.Basic;
using BusinessCommon.Repositorys;
using BaseCommon.Data;
using System.Data;
using System.IO;
using WebCommon.Init;
using BaseCommon.Repositorys;
using System.Text;
using System.Drawing;
using BusinessLogic.AssetsBusiness.Repositorys;
using System.Configuration;

namespace WebApp.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        //protected override void OnResultExecuting(ResultExecutingContext filterContext)
        //{
        //    base.OnResultExecuting(filterContext);
        //    //if (RouteData.DataTokens.Count > 0)
        //    //    AreaName = RouteData.DataTokens["area"].ToString();
        //    InitCssAndJs();

        //}

        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    base.OnException(filterContext);
        //    TempData["Exception"] = filterContext.Exception.Message;
        //}

        //private void InitCssAndJs()
        //{
        //    string layoutContentPath = "~/Content/css/";

        //    string[] layoutCss = new string[]{
        //        "redmond/jquery-ui-1.8.18.custom.css",
        //        "redmond/ui.jqgrid.css",
        //        "tree/zTreeStyle/zTreeStyle.css",
        //        //"button/Button.css",
        //        "uploadify/uploadify.css",
        //        "Site.css",
        //        "pagecss.css"
        //        };
        //    StringBuilder sb = new StringBuilder();
        //    foreach (var item in layoutCss)
        //    {
        //        sb.Append(@"<link type=""text/css"" rel=""stylesheet"" href=""");
        //        sb.Append(Url.Content(layoutContentPath) + item);
        //        sb.AppendLine(@"""/>");
        //    }

        //    TempData["CssBlock"] = sb.ToString();
        //    sb.Clear();
        //    string jsPath = "~/Scripts/used/";
        //    string[] jqScript = new string[]{
        //        "jquery-1.7.1.min.js",
        //        "jquery-ui-1.8.18.custom.min.js",
        //        "jquery.ui.datepicker-zh-CN.js",
        //        "jquery.layout.js",
        //        "grid.locale-cn.js",
        //        "jquery.jqGrid.min.js",
        //        "tree/jquery.ztree.core-3.0.js",
        //        "tree/jquery.ztree.excheck-3.0.js",
        //        "json2.js",
        //        //"btn.js",
        //        "jquery.uploadify.min.js",
        //        "app.customer.js",
        //    };
        //    foreach (var item in jqScript)
        //    {
        //        sb.Append(@"<script type=""text/javascript"" src=""");
        //        sb.Append(Url.Content(jsPath + item));
        //        sb.AppendLine(@" ""></script>");
        //    }
        //    TempData["ScriptBlock"] = sb.ToString();
        //}


        public ActionResult Index()
        {
            ClearClientPageCache(Response);
            //if (AppMember.AutoDepreciation == "true")
            //    AutoDepreciation.ExcuteAutoUpdate();
            //string autoBackup = ConfigurationManager.AppSettings["AutoBackup"].ToString();
            //if (autoBackup == "true")
            //    DataBaseBackupRepository.ExcuteAutoBackUp();
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext, true);
            ViewData["sysUser"] = sysUser;
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult NoAccess()
        {
            return View();
        }

        public ActionResult GetMachineInfo()
        {
            AppRegisterRepository rep = new AppRegisterRepository();
            ViewData["MachineInfo"] = rep.GetMachineInfo();
            return View();
        }


        public JsonResult Menu()
        {
            UserInfo sysUser = CacheInit.GetUserInfo(HttpContext);
            AuthorityRepository rep = new AuthorityRepository();
            DataTable dt = rep.GetUserMenu(sysUser.UserId);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string urls = DataConvert.ToString(dt.Rows[i]["url"]);
                if (urls != "")
                {
                    string[] url = urls.Split('/');
                    string urlmenu = Url.Action(url[3], url[2], new { Area = url[1] });
                    if (url.Length > 4)
                        urlmenu = urlmenu + url[4];
                    dt.Rows[i]["url"] = urlmenu;
                }
            }
            var rows = DataTable2Object.Data(dt);
            //var rows = new object[4];
            //rows[0] = new { id = 1, cell = new[] { "1", "周报", "", "0", "1", "10", "false", "false" } };
            //rows[1] = new { id = 2, cell = new[] { "2", "用户组", "/BusinessCommon/Group/List", "1", "2", "3", "true", "true" } };
            //rows[2] = new { id = 3, cell = new[] { "6", "系统", "", "0", "11", "18", "false", "false" } };
            //rows[3] = new { id = 4, cell = new[] { "7", "用户", "/BusinessCommon/User/List", "1", "12", "13", "true", "true" } };  
            var result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = new { page = 1, records = rows.Length, rows = rows, total = 1 };
            return result;


        }

        public ActionResult AppRegister()
        {

            return View();
        }

        [HttpPost]
        public ActionResult AppRegister(AppRegisterModel model)
        {
            AppRegisterRepository rep = new AppRegisterRepository();
            rep.AddRegister(model.RegisterNo, model.CompanyName);
            return View();
        }


        public ActionResult Login()
        {
            //if (SoftRegister.CheckValid())
            return View();
            //else
            //return RedirectToAction("AppRegister", "Home");
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            //HttpCookie heighCookie = new HttpCookie("HeighCookie");
            //heighCookie.Expires = DateTime.Now.AddYears(1);
            //heighCookie.Value = model.BodyHeight.ToString();
            //this.Response.Cookies.Add(heighCookie);
            //AppMember.BodyHeight = model.BodyHeight;
            string userName = model.UserNo;
            string password = model.UserPwd;
            string yzmcode = Request.Cookies["yzmcode"].Value;

            if (ValidateLogin(userName, password) == 0)
            {
                return View();
            }
            else if (yzmcode.ToLower() != model.TxtCheckCode.ToLower())
            {
                ModelState.AddModelError("TxtCheckCode", AppMember.AppText["CheckCodeError"]);
                return View();
            }
            else if (ValidateLogin(userName, password) == -1)
            {
                ViewData["Message"] = AppMember.AppText["LoginErr"];
                return View();
            }
            //FormsAuthenticationTicket authTicket = new
            //                FormsAuthenticationTicket(1, //version
            //                userName, // user name
            //                DateTime.Now,             //creation
            //                DateTime.Now.AddMinutes(30), //Expiration
            //                false, //Persistent
            //                userName); //since Classic logins don't have a "Friendly Name"

            //string encTicket = FormsAuthentication.Encrypt(authTicket);
            //this.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

            FormsAuthentication.SetAuthCookie(userName, false);
            FormsAuthentication.RedirectFromLoginPage(userName, false);

            SetBooksRepository rep = new SetBooksRepository();
            Session["CurSetBooks"] = rep.GetCurSetBooks(model.SetBooks);
            Session.Timeout = 30;

            string userNa = UserRepository.GetUserName(userName);
            AppLog.WriteLog(userNa, LogType.Info, "Login", string.Format(AppMember.AppText["LogLogin"]));

            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            //HttpContext.Session[this.User.Identity.Name]=null;
            return RedirectToAction("Login", "Home");
        }

        private int ValidateLogin(string userName, string password)
        {
            if (ModelState.IsValid)
            {
                UserRepository rep = new UserRepository();
                if (rep.ValidLogin(userName, password))
                    return 1;
                else
                    return -1;
            }
            else
            {
                return 0;
            }
        }

        public ActionResult Upload()
        {
            HttpPostedFileBase file = Request.Files["Filedata"];
            string folderpath = RouteData.Values["id"].ToString();
            //string path = Server.MapPath("~\\Content\\uploads\\" + folderpath + "\\");
            string path = Server.MapPath("~/Content/uploads/" + folderpath + "/");
            //string path = Url.Content("~/Content/uploads/"+ folderpath + "/");
            if (file != null)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileType = file.FileName.Substring(file.FileName.LastIndexOf('.'));
                string filename = "File" + IdGenerator.GetMaxId("File") + fileType;
                file.SaveAs(path + filename);
                return Content(filename, "text/html");
            }
            else
                return Content("0", "text/html");
        }

        public ActionResult OriginUpload()
        {
            HttpPostedFileBase file = Request.Files["Filedata"];
            string folderpath = RouteData.Values["id"].ToString();
            string path = Server.MapPath("~/Content/uploads/" + folderpath + "/");
            if (file != null)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                file.SaveAs(path + file.FileName);
                return Content(file.FileName, "text/html");
            }
            else
                return Content("0", "text/html");
        }


        public ActionResult CheckExisting()
        {
            string path = RouteData.Values["id"].ToString();
            //string filePath = Server.MapPath("~\\Content\\uploads\\" + path + "\\" + Request.Form["filename"].ToString());
            string filePath = Server.MapPath("~/Content/uploads/" + path + "/" + Request.Form["filename"].ToString());
            //string filePath = Url.Content("~/Content/uploads/" + path + "/" + Request.Form["filename"].ToString());
            if (!System.IO.File.Exists(filePath))
                return Content("0", "text/html");
            else
                return Content("1", "text/html");

        }

        public ActionResult DeleteImg(string fileCurrent, string fileDirectory)
        {
            string filename = Server.MapPath("~/Content/uploads/" + fileDirectory + "/" + fileCurrent);
            //string filename = Server.MapPath("~\\Content\\uploads\\" + fileDirectory + "\\" + fileCurrent);
            if (System.IO.File.Exists(filename))
            {

                FileInfo fi = new FileInfo(filename);

                if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)

                    fi.Attributes = FileAttributes.Normal;

                System.IO.File.Delete(filename);
                return Content("1", "text/html");
            }
            else
                return Content("0", "text/html");
        }

        [HttpPost]
        public JsonResult ApproveGridData(string pkValue, string tableName)
        {
            pkValue = DataConvert.ToString(pkValue);
            var rows = DataTable2Object.Data(ApproveGridDataTable(pkValue, tableName), ApproveGridLayout().GridLayouts);
            var result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = new { page = 1, total = rows.Length, rows = rows };
            return result;
        }

        protected DataTable ApproveGridDataTable(string pkValue, string tableName)
        {
            ApproveRepository rep = new ApproveRepository();
            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("refId", pkValue);
            paras.Add("tableName", tableName);
            return rep.GetGridDataTable(paras);
        }

        protected GridLayout ApproveGridLayout()
        {
            if (HttpContext.Cache["ApproveGridLayout"] == null)
            {
                CacheInit.CreateGridLayoutCache(HttpContext, "Common", "Approve");
            }
            return (GridLayout)HttpContext.Cache["ApproveGridLayout"];
        }

        private string RndNum()
        {
            int number;
            char code;
            string checkCode = String.Empty;

            System.Random random = new Random();

            for (int i = 0; i < 4; i++)
            {
                number = random.Next();
                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));
                if (code == '0' || code == 'O')
                    code = 'M';
                checkCode += code.ToString();
            }
            Response.Cookies.Add(new HttpCookie("yzmcode", checkCode));
            return checkCode;
        }

        private void CreateCheckCodeImage(string checkCode)
        {
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return;
            System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 12.5)), 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器 
                Random random = new Random();
                //清空图片背景色 
                g.Clear(Color.White);
                //画图片的背景噪音线 
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                Font font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);
                //画图片的前景噪音点 
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线 
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                Response.ClearContent();
                Response.ContentType = "image/Gif";
                Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        public ActionResult CheckCode()
        {
            string no = RndNum();
            CreateCheckCodeImage(no);
            return Content(no, "text/html");
        }
        public ActionResult CompareCheckCode(string txtCheckCode)
        {
            string yzmcode = Request.Cookies["yzmcode"].Value;
            if (txtCheckCode != yzmcode)
                return Content(AppMember.AppText["CheckCodeError"], "text/html");
            else
                return Content("1", "text/html");
        }
    }
}

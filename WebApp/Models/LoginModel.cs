using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BusinessCommon.Repositorys;

namespace WebApp.Models
{
    public class LoginModel
    {
        [AppRequiredAttribute()]
        [AppDisplayNameAttribute("UserNoA")]
        public string UserNo { get; set; }
        [AppRequiredAttribute()]
        [AppDisplayNameAttribute("UserPwd")]
        public string UserPwd { get; set; }

        public string SetBooks { get; set; }

        [AppRequiredAttribute()]
        public string TxtCheckCode { get; set; }

        public int BodyHeight { get; set; }
    }
}
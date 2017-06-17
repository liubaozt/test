using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BusinessCommon.Repositorys;

namespace WebApp.Models
{
    public class AppRegisterModel
    {
        [AppRequiredAttribute()]
        [AppDisplayNameAttribute("RegisterNo")]
        public string RegisterNo { get; set; }

        [AppDisplayNameAttribute("CompanyName")]
        public string CompanyName { get; set; }

        
    }
}
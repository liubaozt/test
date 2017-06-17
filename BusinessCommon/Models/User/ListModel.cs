using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BaseCommon.Models;

namespace BusinessCommon.Models.User
{
    public class ListModel : ListViewModel
    {

        [AppDisplayNameAttribute("UserNo")]
        public string UserNo { get; set; }

        [AppDisplayNameAttribute("UserName")]
        public string UserName { get; set; }

        [AppDisplayNameAttribute("GroupId")]
        public string GroupId { get; set; }

        [AppDisplayNameAttribute("DepartmentId")]
        public string DepartmentId { get; set; }
        public string DepartmentUrl { get; set; }
        public string DepartmentDialogUrl { get; set; }
        public string DepartmentAddFavoritUrl { get; set; }
        public string DepartmentReplaceFavoritUrl { get; set; }

    

    }
}
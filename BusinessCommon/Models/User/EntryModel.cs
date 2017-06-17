using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Models;
using BaseCommon.Data;

namespace BusinessCommon.Models.User
{
    public class EntryModel : EntryViewModel
    {

        [AppDisplayNameAttribute("UserId")]
        public string UserId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("UserNo")]
        public string UserNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("UserName")]
        public string UserName { get; set; }

       
        [AppDisplayNameAttribute("GroupId")]
        public string GroupId { get; set; }
        public string GroupIdDisplay { get; set; } 

        [AppRequiredAttribute()]
        [AppDisplayNameAttribute("DepartmentId")]
        public string DepartmentId { get; set; }
        public string DepartmentUrl { get; set; }
        public string DepartmentDialogUrl { get; set; }
        public string DepartmentAddFavoritUrl { get; set; }
        public string DepartmentReplaceFavoritUrl { get; set; }
        public string DepartmentIdDisplay { get; set; }

       
        [AppDisplayNameAttribute("PostId")]
        public string PostId { get; set; }

        [AppDisplayNameAttribute("Sex")]
        public string Sex { get; set; }

        [AppDisplayNameAttribute("Tel")]
        public string Tel { get; set; }

        [AppDisplayNameAttribute("Email")]
        public string Email { get; set; }

        [AppDisplayNameAttribute("Address")]
        public string Address { get; set; }

        [AppDisplayNameAttribute("IsSysUser")]
        public bool IsSysUser { get; set; }

        [AppDisplayNameAttribute("HasApproveAuthority")]
        public bool HasApproveAuthority { get; set; }

        [AppDisplayNameAttribute("AccessLevel")]
        public string AccessLevel { get; set; }

        public string IsFixed { get; set; }

    }
}
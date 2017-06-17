using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.ComponentModel.DataAnnotations;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;
using BaseCommon.Models;
using BusinessLogic.AssetsBusiness.Repositorys;

namespace BusinessLogic.AssetsBusiness.Models.AssetsPurchase
{
    public class PurchaseModel : ApproveEntryViewModel
    {

        [AppRequiredAttribute("AssetsName")]
        [AppDisplayNameAttribute("AssetsName")]
        public string AssetsName { get; set; }

        [AppRequiredAttribute("DepartmentId")]
        [AppDisplayNameAttribute("DepartmentId")]
        public string DepartmentId { get; set; }
        public string DepartmentUrl { get; set; }
        public string DepartmentDialogUrl { get; set; }
        public string DepartmentAddFavoritUrl { get; set; }
        public string DepartmentReplaceFavoritUrl { get; set; }

        [AppRequiredAttribute("StoreSiteId")]
        [AppDisplayNameAttribute("StoreSite")]
        public string StoreSiteId { get; set; }
        public string StoreSiteUrl { get; set; }
        public string StoreSiteDialogUrl { get; set; }
        public string StoreSiteAddFavoritUrl { get; set; }
        public string StoreSiteReplaceFavoritUrl { get; set; }

        [AppDisplayNameAttribute("UsePeople")]
        public string UsePeople { get; set; }

        public string UserSource { get; set; }

        [AppDisplayNameAttribute("Keeper")]
        public string Keeper { get; set; }

        [AppDisplayNameAttribute("AssetsValue")]
        public double? AssetsValue { get; set; }

        [AppDisplayNameAttribute("Remark")]
        public string Remark { get; set; }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;
using BaseCommon.Models;
using BusinessLogic.AssetsBusiness.Repositorys;

namespace BusinessLogic.AssetsBusiness.Models.AssetsManage
{
    public class ListModel : ApproveListViewModel
    {

        [AppDisplayNameAttribute("AssetsNo")]
        public string AssetsNo { get; set; }
        [AppDisplayNameAttribute("AssetsName")]
        public string AssetsName { get; set; }

        [AppDisplayNameAttribute("AssetsBarcode")]
        public string AssetsBarcode { get; set; }


        [AppDisplayNameAttribute("AssetsClassId")]
        public string AssetsClassId { get; set; }
        public string AssetsClassUrl { get; set; }
        public string AssetsClassDialogUrl { get; set; }
        public string AssetsClassAddFavoritUrl { get; set; }
        public string AssetsClassReplaceFavoritUrl { get; set; }

       
        [AppDisplayNameAttribute("DepartmentId")]
        public string DepartmentId { get; set; }
        public string DepartmentUrl { get; set; }
        public string DepartmentDialogUrl { get; set; }
        public string DepartmentAddFavoritUrl { get; set; }
        public string DepartmentReplaceFavoritUrl { get; set; }


        [AppDisplayNameAttribute("StoreSite")]
        public string StoreSiteId { get; set; }
        public string StoreSiteUrl { get; set; }
        public string StoreSiteDialogUrl { get; set; }
        public string StoreSiteAddFavoritUrl { get; set; }
        public string StoreSiteReplaceFavoritUrl { get; set; }

        [AppDisplayNameAttribute("ProjectManageId")]
        public string ProjectManageId { get; set; }

        [AppDisplayNameAttribute("CompanyId")]
        public string CompanyId { get; set; }

        [AppDisplayNameAttribute("UsePeople")]
        public string UsePeople { get; set; }

        public string UserSource { get; set; }

        [AppDisplayNameAttribute("Keeper")]
        public string Keeper { get; set; }

        [AppDisplayNameAttribute("AssetsState")]
        public string AssetsState { get; set; }

        [AppDisplayNameAttribute("PurchaseDateFrom")]
        public string PurchaseDateFrom { get; set; }

        [AppDisplayNameAttribute("PurchaseDateTo")]
        public string PurchaseDateTo { get; set; }

        [AppDisplayNameAttribute("Spec")]
        public string Spec { get; set; }


        [AppDisplayNameAttribute("Remark")]
        public string Remark { get; set; }

    }
}
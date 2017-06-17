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

namespace BusinessLogic.AssetsBusiness.Models.AssetsTransfer
{
    public class EntryModel : ApproveEntryViewModel
    {
       
        public string AssetsTransferId { get; set; }

        [AppRequiredAttribute("AssetsTransferNo")]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("AssetsTransferNo")]
        public string AssetsTransferNo { get; set; }

        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("AssetsTransferName")]
        public string AssetsTransferName { get; set; }

        [AppDisplayNameAttribute("DepartmentId")]
        public string DepartmentId { get; set; }
        public string DepartmentUrl { get; set; }
        public string DepartmentDialogUrl { get; set; }
        public string DepartmentAddFavoritUrl { get; set; }
        public string DepartmentReplaceFavoritUrl { get; set; }

        [AppDisplayNameAttribute("StoreSiteId")]
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

         [AppRequiredAttribute("TransferDate")]
        [AppDisplayNameAttribute("TransferDate")]
        public DateTime? TransferDate { get; set; }

        public string EntryGridId { get; set; }
        public GridLayout EntryGridLayout { get; set; }

        public string SelectUrl { get; set; }
        public string EntryGridString { get; set; }
 

    }
}
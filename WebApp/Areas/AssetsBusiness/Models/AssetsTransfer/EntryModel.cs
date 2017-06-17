using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;
using System.ComponentModel.DataAnnotations;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;

namespace WebApp.Areas.AssetsBusiness.Models.AssetsTransfer
{
    public class EntryModel : ApproveEntryViewModel
    {
        public EntryModel()
        {
            BaseRepository = new AssetsTransferRepository();
        }

        public string AssetsTransferId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("AssetsTransferNo")]
        public string AssetsTransferNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("AssetsTransferName")]
        public string AssetsTransferName { get; set; }

        [AppDisplayNameAttribute("DepartmentId")]
        public string DepartmentId { get; set; }
        public string DepartmentUrl { get; set; }
        public string DepartmentDialogUrl { get; set; }

        [AppDisplayNameAttribute("StoreSiteId")]
        public string StoreSiteId { get; set; }
        public string StoreSiteUrl { get; set; }
        public string StoreSiteDialogUrl { get; set; }

        [AppDisplayNameAttribute("UsePeople")]
        public string UsePeople { get; set; }

        [AppDisplayNameAttribute("Keeper")]
        public string Keeper { get; set; }

        [AppDisplayNameAttribute("TransferDate")]
        public DateTime? TransferDate { get; set; }

        public string EntryGridId { get; set; }
        public Dictionary<string, GridInfo> EntryGridLayout { get; set; }

        public string SelectUrl { get; set; }

        public AssetsTransferRepository Repository = new AssetsTransferRepository();

    }
}
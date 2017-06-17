using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using WebApp.BaseWeb.Common;
using System.ComponentModel.DataAnnotations;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;

namespace WebApp.Areas.AssetsBusiness.Models.AssetsManage
{
    public class EntryModel : ApproveEntryViewModel
    {
        public EntryModel()
        {
            BaseRepository = new AssetsRepository();
        }
        public string AssetsId { get; set; }

        [AppRequiredAttribute("AssetsNo")]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("AssetsNo")]
        public string AssetsNo { get; set; }

        [AppRequiredAttribute("AssetsName")]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("AssetsName")]
        public string AssetsName { get; set; }

        [AppRequiredAttribute("AssetsClassId")]
        [AppDisplayNameAttribute("AssetsClassId")]
        public string AssetsClassId { get; set; }
        public string AssetsClassUrl { get; set; }
        public string AssetsClassDialogUrl { get; set; }

        [AppRequiredAttribute("AssetsTypeId")]
        [AppDisplayNameAttribute("AssetsTypeId")]
        public string AssetsTypeId { get; set; }

        [AppRequiredAttribute("AssetsUsesId")]
        [AppDisplayNameAttribute("AssetsUsesId")]
        public string AssetsUsesId { get; set; }

        [AppRequiredAttribute("DepartmentId")]
        [AppDisplayNameAttribute("DepartmentId")]
        public string DepartmentId { get; set; }
        public string DepartmentUrl { get; set; }
        public string DepartmentDialogUrl { get; set; }

        [AppRequiredAttribute("StoreSiteId")]
        [AppDisplayNameAttribute("StoreSiteId")]
        public string StoreSiteId { get; set; }
        public string StoreSiteUrl { get; set; }
        public string StoreSiteDialogUrl { get; set; }

        [AppRequiredAttribute("DepreciationType")]
        [AppDisplayNameAttribute("DepreciationType")]
        public string DepreciationType { get; set; }

        [AppDisplayNameAttribute("EquityOwnerId")]
        public string EquityOwnerId { get; set; }

        [AppDisplayNameAttribute("PurchaseTypeId")]
        public string PurchaseTypeId { get; set; }

        [AppDisplayNameAttribute("SupplierId")]
        public string SupplierId { get; set; }

        [AppDisplayNameAttribute("UnitId")]
        public string UnitId { get; set; }

        [AppRequiredAttribute("AssetsBarcode")]
        [AppDisplayNameAttribute("AssetsBarcode")]
        public string AssetsBarcode { get; set; }

        [AppRequiredAttribute("AssetsValue")]
        [AppRegularExpression(AppMember.DoubleReg, "DoubleOnly")]
        [AppDisplayNameAttribute("AssetsValue")]
        public double? AssetsValue { get; set; }

        [AppRequiredAttribute("DurableYears")]
        [AppRegularExpression(AppMember.IntReg, "IntOnly")]
        [AppDisplayNameAttribute("DurableYears")]
        public int? DurableYears { get; set; }

        [AppRequiredAttribute("RemainRate")]
        [AppRegularExpression(AppMember.DoubleReg, "DoubleOnly")]
        [AppRangeAttribute(0, 1)]
        [AppDisplayNameAttribute("RemainRate")]
        public double? RemainRate { get; set; }

        [AppDisplayNameAttribute("PurchaseDate")]
        public DateTime? PurchaseDate { get; set; }

        [AppDisplayNameAttribute("PurchaseNo")]
        public string PurchaseNo { get; set; }

        [AppDisplayNameAttribute("InvoiceNo")]
        public string InvoiceNo { get; set; }

        [AppRequiredAttribute("UsePeople")]
        [AppDisplayNameAttribute("UsePeople")]
        public string UsePeople { get; set; }

        [AppRequiredAttribute("Keeper")]
        [AppDisplayNameAttribute("Keeper")]
        public string Keeper { get; set; }

        [AppDisplayNameAttribute("AssetsState")]
        public string AssetsState { get; set; }

        [AppRegularExpression(AppMember.IntReg, "IntOnly")]
        [AppDisplayNameAttribute("GuaranteeDays")]
        public int? GuaranteeDays { get; set; }

        [AppRegularExpression(AppMember.IntReg, "IntOnly")]
        [AppDisplayNameAttribute("MaintainDays")]
        public int? MaintainDays { get; set; }

        [AppDisplayNameAttribute("Remark")]
        public string Remark { get; set; }

        [AppDisplayNameAttribute("AssetsImg")]
        public string ImgFileDefault { get; set; }

        public string ImgSkipUrl { get; set; }

        public string ImgFileContainer { get; set; }

        public string ImgFileCurrent { get; set; }

        public AssetsRepository Repository = new AssetsRepository();
    }
}
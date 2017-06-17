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

namespace BusinessLogic.AssetsBusiness.Models.AssetsManage
{
    public class EntryModel : ApproveEntryViewModel
    {

        public string AssetsId { get; set; }

        [AppRequiredAttribute("AssetsNo")]
        //[AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("AssetsNo")]
        public string AssetsNo { get; set; }

        [AppRequiredAttribute("AssetsName")]
        //[AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("AssetsName")]
        public string AssetsName { get; set; }

        [AppRequiredAttribute("AssetsClassId")]
        [AppDisplayNameAttribute("AssetsClassId")]
        public string AssetsClassId { get; set; }
        public string AssetsClassUrl { get; set; }
        public string AssetsClassDialogUrl { get; set; }
        public string AssetsClassAddFavoritUrl { get; set; }
        public string AssetsClassReplaceFavoritUrl { get; set; }

        [AppDisplayNameAttribute("AssetsTypeId")]
        public string AssetsTypeId { get; set; }


        [AppDisplayNameAttribute("AssetsUsesId")]
        public string AssetsUsesId { get; set; }

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

        [AppDisplayNameAttribute("DepreciationType")]
        public string DepreciationType { get; set; }

        [AppDisplayNameAttribute("DepreciationRule")]
        public string DepreciationRule { get; set; }

        [AppDisplayNameAttribute("EquityOwnerId")]
        public string EquityOwnerId { get; set; }

        [AppDisplayNameAttribute("PurchaseTypeId")]
        public string PurchaseTypeId { get; set; }

        [AppDisplayNameAttribute("SupplierId")]
        public string SupplierId { get; set; }

        [AppDisplayNameAttribute("AcountUnit")]
        public string UnitId { get; set; }

        [AppDisplayNameAttribute("AssetsBarcode")]
        public string AssetsBarcode { get; set; }

        //[AppRequiredAttribute("AssetsValue")]
        //[AppRegularExpression(AppMember.DoubleReg, "DoubleOnly")]
        [AppDisplayNameAttribute("AssetsValue")]
        public double? AssetsValue { get; set; }

        //[AppRequiredAttribute("DurableYears")]
        //[AppRegularExpression(AppMember.IntReg, "IntOnly")]
        [AppDisplayNameAttribute("DurableYears")]
        public int? DurableYears { get; set; }

        //[AppRequiredAttribute("RemainRate")]
        //[AppRegularExpression(AppMember.DoubleReg, "DoubleOnly")]
        [AppRangeAttribute(0, 1)]
        [AppDisplayNameAttribute("RemainRate")]
        public double? RemainRate { get; set; }


        [AppDisplayNameAttribute("AssetsQty")]
        public int? AssetsQty { get; set; } 


        [AppRequiredAttribute("PurchaseDate")]
        [AppDisplayNameAttribute("StockingDate")]
        public DateTime? PurchaseDate { get; set; }



        [AppDisplayNameAttribute("PurchaseDate2")]
        public DateTime? PurchaseDate2 { get; set; }

        [AppDisplayNameAttribute("PurchaseNo")]
        public string PurchaseNo { get; set; }

        [AppDisplayNameAttribute("InvoiceNo")]
        public string InvoiceNo { get; set; }

        [AppDisplayNameAttribute("UsePeople")]
        public string UsePeople { get; set; }

        public string UserSource { get; set; }

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

        [AppDisplayNameAttribute("ProjectManageId")]
        public string ProjectManageId { get; set; }

        [AppDisplayNameAttribute("Remark")]
        public string Remark { get; set; }

        [AppDisplayNameAttribute("IsIdle")]
        public bool IsIdle { get; set; }

        [AppDisplayNameAttribute("Spec")]
        public string Spec { get; set; }

        [AppDisplayNameAttribute("AssetsImg")]
        public string ImgFileDefault { get; set; }

        public string ImgSkipUrl { get; set; }

        public string ImgFileContainer { get; set; }

        public string ImgFileCurrent { get; set; }


        [AppDisplayNameAttribute("CEANo")]
        public string CEANo { get; set; }

        [AppDisplayNameAttribute("TagMaterial")]
        public string TagMaterial { get; set; }

        [AppDisplayNameAttribute("ScrapDate")]
        public DateTime? ScrapDate { get; set; }

        [AppDisplayNameAttribute("SupplierName")]
        public string SupplierName { get; set; }

        [AppDisplayNameAttribute("MarkMK")]
        public string MarkMK { get; set; }

        [AppDisplayNameAttribute("MujuNo")]
        public string MujuNo { get; set; }

        [AppDisplayNameAttribute("ShengchanhuopinNo")]
        public string ShengchanhuopinNo { get; set; }

        [AppDisplayNameAttribute("Mujuxueshu")]
        public string Mujuxueshu { get; set; }

        [AppDisplayNameAttribute("Mujushouming")]
        public string Mujushouming { get; set; }

        [AppDisplayNameAttribute("Shejichannenng")]
        public string Shejichannenng { get; set; }

        [AppDisplayNameAttribute("Hadshejichannneng")]
        public string Hadshejichannneng { get; set; }

        [AppDisplayNameAttribute("ShiYongDiDian")]
        public string ShiYongDiDian { get; set; }

        [AppDisplayNameAttribute("PanDianHao")]
        public string PanDianHao { get; set; }

        public string IsStartDepreciation { get; set; }

        public string AssetsPurchaseDetailId { get; set; }

        public string AssetsPurchaseId { get; set; }  


    }
}
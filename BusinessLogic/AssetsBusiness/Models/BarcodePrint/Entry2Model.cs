using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseCommon.Basic;
using System.ComponentModel.DataAnnotations;
using BaseCommon.Data;
using BusinessLogic.AssetsBusiness;
using BusinessCommon.Repositorys;
using BaseCommon.Models;
using BusinessLogic.AssetsBusiness.Repositorys;
using System.Data;

namespace BusinessLogic.AssetsBusiness.Models.BarcodePrint
{
    public class Entry2Model : EntryViewModel
    {
        [AppDisplayNameAttribute("BarcodeStyleId")]
        public string BarcodeStyleId { get; set; } 

        public string BarcodeStyleName { get; set; }

        public string SelectUrl { get; set; }

         [AppDisplayNameAttribute("PrinterName")]
        public string PrinterName { get; set; } 

        public string DefaultAssetsId { get; set; }

        public string EntryGridId { get; set; }
        public GridLayout EntryGridLayout { get; set; }
        public string EntryGridString { get; set; }

        public string StoreSiteGridId { get; set; }
        public GridLayout StoreSiteGridLayout { get; set; } 

        public string XmlString { get; set; }
        public string LabelType { get; set; }
    

        public BarcodePrintRepository BarcodePrintRepository = new BarcodePrintRepository();

        public AssetsRepository AssetsRepository = new AssetsRepository();

        public BarcodeStyleRepository BarcodeStyleRepository = new BarcodeStyleRepository();

        public AssetsPrint CurrentAssets { get; set; }

        public List<AssetsPrint> AllAssets { get; set; }

        public string AssetsCount { get; set; }

        public DataTable BarcodeStyle { get; set; }


        //查询条件 start

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


        [AppDisplayNameAttribute("StoreSiteId")]
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

        //查询条件 end

    
    }
}
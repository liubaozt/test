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

namespace BusinessLogic.AssetsBusiness.Models.AssetsBorrow
{
    public class EntryModel : ApproveEntryViewModel
    {
       
        public string AssetsBorrowId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("AssetsBorrowNo")]
        public string AssetsBorrowNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("AssetsBorrowName")]
        public string AssetsBorrowName { get; set; }

        [AppDisplayNameAttribute("BorrowDepartmentId")]
        public string DepartmentId { get; set; }
        public string DepartmentUrl { get; set; }
        public string DepartmentDialogUrl { get; set; }
        public string DepartmentAddFavoritUrl { get; set; }
        public string DepartmentReplaceFavoritUrl { get; set; }


        [AppDisplayNameAttribute("BorrowPeople")]
        public string BorrowPeople { get; set; }


        [AppDisplayNameAttribute("BorrowDate")]
        public DateTime? BorrowDate { get; set; }

        public string EntryGridId { get; set; }
        public GridLayout EntryGridLayout { get; set; }

        public string SelectUrl { get; set; }

        public string EntryGridString { get; set; }
      

    }
}
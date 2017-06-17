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

namespace BusinessLogic.AssetsBusiness.Models.AssetsReturn
{
    public class EntryModel : ApproveEntryViewModel
    {
      
        public string AssetsReturnId { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(12)]
        [AppDisplayNameAttribute("AssetsReturnNo")]
        public string AssetsReturnNo { get; set; }

        [AppRequiredAttribute()]
        [AppStringLengthAttribute(25)]
        [AppDisplayNameAttribute("AssetsReturnName")]
        public string AssetsReturnName { get; set; }

 

        [AppDisplayNameAttribute("ReturnPeople")]
        public string ReturnPeople { get; set; }


        [AppDisplayNameAttribute("ReturnDate")]
        public DateTime? ReturnDate { get; set; }

        public string EntryGridId { get; set; }
        public GridLayout EntryGridLayout { get; set; }

        public string SelectUrl { get; set; }

        public string EntryGridString { get; set; }

    }
}
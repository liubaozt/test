using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaseCommon.Models
{
    public class BaseViewModel
    {
        public string PageId { get; set; }
        public string ViewTitle { get; set; }
        public string FormId { get; set; }
        public string HasError { get; set; }
        public string Message { get; set; }
    }
}
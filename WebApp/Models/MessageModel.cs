using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.BaseWeb;
using BaseCommon.Basic;
using BusinessCommon.AppMng;

namespace WebApp.Models
{
    public class MessageModel
    {
        public string PageId { get; set; }

        public string MessageContent { get; set; }

        public string MessageType { get; set; }

    }
}
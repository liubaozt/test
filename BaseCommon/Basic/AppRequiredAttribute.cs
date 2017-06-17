using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BaseCommon.Basic
{
    public class AppRequiredAttribute : RequiredAttribute
    {
        public AppRequiredAttribute(string fieldName)
        {
            ErrorMessage = "["+AppMember.AppText[fieldName] + AppMember.AppText["Require"]+"]  ";
        }
        public AppRequiredAttribute()
        {
            ErrorMessage = AppMember.AppText["Require"];
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BaseCommon.Basic
{
    public class AppRegularExpression : RegularExpressionAttribute
    {
        public AppRegularExpression(string reg, string errormsg)
            : base(reg)
        {
            ErrorMessage = AppMember.AppText[errormsg];
        }
    }
}

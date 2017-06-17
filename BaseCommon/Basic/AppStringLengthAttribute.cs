using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BaseCommon.Basic
{
    public class AppStringLengthAttribute : StringLengthAttribute
    {
        public AppStringLengthAttribute(int maximumLength)
            : base(maximumLength)
        {
            ErrorMessage = AppMember.AppText["Maxlength"] + maximumLength.ToString();
        }
    }
}

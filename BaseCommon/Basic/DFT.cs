using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseCommon.Data;

namespace BaseCommon.Basic
{
    public class DFT
    {
        public const string SQ = "SingleQuotes"; //单引号
        public const string GQ = "GreateQuotes"; //大于号
        public const string LQ = "LessQuotes";//小于号

        public static string HandleExpress(string expression)
        {
            expression = DataConvert.ToString(expression);
            expression = expression.Replace(DFT.SQ, "'");
            expression = expression.Replace(DFT.GQ, ">");
            expression = expression.Replace(DFT.LQ, "<");
            return expression;
        }
    }
}

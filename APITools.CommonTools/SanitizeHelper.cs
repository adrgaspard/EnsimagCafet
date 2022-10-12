using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITools.CommonTools
{
    public static class FormatHelper
    {
        public static string LowerAndTrim(this string value)
        {
            return value.Trim().ToLower();
        }

        public static string FirstUpperThenLowerAndTrim(this string value)
        {
            string result = value.Trim().ToLower();
            if (result.Length >= 1)
            {
                result = result[..1].ToUpper() + result[1..];
            }
            return result;
        }
    }
}

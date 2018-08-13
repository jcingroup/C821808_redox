using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace OutWeb.Expansion.StringProcess
{
    public static class StringHandler
    {
        /// <summary>
        ///  移除HTML TAG
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        /// <summary>
        /// 根據傳入的長度截斷字串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="lenSet"></param>
        /// <returns></returns>
        public static string SplitLengthString(this string str, int lenSet)
        {
            lenSet = lenSet - 3;
            int len = str.Length;
            if (len == 0)
                return str;
            if (len > lenSet)
                str = str.Substring(0, lenSet);
            return str + "...";
        }
    }
}
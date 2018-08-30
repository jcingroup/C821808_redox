using OutWeb.Enums;
using OutWeb.Modules.Manage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace OutWeb.Repositories
{
    public static class PublicMethodRepository
    {
        private static string m_customLanguageCode = string.Empty;

        private static Language m_language = Language.NotSet;

        public static SiteMode CurrentMode { get; set; } = SiteMode.NotSet;

        public static string SplitLengthString(this string str, int lenSet)
        {
            lenSet = lenSet - 3;
            int len = str.Length;
            if (len == 0)
                return str;
            if (len > lenSet)
                str = str.Substring(0, lenSet) + "...";
            return str;
        }

        public static ListDataMode CurrentListDataMode { get; set; } = ListDataMode.PageList;

        /// <summary>
        /// 根據傳入的數字擷取至該長度的字串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StripHTML(string input)
        {
            var toPlain = HtmlToPlainText(input);
            //input = Regex.Replace(input, "<.*?>", String.Empty);
            //input = Regex.Replace(input, "<[^>WW]*>", String.Empty);
            //toPlain = toPlain.Replace("&emsp;", "");
            //toPlain = toPlain.Replace("&nbsp;", "");

            return toPlain;
        }


        private static string HtmlToPlainText(string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }


        /// <summary>
        /// 律師名錄分類
        /// </summary>
        public static Dictionary<int, string> LawyerCateData
        {
            get
            {
                return
                    new Dictionary<int, string>()
                    {
                        { 1,"基本會員"},
                        { 2,"兼區會員"},
                    };
            }
        }

        /// <summary>
        /// 理監事職稱分類
        /// </summary>
        public static Dictionary<int, string> DirectorCateData
        {
            get
            {
                return
                    new Dictionary<int, string>()
                    {
                        { 1,"理事長"},
                        { 2,"副理事長"},
                        { 3,"常務理事"},
                        { 4,"理事"},
                        { 5,"常務監事"},
                        { 6,"監事"},
                        { 7,"秘書長"},
                        { 8,"財務長"},
                        { 9,"副秘書長"},
                    };
            }
        }


        /// <summary>
        /// 本會章程分類
        /// </summary>
        public static Dictionary<int, string> ReguCateData
        {
            get
            {
                return
                    new Dictionary<int, string>()
                    {
                        { 1,"總則"},
                        { 2,"任務"},
                        { 3,"入會及退會"},
                        { 4,"外國律師會員入會及退會"},
                        { 5,"權利及義務"},
                        { 6,"職員及選舉"},
                        { 7,"會議"},
                        { 8,"酬金"},
                        { 9,"經費及會計"},
                        { 10,"風紀"},
                        { 11,"附則"}
                    };
            }
        }

        public static string UrlMathPath
        {
            get
            {
                var context = HttpContext.Current;
                var scheme = context.Request.Url.Scheme;
                var host = context.Request.Url.Host;
                var port = context.Request.Url.Port;
                var applicationPath = context.Request.ApplicationPath;
                var url = string.Empty;
                if (!applicationPath.Equals("/"))
                    url = string.Format("{0}://{1}:{2}/{3}", scheme, host, port, applicationPath);
                else
                    url = string.Format("{0}://{1}:{2}", scheme, host, port);

                return url;
            }
        }

        public static int ListPageSize { get; set; } = 0;

        /// <summary>
        /// 根據傳入的陣列 依照順序建立目錄
        /// </summary>
        /// <param name="dirArray"></param>
        public static void CreateDir(string[] dirArray)
        {
            string serverRoorDir = HttpContext.Current.Server.MapPath("~");
            string dir = string.Empty;
            foreach (string d in dirArray)
            {
                dir = serverRoorDir += @"\" + d;
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
        }

        public static void FilterXss<T>(T obj)
        {
            // Get type.
            Type type = typeof(T);
            if (typeof(String).IsAssignableFrom(type))
            {
                HttpUtility.HtmlEncode((T)obj);
            }
            else if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                foreach (var str in obj as List<string>)
                    HttpUtility.HtmlEncode(str);
            }
            else
            {
                // Loop over properties.
                foreach (PropertyInfo propertyInfo in type.GetProperties())
                {
                    string name = propertyInfo.Name;
                    object value = propertyInfo.GetValue(obj, null);
                    if (value is string)
                    {
                        if (!propertyInfo.CanWrite)
                            continue;
                        //value = Sanitizer.GetSafeHtmlFragment(value.ToString());
                        value = HttpUtility.HtmlEncode(value.ToString());
                        propertyInfo.SetValue(obj, value);
                    }
                }
            }
        }

        public static void HtmlDecode<T>(T obj)
        {
            if (obj == null)
                return;
            // Get type.
            Type type = typeof(T);
            if (typeof(String).IsAssignableFrom(type))
            {
                HttpUtility.HtmlEncode((T)obj);
            }
            else if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                foreach (var str in obj as List<string>)
                    HttpUtility.HtmlDecode(str);
            }
            else
            {
                // Loop over properties.
                foreach (PropertyInfo propertyInfo in type.GetProperties())
                {
                    string name = propertyInfo.Name;
                    object value = propertyInfo.GetValue(obj, null);
                    if (value is string)
                    {
                        if (!propertyInfo.CanWrite)
                            continue;
                        value = HttpUtility.HtmlDecode(value.ToString());
                        propertyInfo.SetValue(obj, value);
                    }
                }
            }
        }

        public static FilterDateModel GetFilterDateBeginAndEnd(object date)
        {
            DateTime dt = new DateTime();
            if (date is string)
                dt = Convert.ToDateTime(date);
            else
                dt = (DateTime)date;
            DateTime startDate = new DateTime(dt.Year, dt.Month, dt.Day, 00, 00, 00);
            DateTime endDate = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
            FilterDateModel model = new FilterDateModel()
            {
                StartDate = startDate,
                EndDate = endDate
            };
            return model;
        }

        public class FilterDateModel
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }

        /// <summary>
        /// 使用者自訂語系
        /// </summary>
        public static string CustomLanguageCode { get { return m_customLanguageCode; } set { m_customLanguageCode = value; } }

        /// <summary>
        /// 取得瀏覽器使用語言清單
        /// </summary>
        public static string[] SupportLanguage
        {
            get
            {
                if (ConfigurationManager.AppSettings["SupportLanguage"] != null)
                {
                    string[] aSupportLang = ConfigurationManager.AppSettings["SupportLanguage"].Split(',');
                    List<string> lstSupportLang = new List<string>();
                    foreach (string langCode in aSupportLang)
                        lstSupportLang.Add(langCode.Trim());
                    return lstSupportLang.ToArray();
                }

                if (ConfigurationManager.AppSettings["DefaultLanguage"] != null)
                    return new string[] { ConfigurationManager.AppSettings["DefaultLanguage"].Trim() };
                return new string[] { "zh-TW" };
            }
        }

        /// <summary>
        /// 取得目前瀏覽器使用語系
        /// </summary>
        public static string CurrentLanguageCode
        {
            get
            {
                string sCurrLang = null;
                if (string.IsNullOrEmpty(m_customLanguageCode))
                {
                    //// 使用瀏覽器語系
                    //sCurrLang = HttpContext.Current.Request.UserLanguages[0].Split(',')[0];

                    // 使用瀏覽器語系
                    if ((String.IsNullOrEmpty(sCurrLang)) && (HttpContext.Current.Request.UserLanguages.Length != 0))
                        sCurrLang = HttpContext.Current.Request.UserLanguages[0].Split(',')[0];

                    // 使用系統語系
                    if (String.IsNullOrEmpty(sCurrLang))
                        sCurrLang = System.Globalization.CultureInfo.CurrentCulture.CompareInfo.Name;

                    // 本系統是否支援目前的語系
                    if (SupportLanguage.Where(g => g.Equals(sCurrLang)).FirstOrDefault() == null)
                        sCurrLang = null;

                    // 不支援目前語系則取預設語系
                    if ((String.IsNullOrEmpty(sCurrLang)) && (ConfigurationManager.AppSettings["DefaultLanguage"] != null))
                        sCurrLang = ConfigurationManager.AppSettings["DefaultLanguage"].Trim();
                    return sCurrLang;
                }
                else
                    sCurrLang = m_customLanguageCode;
                return sCurrLang;
            }
        }

        /// <summary>
        /// 取得目前使用語系列舉
        /// </summary>
        /// <returns></returns>
        public static Language CurrentLanguageEnum
        {
            get
            {
                return m_language;
            }
            set
            {
                m_language = value;
            }
        }

        public static string GetCode(this Language lang)
        {
            return GetEnumDescription<Language>(lang);
        }

        /// <summary>
        ///將DateTime轉換為10碼字串 (ex. 2017-05-05)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ConvertDateTimeTo10CodeString(this DateTime dt)
        {
            return string.Format("{0:yyyy\\/MM\\/dd}", dt);
        }

        /// <summary>
        /// 取得WebConfig的AppSetting
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        #region 取列舉名稱

        /// <summary>
        /// 取得語系的列舉
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Language GetLanguageEnumByCode(string code)
        {
            foreach (var v in Enum.GetValues(typeof(Language)))
            {
                Language en = (Language)v;
                string thisCode = GetEnumDescriptValue<Language>(en);
                if (string.IsNullOrEmpty(thisCode))
                    continue;
                var splitStr = thisCode.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries);
                if (splitStr.Length > 1)
                {
                    if (!thisCode.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries)[1].Equals(code))
                        continue;
                    else
                    {
                        m_language = en;
                        return en;
                    }
                }
                else
                    if (!thisCode.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries)[0].Equals(code))
                    continue;
                else
                {
                    m_language = en;
                    return en;
                }
            }
            throw new Exception("get language enum have some error.");
        }

        /// <summary>
        /// 依照列舉值取得列舉
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="code"></param>
        /// <returns></returns>
        public static TEnum GetEnumByValue<TEnum>(string value)
        {
            foreach (var v in Enum.GetValues(typeof(TEnum)))
            {
                TEnum en = (TEnum)v;
                var enStr = en.ToString();
                if (!enStr.Equals(value))
                    continue;
                else
                    return en;
            }
            throw new Exception("get enum have some error.");
        }

        #endregion 取列舉名稱

        #region 取列舉自訂描述

        /// <summary>
        /// 取列舉自訂描述
        /// </summary>
        /// <typeparam name="Tenum"></typeparam>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GetEnumDescription<Tenum>(Tenum en)
        {
            string status = en.GetEnumDescriptValue();
            return status.Split(new string[] { "$$" }, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        private static string GetEnumDescriptValue<TEnum>(this TEnum en)
        {
            DescriptionAttribute[] enumCodeAttrs = GetEnumCustomAttributes<TEnum, DescriptionAttribute>(en);
            if (enumCodeAttrs.Length == 0)
                return string.Empty;
            return enumCodeAttrs[0].Description;
        }

        private static TAttribute[] GetEnumCustomAttributes<TEnum, TAttribute>(TEnum en)
        {
            System.Reflection.MemberInfo[] enumMembers = typeof(TEnum).GetMember(en.ToString());
            if (enumMembers.Length == 0)
                throw new ArgumentException("Type [" + typeof(TEnum).Name + "] is not contents member: " + en.ToString());
            return enumMembers[0].GetCustomAttributes(typeof(TAttribute), false) as TAttribute[];
        }

        #endregion 取列舉自訂描述
    }
}
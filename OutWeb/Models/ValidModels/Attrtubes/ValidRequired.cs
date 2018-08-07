using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OutWeb.Models.ValidModels.Attrtubes
{
    /// <summary>
    /// 驗證是否必填
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ValidRequiredAttribute : Attribute, IModelValidate
    {
        public ValidRequiredAttribute() { }

        public string Message
        {
            get
            {
                return "欄位為必填.";
            }
        }

        public bool Valid(object value)
        {
            if (value == null)
                return false;
            string v = value.ToString();
            if (string.IsNullOrEmpty(v))
                return false;
            return true;
        }
    }
}
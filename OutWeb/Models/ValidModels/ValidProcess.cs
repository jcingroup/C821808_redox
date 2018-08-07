using System;
using System.Reflection;

namespace OutWeb.Models.ValidModels
{
    public static class ValidProcess
    {
        /// <summary>
        /// 依Info所設定的ModelValidate屬性進行資料驗證
        /// </summary>
        /// <param name="model"></param>
        public static void ValidModel(object model)
        {
            PropertyInfo[] properties = model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo propertyInfo in properties)
            {
                object[] attrs = propertyInfo.GetCustomAttributes(typeof(IModelValidate), false);
                foreach (object vdAttr in attrs)
                {
                    var modelValid = vdAttr as IModelValidate;
                    object propVal = propertyInfo.GetValue(model);
                    if (!modelValid.Valid(propVal))
                        throw new Exception(propertyInfo.Name + " " + modelValid.Message);
                }
            }
        }
    }
}
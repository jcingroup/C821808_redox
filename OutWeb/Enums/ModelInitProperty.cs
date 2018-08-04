using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OutWeb.Enums
{
    public enum ModelInitProperty
    {
        /// <summary>
        /// 初始值
        /// </summary>
        NotSet = -1,
        /// <summary>
        /// 表示此Model為User直接New的Function
        /// </summary>
        New = 0,
        /// <summary>
        /// 表示此Model為從資料庫呼叫後帶出的內容
        /// </summary>
        FromDB = 1
    }
}
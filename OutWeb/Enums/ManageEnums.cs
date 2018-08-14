using System.ComponentModel;

namespace OutWeb.Enums
{
    /// <summary>
    /// 使用者角色列舉
    /// </summary>
    public enum UserRoleEnum
    {
        [Description("未設定")]
        NotSet,

        [Description("使用者")]
        ADMIN,

        [Description("超級管理者")]
        SUPERADMIN,

        [Description("管理者")]
        USER,
    }

    /// <summary>
    /// 列表查詢方法列舉
    /// </summary>
    public enum ListMethodType
    {
        [Description("未設定")]
        NotSet,
        [Description("最新消息")]
        NEWS,
        [Description("產品管理")]
        PRODUCT,
      

    }

    /// <summary>
    /// 分頁每頁數量列舉
    /// </summary>
    public enum PageSizeConfig
    {
        SIZE02 = 2,

        /// <summary>
        /// 列表資料預設筆數3
        /// </summary>
        SIZE03 = 3,

        /// <summary>
        /// 列表資料預設筆數5
        /// </summary>
        SIZE05 = 5,

        /// <summary>
        /// 列表資料預設筆數6
        /// </summary>
        SIZE06 = 6,
        SIZE09 = 9,

        /// <summary>
        /// 列表資料預設筆數10
        /// </summary>
        SIZE10 = 10,
        /// <summary>
        /// 列表資料預設筆數30
        /// </summary>
        SIZE30 = 30,
    }

    public enum SiteMode
    {
        NotSet = 0, FronEnd = 1, Back = 2,Home=3
    }


    /// <summary>
    /// 語系列舉
    /// </summary>
    public enum Language
    {
        [Description("")]
        NotSet,

        /// <summary>
        /// English (United States)
        /// </summary>
        [Description("en-US")]
        enUS,

        /// <summary>
        /// Taiwan
        /// </summary>
        [Description("zh-TW")]
        zhTW,

        /// <summary>
        /// Chinese (PRC)
        /// </summary>
        [Description("zh-CN")]
        zhCN
    }


}
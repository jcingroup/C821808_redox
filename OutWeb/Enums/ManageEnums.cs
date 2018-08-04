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
        [Description("活動花絮")]
        ACTIVITY,
        [Description("活動花絮分類")]
        ACTIVITYCATE,
        [Description("本會章程")]
        REGU,
        [Description("理監事介紹")]
        DIRECTOR,
        [Description("委員會介紹")]
        COMMITTEE,
        [Description("會員服務")]
        CLUB,
        [Description("會員服務辦法")]
        WELFARE,
        [Description("表單下載")]
        DOWNLOAD,
        [Description("桃律通訊")]
        PUBLISH,
        [Description("律師名錄")]
        LAWYER,


        //=================
        [Description("線上課程")]
        COURSE,

        [Description("分類管理")]
        TYPEMANAGE,

        [Description("問卷調查")]
        QUESTIONNAIRES,

        [Description("研討會")]
        TRAIN,
        [Description("研討會報名")]
        TRAINAPPLY,
        [Description("三竹簡訊")]
        SMS,
        [Description("HiNetFax傳真")]
        FAX,
        [Description("EMAIL")]
        EMAIL,
        [Description("出本品分類")]
        BOOKKIND,
        [Description("外部連結")]
        LINK,

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
        NotSet = 0, FronEnd = 1, Back = 2
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
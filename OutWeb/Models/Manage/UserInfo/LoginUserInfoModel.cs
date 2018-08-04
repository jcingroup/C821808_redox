using OutWeb.Enums;
using System;

namespace OutWeb.Models.UserInfo
{
    [Serializable]
    public class LoginUserInfoModel
    {
        /// <summary>
        /// 索引值
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 使用者帳號
        /// </summary>
        public string UserAccount{ get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string UserName { get; set; }

  
        /// <summary>
        /// 停權
        /// </summary>
       public bool IsDisabled { get; set; }

        /// <summary>
        /// GUID
        /// </summary>
        public DateTime CreateDate { get; set; }

        public string GUID { get; internal set; }

        /// <summary>
        /// 角色
        /// </summary>
        public UserRoleEnum Role { get; set; }

    }
}
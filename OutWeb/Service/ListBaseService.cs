using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace OutWeb.Service
{
    public abstract class ListBaseModule : IDisposable
    {
        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="agencyNo"></param>
        internal ListBaseModule()
        { }

        /// <summary>
        /// 列表的抽象基底方法
        /// </summary>
        /// <param name="model"></param>
        public abstract object DoGetList(object model);

        /// <summary>
        /// 明細的抽象基底方法
        /// </summary>
        /// <param name="ID"></param>
        public abstract int DoSaveData(FormCollection form, int? ID = null, List<HttpPostedFileBase> files = null);

        /// <summary>
        /// 明細的抽象基底方法
        /// </summary>
        /// <param name="ID"></param>
        public abstract object DoGetDetailsByID(int ID);

        /// <summary>
        /// 刪除的抽象基底方法
        /// </summary>
        /// <param name="ID"></param>
        public abstract void DoDeleteByID(int ID);

        public virtual void Dispose()
        {
            this.Dispose();
        }
    }
}
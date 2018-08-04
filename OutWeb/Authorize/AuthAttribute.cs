using OutWeb.Provider;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OutWeb.Authorize
{
    public class AuthAttribute : AuthorizeAttribute
    {
        public string ControllerID { get; internal set; }

        public string ActionID { get; internal set; }

        /// <summary>
        /// 實作權限存取規則
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            this.ControllerID = filterContext.RouteData.Values["controller"].ToString();
            this.ActionID = filterContext.RouteData.Values["action"].ToString();

            #region 防守條件

            //驗證順序 OnAuthorization -> AuthorizeCore(false) -> HandleUnauthorizedRequest
            //驗證帳號
            base.OnAuthorization(filterContext);

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                CheckPermission(filterContext);
                return;
            }

            #endregion 防守條件
        }

        /// <summary>
        /// 檢查是否已通過驗證
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (UserProvider.Instance.User == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// AuthorizeCore返回false後 ,filterContext.Result返回HandleUnauthorizedRequest走此function 走此function
        /// </summary>
        /// <param name="filterContext"></param>
        private void CheckPermission(AuthorizationContext filterContext)
        {
            if (UserProvider.Instance.User == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    var urlHelper = new UrlHelper(filterContext.RequestContext);
                    filterContext.HttpContext.Response.StatusCode = 403;
                    filterContext.HttpContext.Response.End();
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        action = "SignInFail",
                        controller = "SignIn"
                    }));
                }
            }
        }
    }
}
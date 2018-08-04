using OutWeb.Enums;
using OutWeb.Repositories;

namespace OutWeb.Inc
{
    public abstract class WebUserController : System.Web.Mvc.Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            ViewBag.IsFirstPage = false; //預設為false
            PublicMethodRepository.CurrentMode = SiteMode.Back;
            PublicMethodRepository.ListPageSize = (int)PageSizeConfig.SIZE10;

        }

    }
}
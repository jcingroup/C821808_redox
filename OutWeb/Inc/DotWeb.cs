namespace OutWeb.Inc
{
    public abstract class WebUserController : System.Web.Mvc.Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            ViewBag.IsFirstPage = false; //預設為false
        }

    }
}
using OutWeb.Inc;
using System.Web.Mvc;

namespace OutWeb.Controllers
{
    public class NewsController : WebUserController
    {
        public ActionResult Index()
        {
            return View("List");
        }
        public ActionResult List()
        {
            return View();
        }
        public ActionResult Content()
        {
            return View();
        }
    }
}
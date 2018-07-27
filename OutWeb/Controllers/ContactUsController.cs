using OutWeb.Inc;
using System.Web.Mvc;

namespace OutWeb.Controllers
{
    public class ContactUsController : WebUserController
    {
        public ActionResult Index()
        {
            return View("ContactUs");
        }

        public ActionResult ContactUs()
        {
            return View();
        }
    }
}
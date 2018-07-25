using OutWeb.Inc;
using System.Web.Mvc;

namespace OutWeb.Controllers
{
    public class HomeController : WebUserController
    {
        public ActionResult Index()
        {
            ViewBag.IsFirstPage = true; //是否為首頁，請在首頁的Action此值設為True
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
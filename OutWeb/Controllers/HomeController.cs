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

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult Service()
        {
            return View();
        }
    }
}
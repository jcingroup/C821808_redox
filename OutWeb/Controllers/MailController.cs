using OutWeb.Inc;
using System.Web.Mvc;

namespace OutWeb.Controllers
{
    public class MailController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.IsFirstPage = false;
            return View("MsgMail");
        }

        public ActionResult MsgMail(FormCollection form)
        {
            return View(form);
        }
    }
}
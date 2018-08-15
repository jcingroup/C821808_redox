using OutWeb.Inc;
using System.Web.Mvc;

namespace OutWeb.Controllers
{
    public class MailController : WebUserController
    {
        public ActionResult Index()
        {
            return View("MsgMail");
        }

        public ActionResult MsgMail()
        {
            return View();
        }
    }
}
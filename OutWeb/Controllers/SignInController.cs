using OutWeb.Inc;
using System.Web.Mvc;

namespace OutWeb.Controllers
{
    public class SignInController : WebUserController
    {
        // GET: _SysAdm
        public ActionResult Index()
        {
            ViewBag.IsFirstPage = true;
            return View("Login");
        }
    }
}
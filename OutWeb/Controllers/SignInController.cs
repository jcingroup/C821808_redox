using OutWeb.Inc;
using System.Web.Mvc;

namespace OutWeb.Controllers
{
    public class SignInController : WebUserController
    {
        // GET: Sign In 後台登入
        public ActionResult Index()
        {
            return View("Login");
        }
    }
}
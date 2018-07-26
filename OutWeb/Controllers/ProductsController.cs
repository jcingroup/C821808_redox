using OutWeb.Inc;
using System.Web.Mvc;

namespace OutWeb.Controllers
{
    public class ProductsController : WebUserController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
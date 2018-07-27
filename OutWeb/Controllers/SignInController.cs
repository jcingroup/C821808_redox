using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OutWeb.Controllers
{
    public class SignInController : Controller
    {
        // GET: _SysAdm
        public ActionResult Index()
        {
            return View("Login");
        }
    }
}
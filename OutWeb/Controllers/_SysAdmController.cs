﻿using System.Web.Mvc;

namespace OutWeb.Controllers
{
    public class _SysAdmController : Controller
    {
        // GET: _SysAdm
        public ActionResult Index()
        {
            return View("~/SignIn");
        }

        public ActionResult NewsList()
        {
            return View();
        }

        public ActionResult NewsEdit()
        {
            return View();
        }

        public ActionResult ProductsList()
        {
            return View();
        }

        public ActionResult ProductsEdit()
        {
            return View();
        }

        public ActionResult FAQList()
        {
            return View();
        }

        public ActionResult FAQEdit()
        {
            return View();
        }

        public ActionResult ChangePW()
        {
            return View();
        }
    }
}
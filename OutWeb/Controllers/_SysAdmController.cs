using OutWeb.ActionFilter;
using OutWeb.Authorize;
using OutWeb.Inc;
using OutWeb.Modules.Manage;
using System;
using System.Web.Mvc;

namespace OutWeb.Controllers
{
    [Auth]
    [ErrorHandler]
    [CheckFolder]
    public class _SysAdmController : WebUserController
    {
        // GET: _SysAdm
        public ActionResult Index()
        {
            return RedirectToAction("NewsList");
        }

        public ActionResult NewsList()
        {
            return View();
        }

        public ActionResult NewsEdit()
        {
            return View();
        }

        public ActionResult ProductsInfo()
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

        #region 修改密碼

        [HttpGet]
        public ActionResult ChangePW()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePW(FormCollection form)
        {
            SignInModule signInModule = new SignInModule();
            try
            {
                signInModule.ChangePassword(form);
                ViewBag.Message = "success";
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            finally
            {
                signInModule.Dispose();
            }
            return View();
        }

        #endregion 修改密碼
    }
}
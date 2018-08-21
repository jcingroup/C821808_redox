using OutWeb.ActionFilter;
using OutWeb.Authorize;
using OutWeb.Enums;
using OutWeb.Inc;
using OutWeb.Models.Manage;
using OutWeb.Models.Manage.ManageNewsModels;
using OutWeb.Models.Manage.ManageProductModels;
using OutWeb.Module.Manage;
using OutWeb.Modules.Manage;
using OutWeb.Repositories;
using OutWeb.Service;
using REDOXEntities.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OutWeb.Controllers
{
    [Auth]
    [ErrorHandler]
    [CheckFolder]
    public class _SysAdmController : WebUserController
    {
        public _SysAdmController()
        {
            PublicMethodRepository.CurrentMode = SiteMode.Back;
        }



        public ActionResult TestCk()
        {
            return RedirectToAction("NewsList");
        }
        // GET: _SysAdm
        public ActionResult Index()
        {
            return RedirectToAction("NewsList");
        }

        #region NEWS

        public ActionResult NewsList(int? page, string qry, string sort, string fSt, string pSdate, string pEdate)
        {
            ListViewBase model = new ListViewBase();
            model.Filter.CurrentPage = page ?? 1;
            model.Filter.QueryString = qry ?? string.Empty;
            model.Filter.SortColumn = sort ?? string.Empty;
            model.Filter.DisplayForFrontEnd = fSt ?? string.Empty;
            model.Filter.PublishStartDate = pSdate;
            model.Filter.PublishEndate = pEdate;

            using (var module = ListFactoryService.Create(ListMethodType.NEWS))
            {
                model.Result = (module.DoGetList(model.Filter) as ListResultBase);
                model.Result.Data = (model.Result.Data as List<NEWS>);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult NewsAdd()
        {
            NewsDetailsDataModel defaultModel = new NewsDetailsDataModel();
            defaultModel.Data.BUD_DT = DateTime.UtcNow.AddHours(8);
            defaultModel.Data.HOME_PAGE_DISPLAY = true;
            using (var db = new REDOXDB())
            {
                if (db.NEWS.Count() > 0)
                    defaultModel.Data.SQ = db.NEWS.Max(s => s.SQ) + 1;
                else
                    defaultModel.Data.SQ = 1;
            }
            return View(defaultModel);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult NewsAdd(FormCollection form, List<HttpPostedFileBase> files)
        {
            int identityId = 0;
            using (var module = ListFactoryService.Create(Enums.ListMethodType.NEWS))
            {
                identityId = module.DoSaveData(form, null, files);
            }
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("NewsEdit", "_SysAdm", new { ID = identityId });
            return Json(new { Url = redirectUrl });
        }

        [HttpGet]
        public ActionResult NewsEdit(int? ID)
        {
            if (!ID.HasValue)
                return RedirectToAction("NewsList");
            NewsDetailsDataModel model;
            using (var module = ListFactoryService.Create(Enums.ListMethodType.NEWS))
            {
                model = (module.DoGetDetailsByID((int)ID) as NewsDetailsDataModel);
            }
            if (model.Data == null)
                return RedirectToAction("Login", "SignIn");

            //取檔案
            using (FileModule fileModule = new FileModule())
            {
                model.FilesData = fileModule.GetFiles((int)model.Data.ID, "News");
            }
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult NewsEdit(FormCollection form, List<HttpPostedFileBase> files)
        {
            int? ID = Convert.ToInt32(form["ID"]);
            int identityId = 0;
            using (var module = ListFactoryService.Create(Enums.ListMethodType.NEWS))
            {
                identityId = module.DoSaveData(form, ID, files);
            }
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("NewsEdit", "_SysAdm", new { ID = identityId });
            return Json(new { Url = redirectUrl });
        }

        [HttpPost]
        public JsonResult NewsDelete(int? ID)
        {
            bool success = true;
            string messages = string.Empty;
            try
            {
                using (var module = ListFactoryService.Create(Enums.ListMethodType.NEWS))
                {
                    module.DoDeleteByID((int)ID);
                }
                messages = "刪除成功";
            }
            catch (Exception ex)
            {
                success = false;
                messages = ex.Message;
            }
            var resultJson = Json(new { success = success, messages = messages });
            return resultJson;
        }

        #endregion NEWS

        [HttpGet]
        public ActionResult ProductsInfo()
        {
            var mdu = new ProductsModule<ListFilterBase, PRODUCT>();
            var model = mdu.GetProductInfo();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ProductsInfo(FormCollection form)
        {
            var mdu = new ProductsModule<ListFilterBase, PRODUCT>();
            var acName = mdu.SaveProductInfo(form);
            return RedirectToAction("ProductsInfo");
        }

        #region Product

        public ActionResult ProductsList(int? page, string qry, string sort, string fSt, string pSdate, string pEdate)
        {
            PublicMethodRepository.CurrentMode = SiteMode.Back;
            ListViewBase model = new ListViewBase();
            model.Filter.CurrentPage = page ?? 1;
            model.Filter.QueryString = qry ?? string.Empty;
            model.Filter.SortColumn = sort ?? string.Empty;
            model.Filter.DisplayForFrontEnd = fSt ?? string.Empty;
            model.Filter.PublishStartDate = pSdate;
            model.Filter.PublishEndate = pEdate;

            using (var module = ListFactoryService.Create(ListMethodType.PRODUCT))
            {
                model.Result = (module.DoGetList(model.Filter) as ListResultBase);
                model.Result.Data = (model.Result.Data as List<PRODUCT>);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult ProductsAdd()
        {
            ProductDetailsDataModel defaultModel = new ProductDetailsDataModel();
            defaultModel.Data.BUD_DT = DateTime.UtcNow.AddHours(8);
            defaultModel.Data.HOME_PAGE_DISPLAY = true;
            using (var db = new REDOXDB())
            {
                if (db.PRODUCT.Count() > 0)
                    defaultModel.Data.SQ = db.PRODUCT.Max(s => s.SQ) + 1;
                else
                    defaultModel.Data.SQ = 1;
            }
            return View(defaultModel);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ProductsAdd(FormCollection form, List<HttpPostedFileBase> files)
        {
            int identityId = 0;
            using (var module = ListFactoryService.Create(Enums.ListMethodType.PRODUCT))
            {
                identityId = module.DoSaveData(form, null, files);
            }
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("ProductsEdit", "_SysAdm", new { ID = identityId });
            return Json(new { Url = redirectUrl });
        }

        [HttpGet]
        public ActionResult ProductsEdit(int? ID)
        {
            if (!ID.HasValue)
                return RedirectToAction("ProductList");
            ProductDetailsDataModel model;
            using (var module = ListFactoryService.Create(Enums.ListMethodType.PRODUCT))
            {
                model = (module.DoGetDetailsByID((int)ID) as ProductDetailsDataModel);
            }
            if (model.Data == null)
                return RedirectToAction("Login", "SignIn");

            //取檔案
            using (FileModule fileModule = new FileModule())
            {
                model.FilesData = fileModule.GetFiles((int)model.Data.ID, "Products");
            }
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ProductsEdit(FormCollection form, List<HttpPostedFileBase> files)
        {
            int? ID = Convert.ToInt32(form["ID"]);
            int identityId = 0;
            using (var module = ListFactoryService.Create(Enums.ListMethodType.PRODUCT))
            {
                identityId = module.DoSaveData(form, ID, files);
            }
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("ProductsEdit", "_SysAdm", new { ID = identityId });
            return Json(new { Url = redirectUrl });
        }

        [HttpPost]
        public JsonResult ProductsDelete(int? ID)
        {
            bool success = true;
            string messages = string.Empty;
            try
            {
                using (var module = ListFactoryService.Create(Enums.ListMethodType.PRODUCT))
                {
                    module.DoDeleteByID((int)ID);
                }
                messages = "刪除成功";
            }
            catch (Exception ex)
            {
                success = false;
                messages = ex.Message;
            }
            var resultJson = Json(new { success = success, messages = messages });
            return resultJson;
        }

        #endregion Product


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
using OutWeb.Enums;
using OutWeb.Inc;
using OutWeb.Models.Manage;
using OutWeb.Models.Manage.ManageProductModels;
using OutWeb.Module.Manage;
using OutWeb.Modules.Manage;
using OutWeb.Repositories;
using OutWeb.Service;
using REDOXEntities.DataBase;
using System.Collections.Generic;
using System.Web.Mvc;

namespace OutWeb.Controllers
{
    public class ProductsController : Controller
    {

        public ProductsController()
        {
            ViewBag.IsFirstPage = true; 
            PublicMethodRepository.CurrentMode = SiteMode.FronEnd;
        }


        public ActionResult Index()
        {
            var mdu = new ProductsModule<ListFilterBase, PRODUCT>();
            Dictionary<bool, string> productInfo = mdu.GetProductInfo();
            TempData["productInfo"] = productInfo;

            ListViewBase model = new ListViewBase();
            Dictionary<int, List<FileViewModel>> files = new Dictionary<int, List<FileViewModel>>();
            using (var module = ListFactoryService.Create(ListMethodType.PRODUCT))
            {
                model.Result = (module.DoGetList(model.Filter) as ListResultBase);

                foreach (var data in (model.Result.Data as List<PRODUCT>))
                {
                    //取檔案
                    using (FileModule fileModule = new FileModule())
                    {
                        var file = fileModule.GetFiles((int)data.ID, "Products");
                        if (!files.ContainsKey(data.ID))
                                files.Add(data.ID, new List<FileViewModel>());
                        files[data.ID] = file;
                    }
                }
            }
            TempData["Files"] = files;
            return View(model);
        }

        public ActionResult Content(int? ID)
        {
            if (!ID.HasValue)
                return RedirectToAction("Index");

            ProductDetailsDataModel model;
            using (var module = ListFactoryService.Create(Enums.ListMethodType.PRODUCT))
            {
                model = (module.DoGetDetailsByID((int)ID) as ProductDetailsDataModel);
            }
            if (model.Data == null)
                return RedirectToAction("Index");

            //取檔案
            using (FileModule fileModule = new FileModule())
            {
                model.FilesData = fileModule.GetFiles((int)model.Data.ID, "Products");
            }
            return View(model);
        }
    }
}
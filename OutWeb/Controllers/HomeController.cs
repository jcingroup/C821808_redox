using OutWeb.Enums;
using OutWeb.Inc;
using OutWeb.Models.Front;
using OutWeb.Models.Manage;
using OutWeb.Module.Manage;
using OutWeb.Modules.Manage;
using OutWeb.Repositories;
using OutWeb.Service;
using REDOXEntities.DataBase;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OutWeb.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            ViewBag.IsFirstPage = true; //是否為首頁，請在首頁的Action此值設為True
            PublicMethodRepository.CurrentMode = SiteMode.FronEnd;
        }

        public ActionResult Index()
        {
            var mdu = new ProductsModule<ListFilterBase, PRODUCT>();
            Dictionary<bool, string> productInfo = mdu.GetProductInfo();
            TempData["productInfo"] = productInfo;

            HomeDataModel model = new HomeDataModel();
            Dictionary<int, List<FileViewModel>> productFiles = new Dictionary<int, List<FileViewModel>>();
            Dictionary<int, List<FileViewModel>> newstFiles = new Dictionary<int, List<FileViewModel>>();
            using (var module = ListFactoryService.Create(ListMethodType.PRODUCT))
            {
                ListViewBase temp = new ListViewBase();
                List<PRODUCT> bindProductData = new List<PRODUCT>();
                var productResult = (module.DoGetList(temp.Filter) as ListResultBase);

                foreach (var data in (productResult.Data as List<PRODUCT>).Take(2))
                {
                    //取檔案
                    using (FileModule fileModule = new FileModule())
                    {
                        var file = fileModule.GetFiles((int)data.ID, "Products");
                        if (!productFiles.ContainsKey(data.ID))
                            productFiles.Add(data.ID, new List<FileViewModel>());
                        productFiles[data.ID] = file;
                    }
                    bindProductData.Add(data);
                }
                temp.Result.Data = bindProductData;
                model.Result.Add("Products", temp);

            }
            TempData["ProductFiles"] = productFiles;

            using (var module = ListFactoryService.Create(ListMethodType.NEWS))
            {
                ListViewBase temp = new ListViewBase();
                List<NEWS> bindNewsData = new List<NEWS>();

                var newResult = (module.DoGetList(temp.Filter) as ListResultBase);

                foreach (var data in (newResult.Data as List<NEWS>).Take(3))
                {
                    data.CONTENT = data.CONTENT = PublicMethodRepository.StripHTML(data.CONTENT).SplitLengthString(60);
                    //取檔案
                    using (FileModule fileModule = new FileModule())
                    {
                        var file = fileModule.GetFiles((int)data.ID, "News");
                        if (!newstFiles.ContainsKey(data.ID))
                            newstFiles.Add(data.ID, new List<FileViewModel>());
                        newstFiles[data.ID] = file;
                        bindNewsData.Add(data);
                    }
                }
                temp.Result.Data = bindNewsData;
                model.Result.Add("News", temp);
            }
            TempData["NewstFiles"] = newstFiles;


            return View(model);



        }

        public ActionResult AboutUs()
        {
            return View();
        }

        //關於阿勃勒
        public ActionResult Info()
        {
            return View();
        }

        public ActionResult Service()
        {
            return View();
        }

        public ActionResult Buy()
        {
            return View();
        }
    }
}
using OutWeb.Enums;
using OutWeb.Inc;
using OutWeb.Models.Manage;
using OutWeb.Models.Manage.ManageNewsModels;
using OutWeb.Modules.Manage;
using OutWeb.Repositories;
using OutWeb.Service;
using REDOXEntities.DataBase;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OutWeb.Controllers
{
    public class NewsController : Controller
    {
        public NewsController()
        {
            ViewBag.IsFirstPage = true;
            PublicMethodRepository.CurrentMode = SiteMode.FronEnd;
        }
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        public ActionResult List()
        {
            ListViewBase model = new ListViewBase();
            Dictionary<int, List<FileViewModel>> files = new Dictionary<int, List<FileViewModel>>();
            using (var module = ListFactoryService.Create(ListMethodType.NEWS))
            {
                model.Result = (module.DoGetList(model.Filter) as ListResultBase);
                List<NEWS> dataBind = (model.Result.Data as List<NEWS>).Take(9).ToList();
                foreach (var data in dataBind)
                {
                    data.CONTENT = data.CONTENT = PublicMethodRepository.StripHTML(data.CONTENT).SplitLengthString(60);
                    //取檔案
                    using (FileModule fileModule = new FileModule())
                    {
                        var file = fileModule.GetFiles((int)data.ID, "News");
                        if (!files.ContainsKey(data.ID))
                            files.Add(data.ID, new List<FileViewModel>());
                        files[data.ID] = file;
                    }
                }
                model.Result.Data = dataBind;
            }
            TempData["Files"] = files;
            return View(model);
        }
        public ActionResult Content(int? ID)
        {
            if (!ID.HasValue)
                return RedirectToAction("Index");

            NewsDetailsDataModel model;
            using (var module = ListFactoryService.Create(Enums.ListMethodType.NEWS))
            {
                model = (module.DoGetDetailsByID((int)ID) as NewsDetailsDataModel);
            }
            if (model.Data == null)
                return RedirectToAction("Index");

            //取檔案
            using (FileModule fileModule = new FileModule())
            {
                model.FilesData = fileModule.GetFiles((int)model.Data.ID, "News");
            }

            string lastPageId = string.Empty;
            string nextPageId = string.Empty;
            using (var module = ListFactoryService.Create(ListMethodType.NEWS))
            {
                ListViewBase temp = new ListViewBase();
                var lstData = (module.DoGetList(temp.Filter) as ListResultBase);
                List<int> pageIDs = (lstData.Data as List<NEWS>).Select(s => s.ID).ToList();
                int idIndex = pageIDs.IndexOf(ID.Value);
                lastPageId = idIndex == 0 ? "" : pageIDs[idIndex - 1].ToString();
                nextPageId = idIndex == pageIDs.Count - 1 ? "" : pageIDs[idIndex + 1].ToString();
            }

            List<string> pageInfo = new List<string>() { lastPageId, nextPageId };
            TempData["PageInfo"] = pageInfo;
            return View(model);
        }
    }
}
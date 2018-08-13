using OutWeb.Enums;
using OutWeb.Expansion.ExcepProcess;
using OutWeb.Models;
using OutWeb.Models.Manage;
using OutWeb.Models.Manage.FileModels;
using OutWeb.Models.Manage.ManageProductModels;
using OutWeb.Provider;
using OutWeb.Repositories;
using OutWeb.Service;
using REDOXEntities.DataBase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OutWeb.Module.Manage
{
    public class ProductsModule<TFilter, TData> : ListBaseModule, IListFilter<TData> where TFilter : ListFilterBase where TData : PRODUCT
    {
        private string _actionName { get; set; } = string.Empty;

        private REDOXDB DB { get; set; } = new REDOXDB();
        private string rootPath { get { return HttpContext.Current.Server.MapPath("~/"); } }

        public ProductsModule()
        {
            _actionName = "Products";
        }

        public Dictionary<bool, string> GetProductInfo()
        {
            Dictionary<bool, string> result = new Dictionary<bool, string>();

            using (var db = new REDOXDB())
            {
                result = db.LISTEDITOR
                    .Where(s => s.OBJ_NAME == _actionName)
                    .ToDictionary(d => d.STATUS, d => d.OBJ_CONTENT);
            }
            if (result.Count > 0)
                PublicMethodRepository.HtmlDecode(result.Values.First());
            return result;
        }

        public string SaveProductInfo(FormCollection form)
        {
            bool status = form["Status"] == "Y" ? true : false;
            string content = form["contenttext"];

            PublicMethodRepository.FilterXss(content);

            using (var db = new REDOXDB())
            {
                ModelInitProperty saveModel = ModelInitProperty.NotSet;
                LISTEDITOR entity = db.LISTEDITOR.Where(s => s.OBJ_NAME == _actionName).FirstOrDefault();
                if (entity == null)
                {
                    entity = new LISTEDITOR
                    {
                        OBJ_NAME = _actionName,
                        BUD_DT = DateTime.UtcNow.AddHours(8),
                        BUD_ID = UserProvider.Instance.User.ID
                    };
                    saveModel = ModelInitProperty.New;
                }
                else
                    saveModel = ModelInitProperty.FromDB;
                entity.STATUS = status;
                entity.OBJ_CONTENT = content;
                entity.UP_DT = DateTime.UtcNow.AddHours(8);
                entity.UP_ID = UserProvider.Instance.User.ID;

                if (saveModel == ModelInitProperty.New)
                    db.LISTEDITOR.Add(entity);
                else
                    db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return _actionName;
        }

        public override object DoGetList(object filter)
        {
            ListResultBase result = new ListResultBase();
            TFilter filterModel = (filter as TFilter);
            PublicMethodRepository.FilterXss(filterModel);
            List<TData> data = new List<TData>();
            try
            {
                var enumerable = (IEnumerable<TData>)(typeof(REDOXDB).GetProperty(typeof(TData).Name).GetValue(DB, null));
                if (PublicMethodRepository.CurrentMode == SiteMode.FronEnd)
                    data = enumerable.Where(s => !s.DISABLE && s.HOME_PAGE_DISPLAY).ToList();
                else
                    data = enumerable.ToList();

                //關鍵字搜尋
                if (!string.IsNullOrEmpty(filterModel.QueryString))
                {
                    data = this.DoListFilterStringQuery(filterModel.QueryString, data);
                }
                //PUB_DT_STR搜尋
                if (!string.IsNullOrEmpty(filterModel.PublishStartDate) && !string.IsNullOrEmpty(filterModel.PublishEndate))
                {
                    data = this.DoListDateFilter(Convert.ToDateTime(filterModel.PublishStartDate), Convert.ToDateTime(filterModel.PublishEndate), data);
                }

                //上下架
                if (!string.IsNullOrEmpty(filterModel.DisplayForFrontEnd))
                {
                    data = this.DoListStatusFilter(filterModel.DisplayForFrontEnd, "F", data);
                }

                //SQ
                data = this.DoListSort(filterModel.SortColumn, filterModel.Status, data);

                //分頁
                bool isDoPage = PublicMethodRepository.CurrentMode == SiteMode.FronEnd ? false : true;
                data = this.DoListPageList(filterModel.CurrentPage, data, out PaginationResult pagination, isDoPage);
                result.Pagination = pagination;
                foreach (var d in data)
                    PublicMethodRepository.HtmlDecode(d);
                result.Data = data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public override int DoSaveData(FormCollection form, int? ID = null, List<HttpPostedFileBase> files = null)
        {
            PRODUCT saveModel;
            FileRepository fileRepository = new FileRepository();
            if (!ID.HasValue)
            {
                saveModel = new PRODUCT();
                saveModel.BUD_DT = DateTime.UtcNow.AddHours(8);
                saveModel.BUD_ID = UserProvider.Instance.User.ID;
            }
            else
            {
                saveModel = this.DB.PRODUCT.Where(s => s.ID == ID).FirstOrDefault();
            }
            saveModel.TITLE = form["TITLE"];
            saveModel.HOME_PAGE_DISPLAY = form["HOME_PAGE_DISPLAY"] == "on" ? true : false;
            saveModel.DISABLE = form["DISABLE"] == "Y" ? true : false;
            saveModel.SQ = form["SQ"] == null ? 1 : form["SQ"] == string.Empty ? 1 : Convert.ToInt32(form["SQ"]);
            saveModel.DESC = form["DESC"];
            saveModel.HOME_INFO = form["HOME_INFO"];
            saveModel.SPEC = form["SPEC"];
            saveModel.CONTENT = form["contenttext"];

            saveModel.EDIT1 = form["EDIT1"];
            saveModel.EDIT1_MOBILE = form["EDIT1_MOBILE"];
            saveModel.EDIT2 = form["EDIT2"];
            saveModel.EDIT2_MOBILE = form["EDIT2_MOBILE"];
            saveModel.EDIT3 = form["EDIT3"];
            saveModel.EDIT3_MOBILE = form["EDIT3_MOBILE"];
            saveModel.QA = form["QA"];

            saveModel.UPT_DT = DateTime.UtcNow.AddHours(8);
            saveModel.UPT_ID = UserProvider.Instance.User.ID;
            PublicMethodRepository.FilterXss(saveModel);

            if (ID.HasValue)
            {
                this.DB.Entry(saveModel).State = EntityState.Modified;
            }
            else
            {
                this.DB.PRODUCT.Add(saveModel);
            }

            try
            {
                this.DB.SaveChanges();
            }
            catch (Exception ex)
            {
                ex.AppException();
            }
            int identityId = (int)saveModel.ID;

            #region FILEBASE處理

            List<int> oldFileList = new List<int>();

            #region 將原存在的ServerFILEBASE保留 記錄FILEBASEID

            //將原存在的ServerFILEBASE保留 記錄FILEBASEID
            foreach (var f in form.Keys)
            {
                if (f.ToString().StartsWith("FileData"))
                {
                    var id = Convert.ToInt16(form[f.ToString().Split('.')[0] + ".ID"]);
                    if (!oldFileList.Contains(id))
                        oldFileList.Add(id);
                }
            }

            #endregion 將原存在的ServerFILEBASE保留 記錄FILEBASEID

            #region 建立FILEBASE模型

            FilesModel fileModel = new FilesModel()
            {
                ActionName = _actionName,
                ID = identityId,
                OldFileIds = oldFileList
            };

            #endregion 建立FILEBASE模型

            #region 若有null則是前端html的name重複於ajax formData名稱

            if (files != null)
            {
                if (files.Count > 0)
                    files.RemoveAll(item => item == null);
            }

            #endregion 若有null則是前端html的name重複於ajax formData名稱

            #region img data binding 單筆多筆裝在不同容器

            fileRepository.UploadFile(fileModel, files, identityId, _actionName);
            fileRepository.SaveFileToDB(fileModel);

            #endregion img data binding 單筆多筆裝在不同容器

            #endregion FILEBASE處理

            return identityId;
        }

        public override object DoGetDetailsByID(int ID)
        {
            ProductDetailsDataModel result = new ProductDetailsDataModel();
            PRODUCT data = new PRODUCT();
            if (PublicMethodRepository.CurrentMode == SiteMode.FronEnd)
            {
                data = DB.PRODUCT.Where(w => w.ID == ID && !w.DISABLE && w.HOME_PAGE_DISPLAY).FirstOrDefault();
            }
            else
            {
                data = DB.PRODUCT.Where(w => w.ID == ID).FirstOrDefault();
            }
            PublicMethodRepository.HtmlDecode(data);
            result.Data = data;
            return result;
        }

        public override void DoDeleteByID(int ID)
        {
            var data = this.DB.PRODUCT.Where(s => s.ID == ID).FirstOrDefault();
            if (data == null)
                throw new Exception("[刪除PRODUCTS] 查無此PRODUCTS，可能已被移除");
            try
            {
                var delFiles = this.DB.FILEBASE.Where(o => o.MAP_SITE.StartsWith(_actionName) && o.MAP_ID == ID).ToList();
                if (delFiles.Count > 0)
                {
                    foreach (var f in delFiles)
                        File.Delete(string.Concat(rootPath, f.FILE_PATH));
                    this.DB.FILEBASE.RemoveRange(delFiles);
                }

                this.DB.PRODUCT.Remove(data);
                this.DB.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("[刪除PRODUCTS]" + ex.Message);
            }
        }

        public List<TData> DoListFilterStringQuery(string filterStr, List<TData> data)
        {
            var result = data.Where(s => s.TITLE.Contains(filterStr.Trim())).ToList();
            return result;
        }

        public List<TData> DoListDateFilter(DateTime publishSdate, DateTime publishEdate, List<TData> data)
        {
            throw new NotImplementedException();
        }

        public List<TData> DoListCategoryFilter(int cateID, List<TData> data)
        {
            throw new NotImplementedException();
        }

        public List<TData> DoListStatusFilter(string status, string displayMode, List<TData> data)
        {
            List<TData> result = null;
            if (displayMode == "F")
            {
                if (status == "Y")
                    result = data.Where(s => s.DISABLE == false).ToList();
                else
                    result = data.Where(s => s.DISABLE == true).ToList();
            }
            return result;
        }

        /// <summary>
        /// 取出分頁資料
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="data"></param>
        public List<TData> DoListPageList(int currentPage, List<TData> data, out PaginationResult pagination, bool doPagination = true)
        {
            int pageSize = doPagination ? PublicMethodRepository.ListPageSize : data.Count() == 0 ? 1 : data.Count();

            int startRow = (currentPage - 1) * pageSize;
            PaginationResult paginationResult = new PaginationResult()
            {
                CurrentPage = currentPage,
                DataCount = data.Count,
                PageSize = pageSize,
                FirstPage = 1,
                LastPage = Convert.ToInt32(Math.Ceiling((decimal)data.Count / pageSize))
            };
            pagination = paginationResult;
            data = data.Skip(startRow).Take(pageSize).ToList();
            return data;
        }

        /// <summary>
        /// 列表SQ功能
        /// </summary>
        /// <param name="sortCloumn"></param>
        /// <param name="data"></param>
        public List<TData> DoListSort(string sortCloumn, string status, List<TData> data)
        {
            switch (sortCloumn)
            {
                case "sortTitle/asc":
                    data = data.OrderBy(o => o.TITLE).ThenBy(g => g.SQ).ToList();
                    break;

                case "sortTitle/desc":
                    data = data.OrderByDescending(o => o.TITLE).ThenBy(g => g.SQ).ToList();
                    break;

                case "sortSpec/asc":
                    data = data.OrderBy(o => o.SPEC).ThenByDescending(g => g.SQ).ToList();
                    break;

                case "sortSpec/desc":
                    data = data.OrderByDescending(o => o.SPEC).ThenByDescending(g => g.SQ).ToList();
                    break;

                case "sortStatus/asc":
                    data = data.OrderBy(o => o.DISABLE).ThenBy(g => g.SQ).ToList();
                    break;

                case "sortStatus/desc":
                    data = data.OrderByDescending(o => o.DISABLE).ThenBy(g => g.SQ).ToList();
                    break;
                case "sortIndex/asc":
                    data = data.OrderBy(o => o.SQ).ThenBy(g => g.SQ).ToList();
                    break;

                case "sortIndex/desc":
                    data = data.OrderByDescending(o => o.SQ).ThenBy(g => g.SQ).ToList();
                    break;

                default:
                    data = data.OrderByDescending(o => o.SQ).ThenByDescending(g => g.BUD_DT).ToList();
                    break;
            }
            return data;
        }

        public override void Dispose()
        {
            if (this.DB.Database.Connection.State == System.Data.ConnectionState.Open)
            {
                this.DB.Database.Connection.Close();
            }
            this.DB.Dispose();
            this.DB = null;
        }
    }
}
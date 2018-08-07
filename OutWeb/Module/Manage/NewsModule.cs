using OutWeb.Enums;
using OutWeb.Models;
using OutWeb.Models.Manage;
using OutWeb.Models.Manage.FileModels;
using OutWeb.Models.Manage.ManageNewsModels;
using OutWeb.Provider;
using OutWeb.Repositories;
using OutWeb.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using REDOXEntities.DataBase;
using OutWeb.Expansion.ExcepProcess;
using OutWeb.Expansion.Datetime;

namespace OutWeb.Module.Manage
{
    /// <summary>
    /// NEWS列表模組
    /// </summary>
    public class NewsModule<TFilter, TData> : ListBaseModule, IListFilter<TData> where TFilter : ListFilterBase where TData : NEWS
    {
        private string _actionName { get; set; } = string.Empty;
        public NewsModule()
        {
            _actionName = "News";
        }
        private REDOXDB DB { get; set; } = new REDOXDB();

        private string rootPath { get { return HttpContext.Current.Server.MapPath("~/"); } }

        public override void DoDeleteByID(int ID)
        {
            var data = this.DB.NEWS.Where(s => s.ID == ID).FirstOrDefault();
            if (data == null)
                throw new Exception("[刪除NEWS] 查無此NEWS，可能已被移除");
            try
            {
                var delFiles = this.DB.FILEBASE.Where(o => o.MAP_SITE.StartsWith(_actionName) && o.MAP_ID == ID).ToList();
                if (delFiles.Count > 0)
                {
                    foreach (var f in delFiles)
                        File.Delete(string.Concat(rootPath, f.FILE_PATH));
                    this.DB.FILEBASE.RemoveRange(delFiles);
                }

                this.DB.NEWS.Remove(data);
                this.DB.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("[刪除NEWS]" + ex.Message);
            }
        }

        public override object DoGetDetailsByID(int ID)
        {
            NewsDetailsDataModel result = new NewsDetailsDataModel();
            NEWS data = DB.NEWS.Where(w => w.ID == ID).FirstOrDefault();
            PublicMethodRepository.HtmlDecode(data);
            result.Data = data;
            return result;
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

                //前台顯示
                if (!string.IsNullOrEmpty(filterModel.DisplayForFrontEnd))
                {
                    data = this.DoListStatusFilter(filterModel.DisplayForFrontEnd, "F", data);
                }

                //SQ
                data = this.DoListSort(filterModel.SortColumn, filterModel.Status, data);

                //分頁
                data = this.DoListPageList(filterModel.CurrentPage, data, out PaginationResult pagination);
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
            NEWS saveModel;
            FileRepository fileRepository = new FileRepository();
            if (!ID.HasValue)
            {
                saveModel = new NEWS();
                saveModel.BUD_DT = DateTime.UtcNow.AddHours(8);

                saveModel.BUD_ID = UserProvider.Instance.User.ID;
            }
            else
            {
                saveModel = this.DB.NEWS.Where(s => s.ID == ID).FirstOrDefault();
            }
            saveModel.TITLE = form["TITLE"];
            saveModel.HOME_PAGE_DISPLAY = form["HOME_PAGE_DISPLAY"] == "on" ? true : false;
            saveModel.DISABLE = form["DISABLE"] == "Y" ? true : false;
            saveModel.SQ = form["SQ"] == null ? 1 : form["SQ"] == string.Empty ? 1 : Convert.ToInt32(form["SQ"]);
            saveModel.CONTENT = form["contenttext"];
            saveModel.PUB_DT_STR = form["PUB_DT_STR"];
            saveModel.UPT_DT = DateTime.UtcNow.AddHours(8);
            saveModel.UPT_ID = UserProvider.Instance.User.ID;
            PublicMethodRepository.FilterXss(saveModel);

            if (ID.HasValue)
            {
                this.DB.Entry(saveModel).State = EntityState.Modified;
            }
            else
            {
                this.DB.NEWS.Add(saveModel);
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

        /// <summary>
        /// 列表關鍵字搜尋
        /// </summary>
        /// <param name="filterStr"></param>
        /// <param name="data"></param>
        public List<TData> DoListFilterStringQuery(string filterStr, List<TData> data)
        {
            var result = data.Where(s => s.TITLE.Contains(filterStr.Trim())).ToList();
            return result;
        }

        /// <summary>
        /// 日期條件搜尋
        /// </summary>
        /// <param name="filterStr"></param>
        /// <param name="data"></param>
        public List<TData> DoListDateFilter(DateTime publishSdate, DateTime publishEdate, List<TData> data)
        {
            publishEdate = new DateTime(publishEdate.Year, publishEdate.Month, publishEdate.Day, 23, 59, 59);
            var r = data.Where(s => s.PUB_DT_STR.ToDatetime() >= publishSdate &&
                   s.PUB_DT_STR.ToDatetime() <= publishEdate).ToList();
            return r;
        }

        /// <summary>
        /// 前台顯示搜尋
        /// </summary>
        /// <param name="filterStr"></param>
        /// <param name="data"></param>
        public List<TData> DoListStatusFilter(string status, string displayMode, List<TData> data)
        {
            List<TData> result = null;
            if (displayMode == "F")
            {
                if (status == "Y")
                    result = data.Where(s => s.DISABLE == true).ToList();
                else
                    result = data.Where(s => s.DISABLE == false).ToList();
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
                case "sortPublishDate/asc":
                    data = data.OrderBy(o => o.BUD_DT).ThenBy(g => g.SQ).ToList();
                    break;

                case "sortPublishDate/desc":
                    data = data.OrderByDescending(o => o.BUD_DT).ThenBy(g => g.SQ).ToList();
                    break;

                case "sortIndex/asc":
                    data = data.OrderBy(o => o.SQ).ThenByDescending(g => g.BUD_DT).ToList();
                    break;

                case "sortIndex/desc":
                    data = data.OrderByDescending(o => o.SQ).ThenByDescending(g => g.BUD_DT).ToList();
                    break;

                case "sortDisable/asc":
                    data = data.OrderBy(o => o.DISABLE).ThenBy(g => g.SQ).ToList();
                    break;

                case "sortDisable/desc":
                    data = data.OrderByDescending(o => o.DISABLE).ThenBy(g => g.SQ).ToList();
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
        public List<TData> DoListCategoryFilter(int cateID, List<TData> data)
        {
            throw new NotImplementedException();
        }
    }
}
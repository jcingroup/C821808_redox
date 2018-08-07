using OutWeb.Enums;
using OutWeb.Expansion.ExcepProcess;
using OutWeb.Models;
using OutWeb.Models.Manage;
using OutWeb.Models.Manage.FileModels;
using OutWeb.Provider;
using OutWeb.Repositories;
using OutWeb.Service;
using REDOXEntities.DataBase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OutWeb.Module.Manage
{
    public class ProductsModule<TFilter, TData> : ListBaseModule, IListFilter<TData> where TFilter : ListFilterBase where TData : PRODUCT
    {
        private string _actionName { get; set; } = string.Empty;

        private REDOXDB DB { get; set; } = new REDOXDB();


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

        public override object DoGetList(object model)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override void DoDeleteByID(int ID)
        {
            throw new NotImplementedException();
        }

        public List<TData> DoListFilterStringQuery(string filterStr, List<TData> data)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public List<TData> DoListPageList(int currentPage, List<TData> data, out PaginationResult pagination, bool doPagination = true)
        {
            throw new NotImplementedException();
        }

        public List<TData> DoListSort(string sortCloumn, string status, List<TData> data)
        {
            throw new NotImplementedException();
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
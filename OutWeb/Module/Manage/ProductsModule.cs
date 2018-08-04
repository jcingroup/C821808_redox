using OutWeb.Enums;
using OutWeb.Provider;
using OutWeb.Repositories;
using REDOXEntities.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OutWeb.Module.Manage
{
    public class ProductsModule
    {
        private string _actionName { get; set; } = string.Empty;

        public ProductsModule()
        {
            _actionName = "ProductsInfo";
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
    }
}
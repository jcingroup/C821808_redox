using REDOXEntities.DataBase;
using System.Collections.Generic;
using REDOXEntities.DataBase;

namespace OutWeb.Models.Manage.ManageNewsModels
{
    public class NewsDetailsDataModel : IManage
    {
        /// <summary>
        /// 圖片
        /// </summary>
        public List<FileViewModel> FilesData { get; set; } = new List<FileViewModel>();

        public NEWS Data { get; set; } = new NEWS();
        public List<FileViewModel> ImagesData { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
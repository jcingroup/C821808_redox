using REDOXEntities.DataBase;
using System.Collections.Generic;

namespace OutWeb.Models.Manage.ManageProductModels
{
    public class ProductDetailsDataModel : IManage
    {
        /// <summary>
        /// 圖片
        /// </summary>
        public List<FileViewModel> FilesData { get; set; } = new List<FileViewModel>();

        public PRODUCT Data { get; set; } = new PRODUCT();
        public List<FileViewModel> ImagesData { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
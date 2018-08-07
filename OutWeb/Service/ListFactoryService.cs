using OutWeb.Enums;
using OutWeb.Models.Manage;
using OutWeb.Module.Manage;
using REDOXEntities.DataBase;

namespace OutWeb.Service
{
    public class ListFactoryService
    {
        public static ListBaseModule Create(ListMethodType methodType)
        {
            ListBaseModule listManageModule = null;
            switch (methodType)
            {
                case ListMethodType.NEWS:
                    listManageModule = new NewsModule<ListFilterBase, NEWS>();
                    break;
                case ListMethodType.PRODUCT:
                    listManageModule = new ProductsModule<ListFilterBase, PRODUCT>();
                    break;
                default:
                    listManageModule = null;
                    break;
            }
            return listManageModule;
        }
    }
}
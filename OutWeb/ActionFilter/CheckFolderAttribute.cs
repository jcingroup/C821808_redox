using OutWeb.Repositories;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace OutWeb.ActionFilter
{
    public class CheckFolderAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string temp = HttpContext.Current.Server.MapPath("~/Content/Upload/Manage/Images/Temp");
            if (Directory.Exists(temp))
            {
                var files = Directory.GetFiles(temp);
                if (files.Length > 0)
                {
                    foreach (var f in files)
                        File.Delete(f);
                }
                Directory.Delete(temp);
            }

            List<string[]> dirList = new List<string[]>()
            {
                new string[]{ "Content", "Upload", "Manage", "Images", "Temp" },
                new string[] { "Content", "Upload", "Manage", "Files", "Temp" },
                new string[] { "MailJson", "finish" },
                new string[]{ "Content", "ExcelTemp" },
                new string[]{ "Content", "Upload", "Manage", "Files", "News" },
                new string[]{ "Content", "Upload", "Ck"},
            };

            foreach (var ary in dirList)
            {
                PublicMethodRepository.CreateDir(ary);
            }

            base.OnActionExecuting(filterContext);
        }


    }
}
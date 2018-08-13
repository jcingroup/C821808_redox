using OutWeb.Models.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OutWeb.Models.Front
{
    public class HomeDataModel
    {
        public Dictionary<string, ListViewBase> Result { get; set; } = new Dictionary<string, ListViewBase>();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OutWeb.Models.ValidModels
{
    public interface IModelValidate
    {
        bool Valid(object value);
        string Message { get; }
    }
}
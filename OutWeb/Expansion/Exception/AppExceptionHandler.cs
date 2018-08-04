using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;

namespace OutWeb.Expansion.ExcepProcess
{
    public static class AppExceptionHandler
    {
        public static void AppException(this Exception e)
        {
            string message = string.Empty;
            if (e is DbEntityValidationException)
            {
                StringBuilder entityErrors = new StringBuilder();
                foreach (var eve in (e as DbEntityValidationException).EntityValidationErrors)
                {
                    entityErrors.AppendLine(
                        string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        entityErrors.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }
                message = entityErrors.ToString();
            }
            else
            {
                message = e.Message;
            }
            throw new Exception(message);
        }

    }


}
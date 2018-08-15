using GoogleRecaptcha;
using Newtonsoft.Json;
using OutWeb.Inc;
using OutWeb.Repositories;
using System.Web.Mvc;

namespace OutWeb.Controllers
{
    public class ContactUsController : Controller
    {
        public ContactUsController()
        {
            ViewBag.IsFirstPage = false;
        }

        public ActionResult Index()
        {
            return View("ContactUs");
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ValidRecaptcha(FormCollection form)
        {
            var content = new JsonResult();
            bool isValid = true;

            string secret = PublicMethodRepository.GetConfigAppSetting("reCAPTCHASecret");
            IRecaptcha<RecaptchaV2Result> recaptcha = new RecaptchaV2(new RecaptchaV2Data()
            {
                Secret = secret
            });

            var result = recaptcha.Verify();
            if (!result.Success)
            {
                isValid = false;
            }

            content.Data = JsonConvert.SerializeObject(new { success = isValid }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            content.ContentType = "application/json";
            content.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return content;
        }


        [HttpPost]
        public ActionResult ContactUs(FormCollection form)
        {
            return View();
        }
    }
}
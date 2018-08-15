﻿using GoogleRecaptcha;
using Newtonsoft.Json;
using OutWeb.Repositories;
using OutWeb.Service.MailerProcess;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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
            string msg = string.Empty;

            string secret = PublicMethodRepository.GetConfigAppSetting("reCAPTCHASecret");

            if (secret == null)
            {
                isValid = false;
                msg = "無法取得Google金鑰";
            }
            else
            {
                IRecaptcha<RecaptchaV2Result> recaptcha = new RecaptchaV2(new RecaptchaV2Data()
                {
                    Secret = secret
                });

                var result = recaptcha.Verify();
                if (!result.Success)
                {
                    isValid = false;
                }
            }

            content.Data = JsonConvert.SerializeObject(new { success = isValid, msg = msg }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            content.ContentType = "application/json";
            content.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return content;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ContactUs(FormCollection form)
        {
            string renderedHTML = RenderViewToString("MailController", "MsgMail", form);
            string mailTo = PublicMethodRepository.GetConfigAppSetting("MailTo");
            string subJect = "測試郵件";
            StringBuilder mailContent = new StringBuilder(renderedHTML);
            List<string> mailToList = new List<string>() { mailTo };
            var mailInfo = new MailInfo()
            {
                Subject = subJect,
                Body = mailContent,
                To = mailToList,
            };
            var isSend = new Mailer(mailInfo).SendMail();
            return View();
        }




        private static string RenderViewToString(string controllerName, string viewName, object model)
        {
            using (var writer = new StringWriter())
            {
                var routeData = new RouteData();
                routeData.Values.Add("controller", controllerName);
                //var emailControllerContext = new ControllerContext(new HttpContextWrapper(new HttpContext(new HttpRequest(null, "http://google.com", null), new HttpResponse(null))), routeData, new ViewTemplateController());
                var emailControllerContext = new ControllerContext(new HttpContextWrapper(System.Web.HttpContext.Current), routeData, new MailController());

                var razorViewEngine = new RazorViewEngine();
                var razorViewResult = razorViewEngine.FindView(emailControllerContext, viewName, "", false);

                var viewContext = new ViewContext(emailControllerContext, razorViewResult.View, new ViewDataDictionary(model), new TempDataDictionary(), writer);
                razorViewResult.View.Render(viewContext, writer);
                return writer.ToString();
            }
        }
    }
}
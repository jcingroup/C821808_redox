using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OutWeb.Service.MailerProcess
{
    public class MailSetting
    {
        public string SmtpServer { get; set; }
        public int? Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
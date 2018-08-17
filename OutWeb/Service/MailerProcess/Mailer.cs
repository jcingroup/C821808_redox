using OutWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace OutWeb.Service.MailerProcess
{
    public class Mailer
    {
        private MailInfo info { get; set; }
        private MailSetting setting { get { return _mailSetting; } }
        private static MailSetting _mailSetting
        {
            get
            {
                var getSetting = PublicMethodRepository.GetConfigAppSetting("SmtpInfo");
                if (getSetting == null)
                    throw new Exception("無法取得郵件SMTP設定,無法寄發郵件");
                var settingSplit = getSetting.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

                MailSetting setting = new MailSetting()
                {
                    SmtpServer = settingSplit[0],
                    Port = Convert.ToInt32(settingSplit[1]),
                    UserName = settingSplit.Length > 2 ? settingSplit[2] : "",
                    Password = settingSplit.Length > 3 ? settingSplit[3] : ""
                };

                return setting;
            }
        }
        /// <summary>
        /// init MailInfo
        /// </summary>
        /// <param name="info"></param>
        public Mailer(MailInfo info)
        {
            this.info = info;
        }

        public Boolean SendMail()
        {
            //ValidMailSetting();
            Boolean isSuccess = false;
            //string address = "furit1984@gmail.com";
            string address = "info@jcin.com.tw";
            MailMessage message = new MailMessage();
            try
            {
                if (info.To.Count == 0)
                    return false;
                foreach (var to in info.To)
                    message.To.Add(to);
                //寄件者
                message.From = new MailAddress(address, "顧客信通知");
                message.IsBodyHtml = true;
                message.Subject = info.Subject;
                message.Body = info.Body.ToString();
                message.Priority = MailPriority.Normal;

                if (info.CC != null) { info.CC.ForEach(x => message.CC.Add(x)); }

                # region 附件
                if (info.Files.Count > 0)
                {
                    foreach (string fileName in info.Files.Keys)
                    {
                        Attachment attfile = new Attachment(info.Files[fileName], fileName);
                        message.Attachments.Add(attfile);
                    }
                }
                #endregion

                #region SendMail

                #endregion

                using (SmtpClient client = new SmtpClient("msa.hinet.net"))
                {
                    client.EnableSsl = true;
                    //client.UseDefaultCredentials = false;
                    //client.Credentials = new System.Net.NetworkCredential(setting.UserName, setting.Password);
                    client.Timeout = Int32.MaxValue;
                    client.Send(message);
                }
                //using (SmtpClient client = new SmtpClient(setting.SmtpServer, (int)setting.Port))
                //{

                //    client.EnableSsl = true;
                //    //client.UseDefaultCredentials = false;
                //    client.Credentials = new System.Net.NetworkCredential(setting.UserName, setting.Password);
                //    client.Timeout = Int32.MaxValue;
                //    client.Send(message);
                //}

                isSuccess = true;
            }
            catch (Exception ex)
            {
                //todo log mail content
                throw new Exception(ex.Message + ", " + ex.StackTrace);
            }
            finally
            {
                if (message.Attachments != null && message.Attachments.Count > 0)
                    message.Attachments.All(a => { a.Dispose(); return true; });
                message.Dispose();
                message = null;
            }
            return isSuccess;
        }


        private void ValidMailSetting()
        {
            if (string.IsNullOrEmpty(this.setting.SmtpServer))
                throw new Exception("SMTP Server Is Empty.");
            //if (string.IsNullOrEmpty(this.setting.UserName))
            //    throw new Exception("UserName Is Empty.");
            //if (string.IsNullOrEmpty(this.setting.Password))
            //    throw new Exception("Password Is Empty.");
            if (this.setting.Port == null)
                throw new Exception("Port Is Null.");
        }
    }

}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace OutWeb.Service.MailerProcess
{
    /// <summary>
    /// Mail Info
    /// </summary>
    public class MailInfo
    {
        /// <summary>
        /// constructor
        /// </summary>
        public MailInfo()
        {
            this.CC = new List<string>();
            this.Body = new StringBuilder();
        }

        /// <summary>
        /// Mail ToMultiple
        /// </summary>
        public List<string> To { get { return m_to; } set { m_to = value; } }
        public List<string> m_to = new List<string>();

        /// <summary>
        /// Mail Title
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Type of String, Mail Body
        /// </summary>
        public StringBuilder Body { get; set; }

        /// <summary>
        /// Multiple CC
        /// </summary>
        public List<string> CC { get; set; }

        /// <summary>
        /// 附件
        /// </summary>

        public Dictionary<string, Stream> Files = new Dictionary<string, Stream>();
    }
}
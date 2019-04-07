using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace SMS.classes
{
    enum Result
    {
        TOTAL_SMS_SEND = 1,DELIVERED =2,FAILED =3,SMSC =4,EXPIRED =5,SMSC_REJECT=6,UNDELIVERED=7,TATBLOCKED=8
    }
    class EmailDetails
    {
        private string _from_email_id;
        private string _host_address;
        private string _credential_emailid;
        private string _credential_password;
        private int _port;
        private int _ssl_isactive;
        private int _is_body_html;

        private string _toEmailId;
        private string _cc;
        private string _bcc;
        private string _subject;
        private string _emailText;
        private string _attachedFile;

        public EmailDetails(){}

        public EmailDetails(string _from_email_id, string _host_address, string _credential_emailid, string _credential_password, int _port, int _ssl_isactive, int _is_body_html)
        {
            this._from_email_id = _from_email_id;
            this._host_address = _host_address;
            this._credential_emailid = _credential_emailid;
            this._credential_password = _credential_password;
            this._port = _port;
            this._ssl_isactive = _ssl_isactive;
            this._is_body_html = _is_body_html;
        }
        public void SendMail(string ccIds = "",string bccIds = "",string attachedFile = "")
        {
            MailMessage message = new MailMessage();
            message.To.Add(ToEmailId);
            message.From = new MailAddress(FromEmailId);

            if (ccIds.Trim() != "")
            {
                string all_cc_id = ccIds;
                string[] arry_cc = all_cc_id.Split(',');
                foreach (string item in arry_cc)
                {
                    message.CC.Add(new MailAddress(item));
                }
                
            }
            if (bccIds.Trim() !="")
            {
                string all_bcc_ids = bccIds;
                string[] array_bcc = all_bcc_ids.Split(',');
                foreach (string item1 in array_bcc)
                {
                    message.Bcc.Add(new MailAddress(item1));
                }
                
            }

            if (attachedFile != "")
            {
                Attachment atcmt = new Attachment(attachedFile);
                message.Attachments.Add(atcmt);
            }
            message.Subject = Subject;
            message.IsBodyHtml = (HtmlBodyActive == 1) ? true : false;

            message.Body = EmailText;
            
            #region smtp config
            SmtpClient client = new SmtpClient();
            client.Host = HostAddress;
            client.Port = Port;
            client.Credentials = new System.Net.NetworkCredential(CredentialEmailId,CredentialPassword);
            //client.UseDefaultCredentials = false; //added for test
            //client.DeliveryMethod = SmtpDeliveryMethod.Network; //added for test
            client.EnableSsl = (SsslActive == 1) ? true : false;
            client.Timeout = 30000;
            message.Priority = MailPriority.High;
            client.Send(message);
            #endregion
        }
        public string FromEmailId
        {
            get { return _from_email_id; }
            set { _from_email_id = value; }
        }
        public string CC
        {
            get { return _cc; }
            set { _cc = value; }
        }

        public string Bcc
        {
            get { return _bcc; }
            set { _bcc = value; }

        }
        public string HostAddress
        {
            get { return _host_address; }
            set { _host_address = value; }
        }
        public string CredentialEmailId
        {
            get { return _credential_emailid; }
            set { _credential_emailid = value; }
        }
        public string CredentialPassword
        {
            get { return _credential_password; }
            set { _credential_password = value; }
        }
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }
        public int SsslActive
        {
            get { return _ssl_isactive; }
            set { _ssl_isactive = value; }
        }
        public int HtmlBodyActive
        {
            get { return _is_body_html; }
            set { _is_body_html = value; }
        }

        public string ToEmailId
        {
            get { return _toEmailId; }
            set { _toEmailId = value; }
        }
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }
        public string EmailText
        {
            get { return _emailText; }
            set { _emailText = value; }
        }
        public string Attachment
        {
            get { return _attachedFile; }
            set { _attachedFile = value; }
        }
    }
}

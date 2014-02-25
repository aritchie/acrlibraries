using System;
using System.Net.Mail;


namespace Acr.Mail {
    
    public static class MailMessageExtensions {

        public static MailMessage AddTo(this MailMessage mail, params MailAddress[] mailAddresses) {
            mailAddresses.Each(mail.To.Add);
            return mail;
        }


        public static MailMessage AddCc(this MailMessage mail, params MailAddress[] mailAddresses) {
            mailAddresses.Each(mail.CC.Add);
            return mail;
        }


        public static MailMessage AddBcc(this MailMessage mail, params MailAddress[] mailAddresses) {
            mailAddresses.Each(mail.Bcc.Add);
            return mail;
        }


        public static MailMessage AddAttachment(this MailMessage mail, params Attachment[] attachments) {
            attachments.Each(mail.Attachments.Add);
            return mail;
        }


        public static MailMessage AddTo(this MailMessage mail, string displayName, string emailAddress) {
            mail.To.Add(new MailAddress(emailAddress, displayName));
            return mail;
        }


        public static MailMessage AddTo(this MailMessage mail, string emailAddress) {
            mail.To.Add(new MailAddress(emailAddress));
            return mail;
        }


        public static MailMessage AddTosFromString(this MailMessage mail, string strings) {
            return AddReceiversFromString(mail, strings, mail.To.Add);
        }


        public static MailMessage AddCcsFromString(this MailMessage mail, string strings) {
            return AddReceiversFromString(mail, strings, mail.CC.Add);
        }


        public static MailMessage AddBccsFromString(this MailMessage mail, string strings) {
            return AddReceiversFromString(mail, strings, mail.Bcc.Add);
        }


        public static MailMessage SetMessageID(this MailMessage mail, string messageID) {
            mail.Headers.Add("Message-ID", messageID);
            return mail;
        }


        public static string GetMessageID(this MailMessage mail) {
            return mail.Headers["Message-ID"];
        }


        public static MailMessage SetInReplyTo(this MailMessage mail, string messageID) {
            mail.Headers.Add("In-Reply-To", messageID);
            return mail;
        }


        public static string GetInReplyTo(this MailMessage mail) {
            return mail.Headers["In-Reply-To"];
        }


        private static MailMessage AddReceiversFromString(MailMessage mail, string strings, Action<MailAddress> action) {
            if (!strings.IsEmpty()) {
                strings
                    .SplitTrim(';')
                    .Each(x => action(new MailAddress(x)));
            }            
            return mail;
        }
    }
}

using System;
using System.Net.Mail;
using System.Threading.Tasks;


namespace Acr.Mail.Senders {

    public class SmtpMailSender : IMailSender {

        #region IMailSender Members

        public async Task Send(MailMessage mail) {
            using (var client = new SmtpClient()) {
                await client.SendMailAsync(mail);
            }
        }

        #endregion
    }
}

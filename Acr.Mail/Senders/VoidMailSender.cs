using System;
using System.Net.Mail;
using System.Threading.Tasks;


namespace Acr.Mail.Senders {

    public class VoidMailSender : IMailSender {

        #region IMailSender Members

        public async Task Send(MailMessage mail) {}

        #endregion
    }
}

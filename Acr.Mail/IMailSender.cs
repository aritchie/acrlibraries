using System;
using System.Net.Mail;
using System.Threading.Tasks;


namespace Acr.Mail {

    public interface IMailSender {

        Task Send(MailMessage mail);
    }
}

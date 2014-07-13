using System;
using System.Threading.Tasks;


namespace Acr.Mail {
    
    public interface IMailFactory {

        void Send(MailSendArgs args);
    }
}

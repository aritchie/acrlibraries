using System;
using System.Net.Mail;
using System.Threading.Tasks;


namespace Acr.Mail.Senders {
    
    public class DebugMailSender : IMailSender {
        public string DebugAddress { get; set; }

        public async Task Send(MailMessage mail) {
            mail.To.Clear();
            mail.CC.Clear();
            mail.Bcc.Clear();

            mail.To.Add(new MailAddress(this.DebugAddress));
            
            using (var smtp = new SmtpClient()) {
                await smtp.SendMailAsync(mail);
            }
        }
    }
}

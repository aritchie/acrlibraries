using System;
using System.Net.Mail;


namespace Acr.Mail {

    public static class Email {

        // pro tip from http://www.deanhume.com/Home/BlogPost/Validating%20email%20addresses%20in%20.NET%20-%20Handy%20Tip/5103?utm_source=feedburner&utm_medium=feed&utm_campaign=Feed%3A+DeanHumesBlog+%28Dean+Hume%27s+Blog%29
        public static bool IsValidAddress(string emailAddress) {
            if (String.IsNullOrWhiteSpace(emailAddress))
                return false;

            try {
                var mailAddress = new MailAddress(emailAddress);
                return true;
            }
            catch {
                return false;
            }
        }
    }
}

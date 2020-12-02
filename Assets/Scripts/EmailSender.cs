
using System.Net.Mail;
using System.Net;

namespace SecAuth {
    public class EmailSender {
            public static void sendEmailTo(string toEmail, string subject, string body) {
            //Thanks to docs.microsoft.com
            body += "\n\nRegards,\n" +
            "College Connections Team";

            MailAddress fromAddress = new MailAddress("csci380.smmmt@gmail.com");
            string fromPassword = "CSCI380EMAILPASSWORD";
            MailAddress toAddress = new MailAddress(toEmail);

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)       
            };

            using (var message = new MailMessage(fromAddress.Address, toAddress.Address, subject, body))
            smtp.Send(message);
        }
    }

}
using LanguageCenter.Core.Models.Email;
using LanguageCenter.Core.Services.Contracts;
using LanguageCenter.Core.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace LanguageCenter.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailServerConfiguration _mailSettings;

        public EmailService(IOptions<EmailServerConfiguration> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public bool SendEmail(Contact message)
        {
            try
            {

                var host = _mailSettings.Host;
                var port = int.Parse(_mailSettings.Port);
                var username = _mailSettings.Username;
                var password = _mailSettings.Password;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Host = host;
                    smtp.Port = port;
                    smtp.Credentials = new NetworkCredential(username, password);
                    // send the email

                    var email = new MailMessage(message.Email, username);

                    string innerConntent = "Name " + message.Name;
                    innerConntent += "<br>Phone: " + message.PhoneNumber;
                    innerConntent += "<br>Address: " + message.Email;
                    innerConntent += "<br>Content: " + message.Content;

                    email.Subject = message.Subject;
                    email.Body = innerConntent;
                    email.IsBodyHtml = true;

                    smtp.Send(email);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

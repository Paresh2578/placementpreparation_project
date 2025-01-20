using System.Net.Mail;
using System.Net;
using backend.Models;
using backend.Constant;

namespace backend.Service{
    public class EmailService{
        private readonly IConfiguration configuration;
        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<ResponseModel> SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var email = configuration.GetValue<string>("Email_configration:Email");
                var password = configuration.GetValue<string>("Email_configration:Password");
                var host = configuration.GetValue<string>("Email_configration:Host");
                var port = configuration.GetValue<int>("Email_configration:Port");

                using (var smtpClient = new SmtpClient(host, port))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(email, password);

                    // Use the CreateHtmlTemplate method to generate HTML content
                    var htmlBody = MailHtmlTemplet.CreateHtmlTemplate(body);

                    using (var mailMessage = new MailMessage(email!, toEmail, subject, htmlBody))
                    {
                        mailMessage.IsBodyHtml = true; // Enable HTML content rendering
                        await smtpClient.SendMailAsync(mailMessage);
                    }
                }

                return new ResponseModel { StatusCode = 200, Message = "Email sent successfully." };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    StatusCode = 500,
                    Message = $"Failed to send email. Error: {ex.Message}"
                };
            }
}

         
    }
}
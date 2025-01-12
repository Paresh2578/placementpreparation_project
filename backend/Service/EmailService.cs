using System.Net.Mail;
using System.Net;
using backend.Models;

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

              var smtpClient = new SmtpClient(host,port);
              smtpClient.EnableSsl = true;
              smtpClient.UseDefaultCredentials = false;
              smtpClient.Credentials = new NetworkCredential(email,password);

              var mailMessage = new MailMessage(email! , toEmail,subject,body);
              await smtpClient.SendMailAsync(mailMessage);

                return new ResponseModel { StatusCode = 200, Message = "Email sent successfully." };
            }catch(Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = "Fail to sent Otp" };
            }
        }
    }
}
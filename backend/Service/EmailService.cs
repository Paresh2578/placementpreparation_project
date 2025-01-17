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

                using (var smtpClient = new SmtpClient(host, port))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(email, password);

                    // Use the CreateHtmlTemplate method to generate HTML content
                    var htmlBody = CreateHtmlTemplate(body);

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

         private string CreateHtmlTemplate(string otp)
        {
           // Dynamically embed the content into the HTML template
          return $@"
            <div style=""font-family: Arial, sans-serif; background-color: #f9f9f9; margin: 0; padding: 0; color: #333333;"">
                <div style=""max-width: 600px; margin: 20px auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);"">
                    <!-- Header Section -->
                    <div style=""background-color: #4CAF50; padding: 20px; text-align: center; color: #ffffff;"">
                        <h1 style=""margin: 0; font-size: 24px;"">Placement Preparation</h1>
                    </div>

                    <!-- Content Section -->
                    <div style=""padding: 20px; text-align: center;"">
                        <h2 style=""font-size: 20px; color: #333333;"">Verify Your Email Address</h2>
                        <p>Thank you for signing up with <b>Placement Preparation</b>. To complete your registration, please use the OTP below to verify your email address.</p>

                        <!-- OTP Code -->
                        <div style=""display: inline-block; margin: 20px 0; font-size: 28px; font-weight: bold; color: #4CAF50; letter-spacing: 4px;"">{otp}</div>

                        <p>If you didn’t request this, please ignore this email.</p>
                    </div>

                    <!-- Footer Section -->
                    <div style=""background-color: #f1f1f1; padding: 10px 20px; text-align: center; font-size: 12px; color: #666666;"">
                        <p>&copy; 2025 Placement Preparation. All Rights Reserved.</p>
                        <p>
                            <a href=""#"" style=""color: #4CAF50; text-decoration: none;"">Privacy Policy</a> | <a href=""#"" style=""color: #4CAF50; text-decoration: none;"">Contact Support</a>
                        </p>
                    </div>
                </div>
            </div>";

  }
    }
}
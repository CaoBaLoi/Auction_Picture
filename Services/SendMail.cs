using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;

public class SendMail:IEmailSender{
    private readonly IConfiguration _configuration;

    public SendMail(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var smtpServer = _configuration["EmailSettings:SmtpServer"];
        var portString = _configuration["EmailSettings:Port"];
        var username = _configuration["EmailSettings:Username"];
        var password = _configuration["EmailSettings:Password"];

        if (int.TryParse(portString, out int port))
        {
            if(smtpServer != null && username != null){
                var smtpClient = new SmtpClient
            {
                Host = smtpServer,
                Port = port,
                EnableSsl = true,
                Credentials = new NetworkCredential(username, password)
            };

            var mailMessage = new MailMessage(username, email)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            await smtpClient.SendMailAsync(mailMessage);
            }
            else{
                throw new ArgumentException("SMTP server or username is null.");
            }
        }
        else
        {
            // Xử lý trường hợp port không hợp lệ
            throw new ArgumentException("Invalid port number in configuration.");
        }
    }
}
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Threading.Tasks;

namespace Dolphin_AI.Emailservice
{
    public class EmailService
    {
        public async Task SendWelcomeEmailAsync(string toEmail, string userName)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Dolphin-AI", "dolphinai@gmail.com"));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "🎉 Welcome to Dolphin-AI";

            message.Body = new TextPart("html")
            {
                Text = $@"
                <html>
                <body style='margin:0;padding:0;background:#f4f6ff;font-family:Arial;'>

                <table align='center' width='100%' cellpadding='0' cellspacing='0'>
                    <tr>
                        <td align='center'>

                            <table width='600' cellpadding='20' cellspacing='0' 
                                   style='background:white;border-radius:12px;box-shadow:0 4px 10px rgba(0,0,0,0.1);margin-top:40px;'>

                                <!-- Logo -->
                                <tr>
                                    <td align='center' style='background:linear-gradient(90deg,#6f42c1,#4e73df);border-radius:12px 12px 0 0;'>
                                        <img src='YOUR_LOGO_URL_HERE' 
                                             alt='Dolphin-AI' 
                                             width='120' 
                                             style='margin:15px 0;' />
                                    </td>
                                </tr>

                                <!-- Content -->
                                <tr>
                                    <td align='center'>
                                        <h2 style='color:#4e73df;'>Welcome to Dolphin-AI 🐬</h2>

                                        <p style='font-size:16px;color:#333;'>
                                            Hello <strong>{userName}</strong>,
                                        </p>

                                        <p style='font-size:15px;color:#555;'>
                                            Your account has been 
                                            <strong style='color:green;'>created successfully!</strong>
                                        </p>

                                        <a href='https://yourwebsite.com/login'
                                           style='display:inline-block;padding:12px 25px;
                                                  background:#4e73df;color:white;
                                                  text-decoration:none;border-radius:30px;
                                                  margin-top:20px;font-weight:bold;'>
                                            Login Now
                                        </a>

                                        <p style='margin-top:30px;font-size:12px;color:#999;'>
                                            © 2026 Dolphin-AI. All rights reserved.
                                        </p>
                                    </td>
                                </tr>

                            </table>

                        </td>
                    </tr>
                </table>

                </body>
                </html>"
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("shekhafjal024@gmail.com", "bgat soqx yfov fcud");
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);

        }
    }
}
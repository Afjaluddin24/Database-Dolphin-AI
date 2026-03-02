using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Dolphin_AI.Emailservice
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendWelcomeEmailAsync(string toEmail, string userName)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(
                "Dolphin-AI",
                _config["EmailSettings:Email"]
            ));

            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "🎉 Welcome to Dolphin-AI";

            message.Body = new TextPart("html")
            {
                Text = $@"
<!DOCTYPE html>
<html>
<head>
<meta charset='UTF-8'>
<meta name='viewport' content='width=device-width, initial-scale=1.0'>
</head>
<body style='margin:0;padding:0;background:#f4f6ff;font-family:Segoe UI,Arial,sans-serif;'>

<table width='100%' cellpadding='0' cellspacing='0' style='padding:40px 0;background:#f4f6ff;'>
<tr>
<td align='center'>

<table width='600' cellpadding='0' cellspacing='0' 
       style='background:#ffffff;border-radius:12px;overflow:hidden;
              box-shadow:0 4px 15px rgba(0,0,0,0.1);'>

<!-- Header -->
<tr>
<td align='center' style='background:linear-gradient(90deg,#6f42c1,#4e73df);
                          padding:30px;color:white;'>
    <h2 style='margin:0;font-size:24px;'>🐬 Dolphin-AI</h2>
</td>
</tr>

<!-- Body -->
<tr>
<td style='padding:40px;text-align:center;'>

    <h3 style='color:#4e73df;margin-bottom:10px;'>
        Welcome, {userName} 🎉
    </h3>

    <p style='font-size:16px;color:#555;line-height:1.6;'>
        Your account has been successfully created.
        We’re excited to have you onboard!
    </p>

    <a href='https://yourwebsite.com/login'
       style='display:inline-block;
              margin-top:25px;
              padding:12px 28px;
              background:#4e73df;
              color:white;
              text-decoration:none;
              border-radius:30px;
              font-weight:600;'>
        Login Now
    </a>

</td>
</tr>

<!-- Footer -->
<tr>
<td align='center' style='background:#f8f9fc;padding:20px;font-size:12px;color:#888;'>
    © 2026 Dolphin-AI | All Rights Reserved
</td>
</tr>

</table>

</td>
</tr>
</table>

</body>
</html>"
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                _config["EmailSettings:Host"],
                int.Parse(_config["EmailSettings:Port"]),
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                _config["EmailSettings:Email"],
                _config["EmailSettings:Password"]);

            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}
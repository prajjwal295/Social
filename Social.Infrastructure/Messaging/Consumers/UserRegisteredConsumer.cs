using MassTransit;
using Social.Infrastructure.Email;
using Social.Infrastructure.Messaging.Events;

namespace Social.Infrastructure.Messaging.Consumer
{
    public class UserRegisteredConsumer : IConsumer<UserRegisteredEvent>
    {
        private readonly IEmailService _emailService;

        public UserRegisteredConsumer(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            try
            {
                var message = context.Message;

                string subject = "Welcome to Social App";
                string htmlBody = BuildHtmlBody(message.FirstName, subject);

                await _emailService.SendEmailAsync(
                    message.Email,
                    subject,
                    htmlBody);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static string BuildHtmlBody(string firstName, string subject)
        {
            string body = $"Hello {firstName}, welcome to Social App! " +
                          "Your account is ready. Click the button below to get started.";

            return $@"<!DOCTYPE html>
<html>
<head>
  <meta charset='UTF-8'>
  <meta name='viewport' content='width=device-width,initial-scale=1.0'>
  <title>{subject}</title>
</head>
<body style='margin:0;padding:0;background-color:#0d0d0d;font-family:Arial,sans-serif;'>
  <table width='100%' cellpadding='0' cellspacing='0' border='0'>
    <tr>
      <td align='center' style='padding:40px 16px;'>
        <table width='600' cellpadding='0' cellspacing='0' border='0'
               style='background:#161616;border-radius:16px;overflow:hidden;'>

          <tr>
            <td style='height:4px;background:linear-gradient(90deg,#c9a84c,#f0d080,#c9a84c);'></td>
          </tr>

          <tr>
            <td align='center' style='padding:36px 40px 24px;'>
              <div style='display:inline-block;background:#1f1f1f;border:1px solid #2a2a2a;border-radius:12px;padding:10px 24px;'>
                <span style='color:#c9a84c;font-size:22px;font-weight:bold;letter-spacing:2px;'>
                  SOCIAL APP
                </span>
              </div>
            </td>
          </tr>

          <tr>
            <td style='padding:0 40px;'>
              <div style='height:1px;background:#2a2a2a;'></div>
            </td>
          </tr>

          <tr>
            <td style='padding:36px 40px;'>
              <h2 style='margin:0 0 16px;color:#ffffff;font-size:24px;font-weight:600;'>
                Welcome, {firstName} ✦
              </h2>
              <p style='margin:0 0 24px;font-size:15px;line-height:1.8;color:#a0a0a0;'>
                {body}
              </p>
              <div style='text-align:center;margin-top:32px;'>
                <a href='https://yourfrontendurl.com'
                   style='display:inline-block;background:#c9a84c;color:#0d0d0d;
                          padding:14px 36px;border-radius:8px;text-decoration:none;
                          font-weight:bold;font-size:15px;letter-spacing:0.5px;'>
                  Open App →
                </a>
              </div>
            </td>
          </tr>

          <tr>
            <td style='padding:0 40px;'>
              <div style='height:1px;background:#2a2a2a;'></div>
            </td>
          </tr>
          <tr>
            <td align='center' style='padding:24px 40px;color:#484848;font-size:12px;line-height:1.8;'>
              © 2026 Social App. All rights reserved.<br>
              <a href='#' style='color:#484848;text-decoration:underline;'>Unsubscribe</a> ·
              <a href='#' style='color:#484848;text-decoration:underline;'>Privacy Policy</a>
            </td>
          </tr>

        </table>
      </td>
    </tr>
  </table>
</body>
</html>";
        }
    }
}

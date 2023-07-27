using IdentityAuthWithJWT.Interfaces;
using IdentityAuthWithJWT.Models.Authentication;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Org.BouncyCastle.Asn1.Ocsp;

namespace IdentityAuthWithJWT.Services
{
	public class MailService : IMailService
	{
		private readonly MailSettings _mailSettings;

		public MailService(IOptions<MailSettings> mailSettings)
		{
			_mailSettings = mailSettings.Value;
		}

		public async Task<bool> SendWelcomeEmailAsync(string toEmail, string userName)
		{
			try
			{
				string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\WelcomeTemplate.html";
				using StreamReader str = new(FilePath);
				string mailHtmlText = str.ReadToEnd();
				mailHtmlText = mailHtmlText.Replace("[username]", userName).Replace("[email]", toEmail);
				var email = new MimeMessage
				{
					Sender = MailboxAddress.Parse(_mailSettings.Mail)
				};
				email.To.Add(MailboxAddress.Parse(toEmail));
				email.Subject = $"Welcome {userName}";
				var builder = new BodyBuilder
				{
					HtmlBody = mailHtmlText
				};
				email.Body = builder.ToMessageBody();
				using var smtp = new SmtpClient();
				smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
				smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
				await smtp.SendAsync(email);
				smtp.Disconnect(true);
				return true;
			}
			catch
			{
				return false;
			}


		}
	}
}

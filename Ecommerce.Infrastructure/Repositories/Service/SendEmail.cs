using Ecommerce.Core.DTO;
using Ecommerce.Core.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Ecommerce.Infrastructure.Repositories.Service
{
	public class SendEmail : ISendEmail
	{
		private readonly IConfiguration _configuration;

		public SendEmail(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task SendEmailAsync(EmailDTO emailDTO)
		{
			MimeMessage message = new MimeMessage();

			message.From.Add(new MailboxAddress("Ecommerce",_configuration["EmailSettings:From"]));
			message.Subject = emailDTO.Subject;
			message.To.Add(new MailboxAddress(emailDTO.To, emailDTO.To));
			message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
			{
				Text = emailDTO.Content
			};

			using (var client = new MailKit.Net.Smtp.SmtpClient())
			{
				try
				{
					await client.ConnectAsync(_configuration["EmailSettings:smtp"], 
						int.Parse(_configuration["EmailSettings:Port"]), true);

					await client.AuthenticateAsync(_configuration["EmailSettings:Username"],
						_configuration["EmailSettings:Password"]);

					await client.SendAsync(message);
				}
				catch (Exception ex)
				{
					throw;
				} finally
				{
                    client.Disconnect(true);
					client.Dispose();
				}
			}
		}
	}
}

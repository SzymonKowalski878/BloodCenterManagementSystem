using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Util;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace BloodCenterManagementSystem.Logics.Services
{
    public class EmailSender:IEmailSender
    {
        private readonly Lazy<EmailConfigModel> _emailConfig;
        protected EmailConfigModel EmailConfig => _emailConfig.Value;

        private readonly Lazy<IConfiguration> _configuration;
        protected IConfiguration Configuration => _configuration.Value;

        public EmailSender(Lazy<EmailConfigModel> emailConfig,
            Lazy<IConfiguration> configuration)
        {
            _emailConfig = emailConfig;
            _configuration = configuration;
        }

        private MimeMessage CreateEmailMessage(MessageModel message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(EmailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<h2 style ='color:red'>{0}</h2>", message.Content) };

            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments.Files)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            var secrets = new ClientSecrets
            {
                ClientId = Configuration.GetSection("GmailSmtpApi").GetSection("ClientId").Value,
                ClientSecret = Configuration.GetSection("GmailSmtpApi").GetSection("ClientSecret").Value
            };

            var fromEmail = Configuration.GetSection("EmailConfiguration").GetSection("From").Value;

            var googleCredentials = GoogleWebAuthorizationBroker.AuthorizeAsync(secrets, new[] { GmailService.Scope.MailGoogleCom }, fromEmail, CancellationToken.None).Result;

            if (googleCredentials.Token.IsExpired(SystemClock.Default))
            {
                googleCredentials.RefreshTokenAsync(CancellationToken.None);
            }

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(EmailConfig.SmtpServer, EmailConfig.Port, SecureSocketOptions.StartTls);
                    var OAuth2 = new SaslMechanismOAuth2(googleCredentials.UserId, googleCredentials.Token.AccessToken);
                    client.Authenticate(OAuth2);
                    client.Send(mailMessage);
                }
                catch(Exception ex)
                {
                    throw(ex);
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        public Result<bool> SendEmail(MessageModel message)
        {
            if (message == null)
            {
                return Result.Error<bool>("Data was null");
            }

            MimeMessage emailMessage;


            try
            {
                emailMessage = CreateEmailMessage(message);
            }
            catch(Exception ex)
            {
                return Result.Error<bool>(ex.Message);
            }

            try
            {
                Send(emailMessage);
            }
            catch (Exception ex)
            {
                return Result.Error<bool>(ex.Message);
            }

            return Result.Ok(true);
        }
    }
}

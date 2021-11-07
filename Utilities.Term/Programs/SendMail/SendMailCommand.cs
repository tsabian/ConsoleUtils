using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

#nullable enable
namespace Utilities.Term.Programs.SendMail
{
    public class SendMailCommand: ICommand
    {
        private readonly string[] _args;

        private CommandsStatus _status;
        public CommandsStatus Status => _status;

        public SendMailCommand(string[] args)
        {
            _args = args;
            _status = CommandsStatus.Started;
        }

        public async Task Execute()
        {
            try
            {
                var path = SendMailArgs.FilePathArg.GetArgValue<string>(_args);
                var body = await ReadFile(path);
                var credentials = GetLogin(_args);
                var subject = SendMailArgs.SubjectArg.GetArgValue<string>(_args);
                var destination = SendMailArgs.DestinationArg.GetArgValue<string>(_args);
                var attachment = SendMailArgs.AttachmentFilePathArg.GetArgValue<string>(_args);
                SendEmail(subject, body, destination, credentials, attachment);
                _status = CommandsStatus.Successfully;
            }
            catch (FileNotFoundException e)
            {
                _status = CommandsStatus.Failure;
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                _status = CommandsStatus.Failure;
                Console.WriteLine(e);
            }
        }

        private async Task<string?> ReadFile(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException(SendMailMessages.TemplatePathNotFound);
            using StreamReader stream = new(filePath);
            var fileBody = await stream.ReadToEndAsync();
            return await TagReplacing(fileBody);
        }

        private async Task<string?> TagReplacing(string body)
        {
            Dictionary<string, string>? replacementCollection = null;
            var replacementArg = SendMailArgs.ReplacementFilePathArg.GetArgValue<string>(_args);
            if (!string.IsNullOrEmpty(replacementArg) && string.IsNullOrWhiteSpace(replacementArg))
            {
                replacementCollection =
                    await Helpers.DeserializeJsonContentAsync<Dictionary<string, string>>(replacementArg);
            }
            return replacementCollection?.Aggregate(body, (current, tag) =>
                current.Replace(tag.Key, tag.Value));
        }

        private static NetworkCredential GetLogin(string[] args)
        {
            var username = SendEmailConstants.SmtpUserName;
            var password = SendEmailConstants.SmtpUserPassword;
            var paramUserName = SendMailArgs.UserNameArg.GetArgValue<string>(args);
            var paramUserPassword = SendMailArgs.UserPasswordArg.GetArgValue<string>(args);
            if (!string.IsNullOrEmpty(paramUserName) || !string.IsNullOrWhiteSpace(paramUserName))
            {
                username = paramUserName;
            }
            if (!string.IsNullOrEmpty(paramUserPassword) || !string.IsNullOrWhiteSpace(paramUserPassword))
            {
                password = paramUserPassword;
            }
            return new NetworkCredential(username, password);
        }

        private void SendEmail(string subject, string? body, string destination, NetworkCredential credential, 
            string? attachmentFilePath = null)
        {
            using SmtpClient client = new(SendEmailConstants.SmtpHost, SendEmailConstants.SmtpPort)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = SendEmailConstants.UseDefaultCredentials,
                EnableSsl = SendEmailConstants.UseSmtpSsl,
                Credentials = credential
            };

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            client.SendCompleted += Client_SendCompleted;

            var encoding = System.Text.Encoding.UTF8;
            var from = credential.UserName;
            var to = destination;
            
            MailMessage mailMessage = new(from, to)
            {
                Body = body,
                IsBodyHtml = SendEmailConstants.UseHtmlBody,
                Subject = subject,
                SubjectEncoding = encoding,
                BodyEncoding = encoding
            };

            if (!string.IsNullOrEmpty(attachmentFilePath) && !string.IsNullOrWhiteSpace(attachmentFilePath))
            {
                mailMessage.Attachments.Add(new Attachment(attachmentFilePath));
            }

            client.Send(mailMessage);
        }

        private static void Client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Console.WriteLine(SendMailMessages.MailMessageSentSuccessfullyMessage);
        }

        public void WriteHelp()
        {
            
        }
    }
}

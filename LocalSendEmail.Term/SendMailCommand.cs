using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace LocalSendEmail.Term
{
    internal class SendMailCommand: ICommand
    {

        private readonly string[] args;

        public SendMailCommand(string[] args)
        {
            this.args = args;
        }

        public void Execute()
        {
            try
            {
                string path = CommandArgsConstants.SendMailArgs.FilePathArg.GetArgValue<string>(args);
                string body = ReadFile(path);
                NetworkCredential credentials = GetLogin(args);
                string subject = CommandArgsConstants.SendMailArgs.SubjectArg.GetArgValue<string>(args);
                string destination = CommandArgsConstants.SendMailArgs.DestinationArg.GetArgValue<string>(args);
                string attachment = CommandArgsConstants.SendMailArgs.AttachmentFilePathArg.GetArgValue<string>(args);
                SendEmail(subject, body, destination, credentials, attachment);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private string ReadFile(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException(MessageConstants.TemplatePathNotFound);

            string fileBody = string.Empty;

            using (StreamReader stream = new(filePath))
            {
                fileBody = stream.ReadToEnd();
            }

            return fileBody;
        }

        private NetworkCredential GetLogin(string[] args)
        {
            string username = ProgramConstants.SmtpUserName;
            string password = ProgramConstants.SmtpUserPassword;
            string paramUserName = CommandArgsConstants.SendMailArgs.UserNameArg.GetArgValue<string>(args);
            string paramUserPassword = CommandArgsConstants.SendMailArgs.UserPasswordArg.GetArgValue<string>(args);

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

        private void SendEmail(string subject, string body, string destination, NetworkCredential credential, string attachmentFilePath = null)
        {
            using SmtpClient client = new(ProgramConstants.SmtpHost, ProgramConstants.SmtpPort)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = ProgramConstants.UseDefaultCredentials,
                EnableSsl = ProgramConstants.UseSmtpSsl,
                Credentials = credential
            };

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            client.SendCompleted += Client_SendCompleted;

            var encoding = System.Text.Encoding.UTF8;

            string from = credential.UserName;
            string to = destination;
            
            MailMessage mailMessage = new(from, to)
            {
                Body = body,
                IsBodyHtml = ProgramConstants.UseHtmlBody,
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

        private void Client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Console.WriteLine(MessageConstants.MailMessageSentSuccessfullyMessage);
        }
    }
}

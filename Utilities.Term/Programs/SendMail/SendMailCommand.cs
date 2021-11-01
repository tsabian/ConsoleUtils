using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Utilities.Term.Programs.SendMail
{
    internal class SendMailCommand: ICommand
    {
        private readonly string[] args;

        private readonly IReadOnlyDictionary<string, string> _templatePixNotification = new Dictionary<string, string>() {
            { "cid:Template-geral_01.png", "https://bitz-email-assets.s3.amazonaws.com/Template-geral_01.png" },
            { "${title}", "Transferência enviada via Pix" },
            { @"
                                                <tr>
                                                    <td align=""left"" style=""padding:0;padding-bottom:20px;word-break:break-word;"">
                                                        <div
                                                            style=""font-family: 'Ubuntu', sans-serif; font-size:22px; font-weight:bold; line-height:1.5; text-align:left;color:#ef3e64;"">
                                                            <p>Você recebeu R$ ${amount} via transferência Pix com sucesso</p>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align=""left"" style=""padding:0;padding-bottom:30px;word-break:break-word;"">
                                                        <p>
                                                        <div style=""font-family: 'Ubuntu' , sans-serif;  font-size:16px; weight: 500; line-height:2.0; text-align:left; color:#231F20;"">
                                                            <b>Dados do pagador</b>
                                                        </div>
                                                        <div style=""font-family: 'Ubuntu' , sans-serif;  font-size:16px; weight: 500; line-height:2.0; text-align:left; color:#231F20;"">
                                                            Nome: <b>${payerName}</b>
                                                        </div>
                                                        <div style=""font-family: 'Ubuntu' , sans-serif;  font-size:16px; weight: 500; line-height:2.0; text-align:left; color:#231F20;"">
                                                            CPF/CNPJ: <b>${payerDocument}</b>
                                                        </div>
                                                        <div style=""font-family: 'Ubuntu' , sans-serif;  font-size:16px; weight: 500; line-height:2.0; text-align:left; color:#231F20;"">
                                                            Instituição: <b>${payerBankName}</b>
                                                        </div>
                                                        <div style=""font-family: 'Ubuntu' , sans-serif;  font-size:16px; weight: 500; line-height:2.0; text-align:left; color:#231F20;"">
                                                            Agência nº: <b>${payerBranch}</b>
                                                        </div>
                                                        <div style=""font-family: 'Ubuntu' , sans-serif;  font-size:16px; weight: 500; line-height:2.0; text-align:left; color:#231F20;"">
                                                            Conta nº: <b>${payerAccount}</b>
                                                        </div>
                                                        <div style=""font-family: 'Ubuntu' , sans-serif;  font-size:16px; weight: 500; line-height:2.0; text-align:left; color:#231F20;"">
                                                            Tipo: <b>${payerAccountType}</b>
                                                        </div>
                                                        <div style=""font-family: 'Ubuntu' , sans-serif;  font-size:16px; weight: 500; line-height:2.0; text-align:left; color:#231F20;"">
                                                            Data e hora: <b>${transactionDate} às ${transactionHour}</b>
                                                        </div>
                                                        <div style=""font-family: 'Ubuntu' , sans-serif;  font-size:16px; weight: 500; line-height:2.0; text-align:left; color:#231F20;"">
                                                            Descrição: <b>${payerAnswer}</b>
                                                        </div>
                                                        </p>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align=""left"" style=""padding:0;padding-bottom:30px;word-break:break-word;"">
                                                        <p>
                                                        <div style=""font-family: 'Ubuntu' , sans-serif;  font-size:16px; weight: 500; line-height:2.0; text-align:left; color:#231F20;"">
                                                            <b>Dados do recebedor</b>
                                                        </div>
                                                        <div style=""font-family: 'Ubuntu' , sans-serif;  font-size:16px; weight: 500; line-height:2.0; text-align:left; color:#231F20;"">
                                                            Nome: <b>${receiverName}</b>
                                                        </div>
                                                        <div style=""font-family: 'Ubuntu' , sans-serif;  font-size:16px; weight: 500; line-height:2.0; text-align:left; color:#231F20;"">
                                                            Instituição: <b>${receiverBankName}</b>
                                                        </div>
                                                        </p>
                                                    </td>
                                                </tr>
", "${internalBody}" },
            { "cid:Template-geral_04.png", "https://bitz-email-assets.s3.amazonaws.com/Template-geral_04.png" },
            { "cid:Facebook.png", "https://bitz-email-assets.s3.amazonaws.com/Facebook.png" },
            { "cid:Linkedin.png", "https://bitz-email-assets.s3.amazonaws.com/Linkedin.png" },
            { "cid:Twitter.png", "https://bitz-email-assets.s3.amazonaws.com/Twitter.png" },
            { "cid:youtube.png", "https://bitz-email-assets.s3.amazonaws.com/youtube.png" },
            { "cid:Instagram.png", "https://bitz-email-assets.s3.amazonaws.com/Instagram.png" }
        };

        public SendMailCommand(string[] args)
        {
            this.args = args;
        }

        public async Task Execute()
        {
            try
            {
                var path = SendMailArgs.FilePathArg.GetArgValue<string>(args);
                var body = await ReadFile(path);
                Console.WriteLine(body);
                var credentials = GetLogin(args);
                var subject = SendMailArgs.SubjectArg.GetArgValue<string>(args);
                var destination = SendMailArgs.DestinationArg.GetArgValue<string>(args);
                var attachment = SendMailArgs.AttachmentFilePathArg.GetArgValue<string>(args);
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

        private async Task<string> ReadFile(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException(SendMailMessages.TemplatePathNotFound);
            using StreamReader stream = new(filePath);
            var fileBody = await stream.ReadToEndAsync();
            return TagReplacing(fileBody);
        }

        private string TagReplacing(string body)
        {
            return _templatePixNotification.Aggregate(body, (current, tag) =>
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

        private void SendEmail(string subject, string body, string destination, NetworkCredential credential, string attachmentFilePath = null)
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

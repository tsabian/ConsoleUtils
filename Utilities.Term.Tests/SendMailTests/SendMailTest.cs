using System.Threading.Tasks;
using Utilities.Term.Programs.SendMail;
using Xunit;

namespace Utilities.Term.Tests.SendMailTests
{
    public class SendMailTest
    {
        private const string SimpleTemplateTestFilePath = "./HtmlTemplates/SimpleTemplateTest.html";
        private const string SimpleReplacementTestFilePath = "./JsonTemplates/SimpleReplacementTemplateTest.json";
        
        [Fact]
        public async Task SendMailSuccessTest()
        {
            var args = new string[] { 
                "-d tsabian@hotmail.com", 
                $"-p {SimpleTemplateTestFilePath}",
                "-s Teste de envio de e-mail",
                $"-r {SimpleReplacementTestFilePath}"
            };
            var sendMailCommand = new SendMailCommand(args);
            await sendMailCommand.Execute();
         }
    }
}
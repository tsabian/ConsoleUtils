using System.IO;
using Utilities.Term.Programs.HttpFlow.Domains;
using Newtonsoft.Json;

#nullable enable
namespace Utilities.Term.Tests.HttpFlowTests.Fixture
{
    public static class RequestFlowCollectionFixtures
    {
        private const string CreateAccountFilePath = "../../JsonTemplates/CreateAccountTemplateTe.json";
        
        public static RequestFlowCollection? GetDefaultValue
        {
            get
            {
                using var stream = new StreamReader(CreateAccountFilePath);
                var json = stream.ReadToEnd();
                stream.Close();
                return JsonConvert.DeserializeObject<RequestFlowCollection>(json);
            }
        }
    }
}
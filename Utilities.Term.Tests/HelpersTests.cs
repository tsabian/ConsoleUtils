using System.Threading.Tasks;
using Utilities.Term.Programs;
using Utilities.Term.Programs.HttpFlow.Domains;
using Xunit;

namespace Utilities.Term.Tests
{
    public class HelpersTests
    {
        private const string CreateAccountFilePath = "./JsonTemplates/CreateAccountTemplate.json";
        
        [Fact]
        public async Task DeserializeObjectSuccessTest()
        {
            var allRequests = Helpers
                .DeserializeJsonContentAsync<RequestFlowCollection>(CreateAccountFilePath);
            Assert.NotNull(allRequests);
        }
    }
}
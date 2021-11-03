using System.Linq;
using System.Threading.Tasks;
using Moq;
using Utilities.Term.Programs.CreateAccount;
using Utilities.Term.Programs.HttpFlow;
using Xunit;

namespace Utilities.Term.Tests.HttpFlowTests
{
    public class HttpFlowStartTest
    {
        private const string CreateAccountFilePath = "../JsonTemplates/CreateAccountTemplateTe.json";
        
        public HttpFlowStartTest()
        {
            _httpFlowMock = new Mock<HttpFlow>();
        }
        
        private readonly Mock<HttpFlow> _httpFlowMock;
        
        [Fact]
        public async Task StartFlowTestAsync()
        {
            var allRequests = Fixture.RequestFlowCollectionFixtures.GetDefaultValue;
            Assert.NotNull(allRequests);
            var firstRequest = allRequests.FirstOrDefault();
            _httpFlowMock.Setup(_ => _.StartFlowAsync(allRequests, firstRequest));
            var args = new[]
            {
                $"-f {CreateAccountFilePath}"
            };
            var sut = new CreateAccountCommand(args, _httpFlowMock.Object);
            await sut.Execute();
        }
    }
}
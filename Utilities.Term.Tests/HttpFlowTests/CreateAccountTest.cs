using System.Threading.Tasks;
using Utilities.Term.Programs.CreateAccount;
using Xunit;

namespace Utilities.Term.Tests.HttpFlowTests
{
    public class HttpFlowStartTest
    {
        [Fact]
        public async Task StartFlowTestAsync()
        {
            var args = new[] {"--file", "./createAccount.json"};
            var sut = new CreateAccountCommand(args);
            await sut.Execute();
        }
    }
}
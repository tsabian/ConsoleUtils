using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Term.Programs.HttpFlow;
using Utilities.Term.Programs.HttpFlow.Domains;

#nullable enable
namespace Utilities.Term.Programs.CreateAccount
{
    public class CreateAccountCommand : ICommand
    {
        private readonly string[] _args;
        private readonly HttpFlow.HttpFlow _httpFlow;

        public CommandsStatus Status { get; }

        public CreateAccountCommand(string[] args, HttpFlow.HttpFlow? httpFlow = null)
        {
            _args = args;
            _httpFlow = httpFlow ?? new HttpFlow.HttpFlow();
            Status = CommandsStatus.Started;
        }

        public async Task Execute()
        {
            var filePath = HttpFlowArgs.FullFilePathArgs.GetArgValue<string>(_args);
            var allRequestFlows = await Helpers.DeserializeJsonContentAsync<RequestFlowCollection>(filePath);
            if (allRequestFlows == null) return;
            var firstRequest = allRequestFlows.FirstOrDefault();
            await _httpFlow.StartFlowAsync(allRequestFlows, firstRequest);
        }

        public void Cancel(Action callback)
        {
            _httpFlow.Cancel();
        }

        public void WriteHelp()
        {

        }
    }
}

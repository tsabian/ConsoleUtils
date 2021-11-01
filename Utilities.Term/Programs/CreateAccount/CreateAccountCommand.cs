using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
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
        
        public CreateAccountCommand(string[] args, HttpFlow.HttpFlow? httpFlow = null)
        {
            _args = args;
            _httpFlow = httpFlow ?? new HttpFlow.HttpFlow();
        }

        public async Task Execute()
        {
            var filePath = HttpFlowArgs.FullFilePathArgs.GetArgValue<string>(_args);
            var json = await GetJsonContent(filePath);
            var allRequestFlows = JsonSerializer.Deserialize<RequestFlowCollection>(json ?? "[]") ?? 
                                  new RequestFlowCollection();
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

        private static async Task<string?> GetJsonContent(string path)
        {
            if (!File.Exists(path)) return null;
            using StreamReader reader = new(path);
            var jsonContent = await reader.ReadToEndAsync();
            reader.Close();
            return jsonContent;
        }
    }
}

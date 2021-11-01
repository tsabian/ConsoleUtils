using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Utilities.Term.Programs.HttpFlow.Domains;

#nullable enable
namespace Utilities.Term.Programs.HttpFlow
{
    public sealed class HttpFlow : IHttpFlow
    {
        private HttpClient Client { get; }
        private readonly CancellationTokenSource _cancellationToken;

        public HttpFlow(DecompressionMethods decompressionMethods = DecompressionMethods.GZip | DecompressionMethods.Deflate)
        {
            var customHandler = new HttpClientCustomHandler { AutomaticDecompression = decompressionMethods };
            Client = new HttpClient(customHandler);
            _cancellationToken = new CancellationTokenSource();
        }

        public async Task StartFlowAsync(RequestFlowCollection allRequests, RequestFlow current)
        {
            while (true)
            {
                if (_cancellationToken.IsCancellationRequested) return;

                var response = await Request(current, _cancellationToken.Token);

                if (current.DelayAfter != null) Thread.Sleep(current.DelayAfter ?? TimeSpan.Zero);

                if (current.ExpectedResults == null) return;

                var next = allRequests.Next(current, response.StatusCode);
                if (next.HasValue)
                {
                    current = next.Value;
                    continue;
                }

                break;
            }
        }

        public void Cancel()
        {
            _cancellationToken.Cancel();
        }

        private async Task<ResponseFlow> Request(RequestFlow requestFlow, CancellationToken cancellationToken)
        {
            Client.DefaultRequestHeaders.Clear();
            foreach (var (key, value) in requestFlow.Headers ?? new Dictionary<string, string>())
            {
                Client.DefaultRequestHeaders.Add(key, value);
            }
            var uri = new Uri(requestFlow.FullUrl);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var httpResponse = await Client.SendAsync(request, cancellationToken);
            var response = new ResponseFlow
            {
                StatusCode = (int) httpResponse.StatusCode
            };
            return response;
        }
    }
}

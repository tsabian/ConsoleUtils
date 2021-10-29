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
    internal class HttpFlow
    {
        private HttpClient Client { get; }

        internal HttpFlow(DecompressionMethods decompressionMethods = DecompressionMethods.GZip | DecompressionMethods.Deflate)
        {
            var customHandler = new HttpClientCustomHandler { AutomaticDecompression = decompressionMethods };
            Client = new HttpClient(customHandler);
        }

        internal async Task<ResponseFlow> Request(RequestFlow requestFlow, CancellationToken cancellationToken)
        {
            Client.DefaultRequestHeaders.Clear();
            foreach (var header in requestFlow.Headers ?? new Dictionary<string, string>())
            {
                Client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
            var uri = new Uri(requestFlow.FullUrl);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var httpResponse = await Client.SendAsync(request, cancellationToken);
            var response = new ResponseFlow()
            {
                StatusCode = (int) httpResponse.StatusCode
            };
            return response;
        }
    }
}

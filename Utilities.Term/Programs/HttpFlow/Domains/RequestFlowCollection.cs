using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Utilities.Term.Programs.HttpFlow.Domains
{
    public class RequestFlowCollection : List<RequestFlow>
    {
        public RequestFlow? this[string name]
        {
            get
            {
                var current = Find(_ => _.Name.Equals(name));
                return current;
            }
        }

        public RequestFlow? Next(RequestFlow current, int statusCode)
        {
            var expected = current.ExpectedResults?.FirstOrDefault(_ => _.StatusCode.Equals(statusCode));
            return expected.HasValue ? this[expected.Value.CallbackName] : default;
        }
    }
}
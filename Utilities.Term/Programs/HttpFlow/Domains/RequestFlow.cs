using System;
using System.Collections.Generic;

#nullable enable
namespace Utilities.Term.Programs.HttpFlow.Domains
{
    public struct RequestFlow
    {
        public string Name { get; set; }
        public Dictionary<string, string>? Headers { get; set; }
        public object Body { get; set; }
        public string FullUrl { get; set; }
        public IEnumerable<ExpectedFlow>? ExpectedResults { get; set; }
        public TimeSpan? DelayAfter { get; set; }
    }
}

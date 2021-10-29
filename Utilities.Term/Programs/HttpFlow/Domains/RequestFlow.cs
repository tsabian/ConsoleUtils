using System.Collections.Generic;

namespace Utilities.Term.Programs.HttpFlow.Domains
{
    internal class RequestFlow
    {
        public string Name { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string FullUrl { get; set; }
    }
}

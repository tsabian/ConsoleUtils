using System.Threading.Tasks;
using Utilities.Term.Programs.HttpFlow.Domains;

namespace Utilities.Term.Programs.HttpFlow
{
    public interface IHttpFlow
    {
        Task StartFlowAsync(RequestFlowCollection allRequests, RequestFlow current);
        void Cancel();
    }
}
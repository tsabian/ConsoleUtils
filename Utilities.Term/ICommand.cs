using System.Threading.Tasks;

namespace Utilities.Term
{
    internal interface ICommand
    {
        CommandsStatus Status { get; }
        Task Execute();
        void WriteHelp();
    }
}

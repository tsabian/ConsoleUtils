using System.Threading.Tasks;

namespace Utilities.Term
{
    internal interface ICommand
    {
        Task Execute();
        void WriteHelp();
    }
}

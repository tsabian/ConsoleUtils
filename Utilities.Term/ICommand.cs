namespace Utilities.Term
{
    public interface ICommand
    {
        void Execute();
        string WriteHelp();
    }
}

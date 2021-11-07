namespace Utilities.Term
{
    internal enum Commands {
        SendMail,
        CreateAccount
    }

    public enum CommandsStatus
    {
        Started,
        Successfully,
        Failure
    }
}
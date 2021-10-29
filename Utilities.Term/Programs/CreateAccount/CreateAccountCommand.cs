namespace Utilities.Term.Programs.CreateAccount
{
    internal class CreateAccountCommand : ICommand
    {
        private string[] args { get; }

        internal CreateAccountCommand(string[] args)
        {
            this.args = args;
        }

        public void Execute()
        {
            throw new System.NotImplementedException();
        }

        public string WriteHelp()
        {
            throw new System.NotImplementedException();
        }
    }
}

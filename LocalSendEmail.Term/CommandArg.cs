namespace LocalSendEmail.Term
{
    internal struct CommandArg
    {
        internal string Name { get; private set; }
        internal string ShortName { get; private set; }
        internal string Description { get; private set; }
        internal string HelpDescription { get; private set; }

        internal CommandArg(string name, string shortName, string description, string helpDescription)
        {
            Name = name;
            ShortName = shortName;
            Description = description;
            HelpDescription = helpDescription;
        }
    }
}

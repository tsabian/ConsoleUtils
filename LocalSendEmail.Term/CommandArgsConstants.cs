namespace LocalSendEmail.Term
{
    internal struct CommandArgsConstants
    {
        internal struct SendMailArgs
        {
            internal static CommandArg DestinationArg => new("--destination", "-d", "Destination", "Set destination mail address");
            internal static CommandArg FilePathArg => new("--path", "-p", "File Path", "Set template file path");
            internal static CommandArg UserNameArg => new("--username", "-u", "User Name", "Set user name");
            internal static CommandArg UserPasswordArg => new("--password", "-w", "User password", "Set user password");
            internal static CommandArg SubjectArg => new("--subject", "-s", "Mail subject", "Set mail subject");
            internal static CommandArg AttachmentFilePathArg => new("--attach", "-a", "Attach File", "Attach file");
        }
    }
}

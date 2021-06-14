using System;
namespace Utilities.Term
{
    internal class CommandNullValueException : Exception
    {
        private readonly CommandArg command;

        internal CommandNullValueException(CommandArg command) : base(message: $"Ncessário informar o valor do commando {command.Name} ou {command.ShortName}")
        {
            this.command = command;
        }
    }
}
